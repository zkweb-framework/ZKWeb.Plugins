using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 取消订单的表单
	/// </summary>
	public class OrderCancelForm : ModelFormBuilder {
		/// <summary>
		/// 取消原因
		/// </summary>
		[TextAreaField("CancelReason", 5, "The reason of cancel order, must provide")]
		public string CancelReason { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var serial = Request.Get<string>("serial");
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var orderManager = Application.Ioc.Resolve<BuyerOrderManager>();
			var orderId = orderManager.GetBuyerOrderIdFromSerial(serial) ?? Guid.Empty;
			if (!orderManager.CancelOrder(orderId, user?.Id, CancelReason)) {
				throw new BadRequestException(new T("Cancel order failed, please check order records"));
			}
			return new {
				message = new T("Cancel order success"),
				script = BaseScriptStrings.AjaxtableUpdatedAndCloseModal
			};
		}
	}
}
