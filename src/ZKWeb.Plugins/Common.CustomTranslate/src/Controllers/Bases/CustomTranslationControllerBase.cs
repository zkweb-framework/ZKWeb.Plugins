using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWeb.Localize;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Web;
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.HtmlItems.Extensions;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Server;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases {
	/// <summary>
	/// 自定义翻译的控制器基础类，同时也是翻译提供器
	/// 可以在后台设置中设置自定义翻译
	/// 注册时必须添加SingletonReuse属性否则影响性能
	/// </summary>
	public abstract class CustomTranslationControllerBase :
		CrudAdminSettingsControllerBase<CustomTranslation, string>,
		ITranslateProvider, ICacheCleaner {
		public override string Group { get { return "CustomTranslate"; } }
		public override string GroupIconClass { get { return "fa fa-language"; } }
		public override string IconClass { get { return "fa fa-language"; } }
		public override string Url { get { return "/admin/settings/custom_translate/" + Name.ToLower(); } }
		public virtual string DeleteUrl { get { return Url + "/delete"; } }
		public override string BatchUrl { get { return null; } }
		public override string[] ViewPrivileges { get { return new[] { "CustomTranslate:" + Name }; } }
		public override string[] EditPrivileges { get { return ViewPrivileges; } }
		public override string[] DeletePrivileges { get { return ViewPrivileges; } }
		public override string[] DeleteForeverPrivileges { get { return ViewPrivileges; } }
		protected override IAjaxTableHandler<CustomTranslation, string> GetTableHandler() { throw new NotSupportedException(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(this); }
		protected override IModelFormBuilder GetEditForm() { return new Form(this); }

		/// <summary>
		/// 列表页的处理函数
		/// </summary>
		/// <returns></returns>
		protected override IActionResult Action() {
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = ListTableId;
			table.Target = SearchUrl;
			table.MenuItems.AddDivider();
			table.MenuItems.AddEditAction("Translation", EditUrl, dialogParameters: new { size = "size-wide" });
			table.MenuItems.AddAddAction("Translation", AddUrl, dialogParameters: new { size = "size-wide" });
			var searchBar = Application.Ioc.Resolve<AjaxTableSearchBarBuilder>();
			searchBar.TableId = table.Id;
			searchBar.KeywordPlaceHolder = "Origin/Translated";
			searchBar.MenuItems.AddDivider();
			searchBar.MenuItems.AddAddAction("Translation", AddUrl, dialogParameters: new { size = "size-wide" });
			searchBar.BeforeItems.AddAddAction("Translation", AddUrl, dialogParameters: new { size = "size-wide" });
			return new TemplateResult(ListTemplatePath, new {
				title = new T(Name),
				extra = ExtraTemplateArguments,
				table,
				searchBar
			});
		}

		/// <summary>
		/// 搜索请求的处理函数
		/// </summary>
		/// <returns></returns>
		protected override IActionResult SearchAction() {
			var json = Request.Get<string>("json");
			var request = AjaxTableSearchRequest.FromJson(json);
			var query = Translates.Select(t =>
				new CustomTranslation() { Id = t.Key, Translated = t.Value });
			if (!string.IsNullOrEmpty(request.Keyword)) {
				query = query.Where(q =>
					q.Id.Contains(request.Keyword) || q.Translated.Contains(request.Keyword));
			}
			var response = new AjaxTableSearchResponse();
			var result = response.Pagination.Paging(request, query.AsQueryable());
			response.PageNo = request.PageNo;
			response.PageSize = request.PageSize;
			response.Rows.AddRange(result.Select(translation => new Dictionary<string, object>() {
				{ "Id", HttpUtils.UrlEncode(translation.Id) },
				{ "OriginalText", translation.Id },
				{ "TranslatedText", translation.Translated },
				{ "ToString", translation.ToString() }
			}));
			response.Columns.AddNoColumn();
			response.Columns.AddMemberColumn("OriginalText");
			response.Columns.AddMemberColumn("TranslatedText");
			var actionColumn = response.Columns.AddActionColumn("130");
			actionColumn.AddEditAction("Translation", EditUrl, dialogParameters: new { size = "size-wide" });
			actionColumn.AddDeleteAction("Translation", DeleteUrl);
			return new JsonResult(response);
		}

		/// <summary>
		/// 删除翻译内容
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult DeleteAction() {
			this.RequireAjaxRequest();
			var json = Request.Get<string>("json");
			var original = JsonConvert.DeserializeObject<IList<string>>(json).FirstOrDefault();
			if (!string.IsNullOrEmpty(original)) {
				Translates.Remove(original);
			}
			Translates = Translates;
			return new JsonResult(new { message = new T("Delete Successful") });
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public override void OnWebsiteStart() {
			base.OnWebsiteStart();
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(DeleteUrl, HttpMethods.POST, DeleteAction);
		}

		/// <summary>
		/// 保存和载入翻译内容的路径
		/// </summary>
		public virtual string TranslatesPath {
			get {
				var pathManager = Application.Ioc.Resolve<PathManager>();
				return pathManager.GetStorageFullPath(
					"texts", "common.custom_translate", string.Format("{0}.json", Name));
			}
		}

		/// <summary>
		/// 翻译内容
		/// 获取时从文件载入，设置时保存到文件
		/// </summary>
		public virtual Dictionary<string, string> Translates {
			get {
				if (_Translates == null) {
					var path = TranslatesPath;
					_Translates = File.Exists(path) ?
						JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path)) :
						new Dictionary<string, string>();
				}
				return _Translates;
			}
			set {
				var path = TranslatesPath;
				PathUtils.EnsureParentDirectory(path);
				File.WriteAllText(path, JsonConvert.SerializeObject(
					_Translates = value ?? new Dictionary<string, string>(), Formatting.Indented));
			}
		}
		protected Dictionary<string, string> _Translates { get; set; }

		/// <summary>
		/// 判断是否可以翻译指定语言
		/// </summary>
		public virtual bool CanTranslate(string code) {
			return code == Name;
		}

		/// <summary>
		/// 翻译文本
		/// </summary>
		public virtual string Translate(string text) {
			return Translates.GetOrDefault(text);
		}

		/// <summary>
		/// 清理缓存
		/// 手动编辑后可以清理缓存让翻译器读取文件内容
		/// </summary>
		public virtual void ClearCache() {
			_Translates = null;
		}

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 属于的控制器
			/// </summary>
			public CustomTranslationControllerBase Controller { get; set; }
			/// <summary>
			/// 原文
			/// </summary>
			[TextAreaField("OriginalText", 3, "OriginalText")]
			public string OriginalText { get; set; }
			/// <summary>
			/// 译文
			/// </summary>
			[TextAreaField("TranslatedText", 3, "TranslatedText")]
			public string TranslatedText { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			/// <param name="controller">属于的控制器</param>
			public Form(CustomTranslationControllerBase controller) {
				Controller = controller;
			}

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				OriginalText = HttpUtils.UrlDecode(Request.Get<string>("Id"));
				TranslatedText = Controller.Translates.GetOrDefault(OriginalText);
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				// 删除旧的原文再添加新的原文
				var oldOriginalText = HttpUtils.UrlDecode(Request.Get<string>("Id"));
				if (!string.IsNullOrEmpty(oldOriginalText)) {
					Controller.Translates.Remove(oldOriginalText);
				}
				if (!string.IsNullOrEmpty(OriginalText)) {
					Controller.Translates[OriginalText] = TranslatedText ?? "";
				}
				Controller.Translates = Controller.Translates;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
