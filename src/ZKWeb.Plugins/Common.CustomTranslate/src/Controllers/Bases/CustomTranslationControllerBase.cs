using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWeb.Localize;
using ZKWeb.Cache;
using ZKWebStandard.Web;
using ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Entities;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.CustomTranslate.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;

namespace ZKWeb.Plugins.Common.CustomTranslate.src.Controllers.Bases {
	/// <summary>
	/// 自定义翻译的控制器基础类
	/// 可以在后台设置中设置自定义翻译
	/// 注册时必须添加SingletonReuse属性否则影响性能
	/// </summary>
	/// <typeparam name="TController">继承这个类的类型</typeparam>
	public abstract class CustomTranslationControllerBase<TController> :
		CrudAdminSettingsControllerBase<CustomTranslation, Guid>,
		ITranslateProvider, ICacheCleaner
		where TController : CustomTranslationControllerBase<TController>, new() {
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
		protected override IAjaxTableHandler<CustomTranslation, Guid> GetTableHandler() { throw new NotSupportedException(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(this); }
		protected override IModelFormBuilder GetEditForm() { return new Form(this); }

		/// <summary>
		/// 当前语言的所有翻译内容
		/// </summary>
		public virtual IDictionary<string, string> Translates {
			get {
				if (_Translates == null) {
					var manager = Application.Ioc.Resolve<CustomTranslationManager>();
					_Translates = manager.GetManyForLanguage(Name);
				}
				return _Translates;
			}
		}
		protected IDictionary<string, string> _Translates { get; set; }

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
		/// 表格处理器
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<CustomTranslation, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<TController>();
				searchBar.StandardSetupFor<TController>("Title/Summary/Author");
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<CustomTranslation> query) {
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Original.Contains(request.Keyword) ||
						q.Translated.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<CustomTranslation>> pairs) {
				foreach (var pair in pairs) {
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Original"] = pair.Entity.Original;
					pair.Row["Translated"] = pair.Entity.Translated;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<TController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Original");
				response.Columns.AddMemberColumn("Translated");
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.StandardSetupFor<TController>(request);
			}
		}

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : TabEntityFormBuilder<CustomTranslation, Guid, Form> {
			/// <summary>
			/// 属于的控制器
			/// </summary>
			public CustomTranslationControllerBase<TController> Controller { get; set; }
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
			public Form(CustomTranslationControllerBase<TController> controller) {
				Controller = controller;
			}

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(CustomTranslation bindFrom) {
				OriginalText = bindFrom.Original;
				TranslatedText = bindFrom.Translated;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit(CustomTranslation saveTo) {
				saveTo.Original = OriginalText;
				saveTo.Translated = TranslatedText;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
