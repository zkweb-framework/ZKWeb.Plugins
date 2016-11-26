using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 后台代替买家确认收货的表单
	/// </summary>
	public class OrderConfirmInsteadOfBuyerForm : ModelFormBuilder {
		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit() {
			throw new NotImplementedException();
		}
	}
}
