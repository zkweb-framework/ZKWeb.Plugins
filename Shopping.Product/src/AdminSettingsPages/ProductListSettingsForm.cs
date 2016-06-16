using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.AdminSettings.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Shopping.Product.src.Config;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.AdminSettingsPages {
	/// <summary>
	/// 商品列表设置
	/// </summary>
	[ExportMany]
	public class ProductListSettingsForm : AdminSettingsFormPageBuilder {
		public override string Group { get { return "ProductSettings"; } }
		public override string GroupIconClass { get { return "fa fa-diamond"; } }
		public override string Name { get { return "ProductListSettings"; } }
		public override string IconClass { get { return "fa fa-list"; } }
		public override string Url { get { return "/admin/settings/product_list_settings"; } }
		public override string Privilege { get { return "AdminSettings:ProductListSettings"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 每页显示的商品数量
			/// </summary>
			[Required]
			[TextBoxField("ProductsPerPage", "ProductsPerPage")]
			public int ProductsPerPage { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<ProductListSettings>();
				ProductsPerPage = settings.ProductsPerPage;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<ProductListSettings>();
				settings.ProductsPerPage = ProductsPerPage;
				configManager.PutData(settings);
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
