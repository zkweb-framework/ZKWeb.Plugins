using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 编辑订单收货地址使用的表单
	/// </summary>
	public class OrderEditShippingAddressForm :
		EntityFormBuilder<SellerOrder, Guid, OrderEditShippingAddressForm> {
		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind(SellerOrder bindFrom) {
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit(SellerOrder saveTo) {
			throw new NotImplementedException();
		}
	}
}
