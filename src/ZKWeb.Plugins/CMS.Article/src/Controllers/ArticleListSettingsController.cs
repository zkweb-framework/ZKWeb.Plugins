using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.CMS.Article.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	/// <summary>
	/// 商品列表设置
	/// </summary>
	[ExportMany]
	public class ArticleListSettingsForm : FormAdminSettingsControllerBase {
		public override string Group { get { return "ArticleSettings"; } }
		public override string GroupIconClass { get { return "fa fa-pencil"; } }
		public override string Name { get { return "ArticleListSettings"; } }
		public override string IconClass { get { return "fa fa-list"; } }
		public override string Url { get { return "/admin/settings/article_list_settings"; } }
		public override string Privilege { get { return "AdminSettings:ArticleListSettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 每页显示的商品数量
			/// </summary>
			[Required]
			[TextBoxField("ArticlesPerPage", "ArticlesPerPage")]
			public int ArticlesPerPage { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<ArticleListSettings>();
				ArticlesPerPage = settings.ArticlesPerPage;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<ArticleListSettings>();
				settings.ArticlesPerPage = ArticlesPerPage;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
