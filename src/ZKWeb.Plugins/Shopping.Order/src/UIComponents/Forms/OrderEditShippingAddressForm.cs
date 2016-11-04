using System;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Admin.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Region.src.Components.Regions;
using ZKWeb.Plugins.Common.Region.src.UIComponents.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 编辑订单收货地址使用的表单
	/// </summary>
	public class OrderEditShippingAddressForm :
		TabEntityFormBuilder<SellerOrder, Guid, OrderEditShippingAddressForm> {
		/// <summary>
		/// 地区
		/// </summary>
		[RegionEditor("Region")]
		public CountryAndRegion Region { get; set; }
		/// <summary>
		/// 邮政编码
		/// </summary>
		[TextBoxField("ZipCode")]
		public string ZipCode { get; set; }
		/// <summary>
		/// 详细地址
		/// </summary>
		[Required]
		[TextBoxField("DetailedAddress", "DetailedAddress")]
		[StringLength(1000, MinimumLength = 1)]
		public string DetailedAddress { get; set; }
		/// <summary>
		/// 收货人姓名
		/// </summary>
		[Required]
		[TextBoxField("Fullname", "Fullname")]
		[StringLength(100, MinimumLength = 1)]
		public string ReceiverName { get; set; }
		/// <summary>
		/// 收货人电话/手机
		/// </summary>
		[Required]
		[TextBoxField("TelOrMobile", "TelOrMobile")]
		[StringLength(100, MinimumLength = 1)]
		public string ReceiverTel { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind(SellerOrder bindFrom) {
			var address = bindFrom.OrderParameters.GetShippingAddress();
			Region = new CountryAndRegion(address.Country, address.RegionId);
			ZipCode = address.ZipCode;
			DetailedAddress = address.DetailedAddress;
			ReceiverName = address.ReceiverName;
			ReceiverTel = address.ReceiverTel;
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit(SellerOrder saveTo) {
			var address = saveTo.OrderParameters.GetShippingAddress();
			address.Country = Region.Country;
			address.RegionId = Region.RegionId;
			address.ZipCode = ZipCode;
			address.DetailedAddress = DetailedAddress;
			address.ReceiverName = ReceiverName;
			address.ReceiverTel = ReceiverTel;
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var operatorId = sessionManager.GetSession().GetUser().Id;
			orderManager.EditShippingAddress(saveTo, operatorId, address);
			return this.SaveSuccessAndCloseModal();
		}
	}
}
