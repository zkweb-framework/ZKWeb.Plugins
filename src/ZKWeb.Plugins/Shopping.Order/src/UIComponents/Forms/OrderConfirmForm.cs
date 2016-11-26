using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 确认收货的表单
	/// </summary>
	[Form("OrderConfirmForm", SubmitButtonText = "ConfirmOrder")]
	public class OrderConfirmForm : ModelFormBuilder {
		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit() {
			var serial = Request.Get<string>("serial");
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var orderManager = Application.Ioc.Resolve<BuyerOrderManager>();
			var orderId = orderManager.GetBuyerOrderIdFromSerial(serial) ?? Guid.Empty;
			var message = orderManager.ConfirmOrder(orderId, user?.Id) ?
				new T("Confirm order success") :
				new T("Confirm order failed, please check order records");
			return new { message = message, script = BaseScriptStrings.AjaxtableUpdatedAndCloseModal };
		}
	}
}
