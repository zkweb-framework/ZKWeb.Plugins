using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Region.src.FormFieldAttributes;
using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.ListItemProviders;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.Forms {
	/// <summary>
	/// 用户收货地址表单
	/// </summary>
	public class UserShippingAddressForm : FieldsOnlyModelFormBuilder {
		/// <summary>
		/// 收货地址
		/// </summary>
		[SearchableDropdownListField("ShippingAddress", typeof(ShippingAddressListItemProvider))]
		public string ShippingAddress { get; set; }
		/// <summary>
		/// 管理收货地址, 保存收货地址
		/// </summary>
		[HtmlField("ShippingAddressAddin")]
		public HtmlString ShippingAddressAddin { get; set; }
		/// <summary>
		/// 最终填写的收货地址的Json
		/// </summary>
		[Required]
		[HiddenField("ShippingAddressJson")]
		public string ShippingAddressJson { get; set; }
		/// <summary>
		/// 收货人姓名
		/// </summary>
		[Required]
		[TextBoxField("Fullname", "Fullname")]
		public string ReceiverName { get; set; }
		/// <summary>
		/// 收货人电话
		/// </summary>
		[Required]
		[TextBoxField("TelOrMobile", "TelOrMobile")]
		public string ReceiverTel { get; set; }
		/// <summary>
		/// 地区
		/// </summary>
		[Required]
		[RegionEditor("Region")]
		public CountryAndRegion Region { get; set; }
		/// <summary>
		/// 邮政编码
		/// </summary>
		[TextBoxField("ZipCode", "ZipCode")]
		public string ZipCode { get; set; }
		/// <summary>
		/// 详细地址
		/// </summary>
		[Required]
		[TextBoxField("DetailedAddress", "DetailedAddress")]
		public string DetailedAddress { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			ShippingAddressAddin = new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.user_shipping_address_addin.html", null));
		}
	}
}
