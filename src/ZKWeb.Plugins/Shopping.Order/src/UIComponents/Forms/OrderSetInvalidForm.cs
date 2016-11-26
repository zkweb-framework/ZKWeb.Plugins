using System;
using System.ComponentModel.DataAnnotations;
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
	/// 作废订单的表单
	/// </summary>
	public class OrderSetInvalidForm : ModelFormBuilder {
		/// <summary>
		/// 作废原因
		/// </summary>
		[Required]
		[TextAreaField("SetInvalidReason", 5, "The reason of set order invalid, must provide")]
		public string SetInvalidReason { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var orderId = Request.Get<Guid>("id");
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var message = orderManager.SetOrderInvalid(orderId, user?.Id, SetInvalidReason) ?
				new T("Set order invalid success") :
				new T("Set order invalid failed, please check order records");
			return new { message = message, script = BaseScriptStrings.AjaxtableUpdatedAndCloseModal };
		}
	}
}
