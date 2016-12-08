using System;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.Forms {
	/// <summary>
	/// Ping++支付的表单
	/// </summary>
	[Form("PingppPayForm", "/payment/pingpp/pay",
		SubmitButtonText = "PayNow", SubmitButtonCssClass = "btn btn-themed")]
	public class PingppPayForm : ModelFormBuilder {
		/// <summary>
		/// 支付途径
		/// </summary>
		[Required]
		[HiddenField("PaymentChannel")]
		public string PaymentChannel { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			throw new NotImplementedException();
		}
	}
}
