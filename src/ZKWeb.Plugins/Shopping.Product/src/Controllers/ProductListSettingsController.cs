using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Components.GenericConfigs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// 商品列表设置
	/// </summary>
	[ExportMany]
	public class ProductListSettingsController : FormAdminSettingsControllerBase {
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
