using DryIoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.AdminSettings.src;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.CustomTranslate.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Server;
using ZKWeb.Web.Interfaces;
using ZKWeb.Localize;
using ZKWeb.Web;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Cache.Interfaces;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Scaffolding {
	/// <summary>
	/// 自定义翻译器
	/// 可以在后台设置中设置自定义翻译
	/// 例子，必须添加SingletonReuse属性否则影响性能
	/// [ExportMany, SingletonReuse]
	/// public class Chinese : CustomTranslator {
	///		public override string Name { get { return "zh-CN"; } }
	/// }
	/// </summary>
	public abstract class CustomTranslator :
		GenericListForAdminSettings<Translation, CustomTranslator>, ITranslateProvider, ICacheCleaner {
		/// <summary>
		/// 使用的权限
		/// </summary>
		public override string Privilege { get { return "CustomTranslate:" + Name; } }
		/// <summary>
		/// 所属分组
		/// </summary>
		public override string Group { get { return "CustomTranslate"; } }
		/// <summary>
		/// 分组图标
		/// </summary>
		public override string GroupIcon { get { return "fa fa-language"; } }
		/// <summary>
		/// 图标的Css类
		/// </summary>
		public override string IconClass { get { return "fa fa-language"; } }
		/// <summary>
		/// Url地址
		/// </summary>
		public override string Url { get { return "/admin/settings/custom_translate/" + Name.ToLower(); } }
		/// <summary>
		/// 添加使用的Url地址
		/// </summary>
		public virtual string AddUrl { get { return Url + "/add"; } }
		/// <summary>
		/// 编辑使用的Url地址
		/// </summary>
		public virtual string EditUrl { get { return Url + "/edit"; } }
		/// <summary>
		/// 删除使用的Url地址
		/// </summary>
		public virtual string DeleteUrl { get { return Url + "/delete"; } }
		/// <summary>
		/// 获取表格回调，这里不使用
		/// </summary>
		/// <returns></returns>
		protected override IAjaxTableCallback<Translation> GetTableCallback() {
			throw new NotSupportedException();
		}

		/// <summary>
		/// 列表页的处理函数
		/// </summary>
		/// <returns></returns>
		protected override IActionResult Action() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 表格构建器
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = TableId;
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
			return new TemplateResult(TemplatePath, new {
				title = new T(Name),
				iconClass = IconClass,
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
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 获取参数并转换到搜索请求
			var json = HttpContextUtils.CurrentContext.Request.Get<string>("json");
			var request = AjaxTableSearchRequest.FromJson(json);
			// 构建搜索回应
			var query = Translates.Select(t => new Translation() { Original = t.Key, Translated = t.Value });
			if (!string.IsNullOrEmpty(request.Keyword)) {
				query = query.Where(q => q.Original.Contains(request.Keyword) || q.Translated.Contains(request.Keyword));
			}
			var response = new AjaxTableSearchResponse();
			var result = response.Pagination.Paging(request, query);
			response.PageIndex = request.PageIndex;
			response.PageSize = request.PageSize;
			response.Rows.AddRange(result.Select(translation => new Dictionary<string, object>() {
				{ "Id", HttpUtility.UrlEncode(translation.Original) },
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
		/// 添加翻译内容
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult AddAction() {
			return EditAction();
		}

		/// <summary>
		/// 编辑翻译内容
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult EditAction() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 处理表单绑定或提交
			var form = new Form(this);
			var request = HttpContextUtils.CurrentContext.Request;
			if (request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult("common.admin/generic_edit.html", new { form });
			}
		}

		/// <summary>
		/// 删除翻译内容
		/// </summary>
		/// <returns></returns>
		protected virtual IActionResult DeleteAction() {
			// 检查权限
			HttpRequestChecker.RequieAjaxRequest();
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 获取参数并执行删除
			var json = HttpContextUtils.CurrentContext.Request.Get<string>("json");
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
			controllerManager.RegisterAction(AddUrl, HttpMethods.GET, AddAction);
			controllerManager.RegisterAction(AddUrl, HttpMethods.POST, AddAction);
			controllerManager.RegisterAction(EditUrl, HttpMethods.GET, EditAction);
			controllerManager.RegisterAction(EditUrl, HttpMethods.POST, EditAction);
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
				OriginalText = HttpUtility.UrlDecode(HttpContextUtils.CurrentContext.Request.Get<string>("Id"));
				TranslatedText = Translator.Translates.GetOrDefault(OriginalText);
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var oldOriginalText = HttpUtility.UrlDecode(HttpContextUtils.CurrentContext.Request.Get<string>("Id"));
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
