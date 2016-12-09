#if !NETCORE
using Pingpp.Models;
using ZKWeb.Plugins.Finance.Payment.Pingpp.src.Domain.Services;
#endif
using System;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.Forms {
	/// <summary>
	/// Ping++支付的表单
	/// </summary>
	[Form("PingppPayForm", "/payment/pingpp/pay",
		SubmitButtonText = "PayNow", SubmitButtonCssClass = "btn btn-themed")]
	public class PingppPayForm : ModelFormBuilder {
		/// <summary>
		/// 交易Id
		/// </summary>
		[Required]
		[HiddenField("TransactionId")]
		public Guid TransactionId { get; set; }
		/// <summary>
		/// 支付途径
		/// </summary>
		[Required]
		[HiddenField("PaymentChannel")]
		public string PaymentChannel { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() {
			TransactionId = Request.Get<Guid>("id");
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
#if NETCORE
			throw new BadRequestException("Ping++ on .Net Core is unsupported yet");
#else
			var pingppManager = Application.Ioc.Resolve<PingppManager>();
			Charge charge;
			string realResultUrl;
			string waitResultUrl;
			pingppManager.GetCharge(
				TransactionId, PaymentChannel,
				Request.GetUserAgent(),
				Request.RemoteIpAddress.MapToIPv4().ToString(),
				out charge,
				out realResultUrl,
				out waitResultUrl);
			return new { charge, realResultUrl, waitResultUrl };
#endif
		}
	}
}
