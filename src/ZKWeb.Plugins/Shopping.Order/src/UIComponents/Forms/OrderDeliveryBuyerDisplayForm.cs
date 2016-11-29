using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 买家查看的发货单详情页
	/// </summary>
	[Form("OrderDeliveryBuyerDisplayForm", SubmitButtonCssClass = "hide")]
	public class OrderDeliveryBuyerDisplayForm :
		OrderDeliveryBaseDisplayForm<OrderDeliverySellerDisplayForm> {
		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit(OrderDelivery saveTo) {
			throw new NotSupportedException();
		}
	}
}
