using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.CMS.Article.src.Config;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.AdminSettingsPages {
	/// <summary>
	/// 商品列表设置
	/// </summary>
	[ExportMany]
	public class ArticleListSettingsForm : AdminSettingsFormPageBuilder {
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
