using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.CustomTranslate.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWeb.Server;
using ZKWeb.Web;
using ZKWeb.Localize;
using ZKWeb.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Cache;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding {
	/// <summary>
	/// 自定义翻译器
	/// 可以在后台设置中设置自定义翻译
	/// 注册时必须添加SingletonReuse属性否则影响性能
	/// <example>
	/// [ExportMany, SingletonReuse]
	/// public class Chinese : CustomTranslator {
	///		public override string Name { get { return "zh-CN"; } }
	/// }
	/// </example>
	/// </summary>
	public abstract class CustomTranslator :
		AdminSettingsCrudPageBuilder<Translation>, ITranslateProvider, ICacheCleaner {
		public override string Group { get { return "CustomTranslate"; } }
		public override string GroupIconClass { get { return "fa fa-language"; } }
		public override string IconClass { get { return "fa fa-language"; } }
		public override string Url { get { return "/admin/settings/custom_translate/" + Name.ToLower(); } }
		public virtual string DeleteUrl { get { return Url + "/delete"; } }
		public override string BatchUrl { get { return null; } }
		public override string[] ViewPrivileges { get { return new[] { "CustomTranslate:" + Name }; } }
		public override string[] EditPrivileges { get { return ViewPrivileges; } }
		public override string[] DeletePrivileges { get { return ViewPrivileges; } }
		public override string[] DeleteForeverPrivilege { get { return ViewPrivileges; } }
		protected override IAjaxTableCallback<Translation> GetTableCallback() { throw new NotSupportedException(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(this); }
		protected override IModelFormBuilder GetEditForm() { return new Form(this); }

		/// <summary>
		/// 列表页的处理函数
		/// </summary>
		/// <returns></returns>
		protected override IActionResult Action() {
			// 检查权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, RequiredPrivileges);
			// 表格构建器
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = ListTableId;
			table.Target = SearchUrl;
			table.MenuItems.AddDivider();
			table.MenuItems.AddEditAction("Translation", EditUrl, dialogParameters: new { size = "size-wide" });
			table.MenuItems.AddAddAction("Translation", AddUrl, dialogParameters: new { size = "size-wide" });
			// 搜索栏构建器
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
			// 检查权限
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, RequiredPrivileges);
			// 获取参数并转换到搜索请求
			var json = HttpManager.CurrentContext.Request.Get<string>("json");
			var request = AjaxTableSearchRequest.FromJson(json);
			// 构建搜索回应
			var query = Translates.Select(t => new Translation() { Original = t.Key, Translated = t.Value });
			if (!string.IsNullOrEmpty(request.Keyword)) {
				query = query.Where(q => q.Original.Contains(request.Keyword) || q.Translated.Contains(request.Keyword));
			}
			var response = new AjaxTableSearchResponse();
			var result = response.Pagination.Paging(request, query);
			response.PageNo = request.PageNo;
			response.PageSize = request.PageSize;
			response.Rows.AddRange(result.Select(translation => new Dictionary<string, object>() {
				{ "Id", HttpUtils.UrlEncode(translation.Original) },
				{ "OriginalText", translation.Original },
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
			// 检查权限
			HttpRequestChecker.RequieAjaxRequest();
			var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
			privilegeManager.Check(AllowedUserTypes, RequiredPrivileges);
			// 获取参数并执行删除
			var json = HttpManager.CurrentContext.Request.Get<string>("json");
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
			/// 属于的翻译器
			/// </summary>
			public CustomTranslator Translator { get; set; }
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
			/// <param name="translator">属于的翻译器</param>
			public Form(CustomTranslator translator) {
				Translator = translator;
			}

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				OriginalText = HttpUtils.UrlDecode(HttpManager.CurrentContext.Request.Get<string>("Id"));
				TranslatedText = Translator.Translates.GetOrDefault(OriginalText);
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var oldOriginalText = HttpUtils.UrlDecode(HttpManager.CurrentContext.Request.Get<string>("Id"));
				if (!string.IsNullOrEmpty(oldOriginalText)) {
					Translator.Translates.Remove(oldOriginalText);
				}
				if (!string.IsNullOrEmpty(OriginalText)) {
					Translator.Translates[OriginalText] = TranslatedText ?? "";
				}
				Translator.Translates = Translator.Translates;
				return new {
					message = new T("Saved Successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}
		}
	}
}
