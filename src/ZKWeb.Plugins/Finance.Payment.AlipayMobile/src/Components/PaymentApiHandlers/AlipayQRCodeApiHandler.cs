using System.ComponentModel.DataAnnotations;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Finance.Payment.AlipayMobile.src.Domain.Services;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentApiHandlers.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.UIComponents.Forms;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.AlipayMobile.src.Components.PaymentApiHandlers {
	/// <summary>
	/// 支付宝扫码支付的处理器
	/// </summary>
	[ExportMany]
	public class AlipayQRCodeApiHandler : IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		public string Type { get { return "AlipayQRCodePay"; } }
		/// <summary>
		/// 后台编辑表单使用的接口数据
		/// </summary>
		protected ApiData ApiDataEditing = new ApiData();

		/// <summary>
		/// 后台编辑表单创建后的处理
		/// </summary>
		public void OnFormCreated(PaymentApiEditForm form) {
			form.AddFieldsFrom(ApiDataEditing);
		}

		/// <summary>
		/// 后台编辑表单绑定时的处理
		/// </summary>
		public void OnFormBind(PaymentApiEditForm form, PaymentApi bindFrom) {
			var apiData = bindFrom.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			ApiDataEditing.PartnerId = apiData.PartnerId;
			ApiDataEditing.PayeePartnerId = apiData.PayeePartnerId;
			ApiDataEditing.AppId = apiData.AppId;
			ApiDataEditing.PartnerKey = apiData.PartnerKey;
			ApiDataEditing.ReturnDomain = apiData.ReturnDomain;
		}

		/// <summary>
		/// 后台编辑表单保存时的处理
		/// </summary>
		public void OnFormSubmit(PaymentApiEditForm form, PaymentApi saveTo) {
			saveTo.ExtraData["ApiData"] = ApiDataEditing;
		}

		/// <summary>
		/// 计算支付手续费
		/// </summary>
		public void CalculatePaymentFee(PaymentApi api, decimal amount, ref decimal paymentFee) {
			paymentFee = 0;
		}

		/// <summary>
		/// 获取支付Html
		/// </summary>
		public void GetPaymentHtml(PaymentTransaction transaction, ref HtmlString html) {
			var alipayMobileManager = Application.Ioc.Resolve<AlipayMobileManager>();
			html = alipayMobileManager.GetQRCodePaymentHtml(transaction);
		}

		/// <summary>
		/// 调用发货接口
		/// 支付宝扫码支付不需要调发货接口
		/// </summary>
		public void DeliveryGoods(
			PaymentTransaction transaction, string logisticsName, string invoiceNo) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format(
				"PaymentApi send goods: transaction {0} logisticsName {1} invoiceNo {2}",
				transaction.Serial, logisticsName, invoiceNo));
		}

		/// <summary>
		/// 接口数据
		/// </summary>
		public class ApiData {
			/// <summary>
			/// 商户Id
			/// </summary>
			[Required]
			[TextBoxField("PartnerId", "PartnerId, usually starts with 2088")]
			public string PartnerId { get; set; }
			/// <summary>
			/// 收款商户Id
			/// </summary>
			[Required]
			[TextBoxField("PayeePartnerId", "PayeePartnerId, usually same with PartnerId")]
			public string PayeePartnerId { get; set; }
			/// <summary>
			/// AppId
			/// </summary>
			[Required]
			[TextBoxField("AppId", "AppId, usually starts with datetime such as 20170306")]
			public string AppId { get; set; }
			/// <summary>
			/// 商户密钥
			/// </summary>
			[Required]
			[TextAreaField("PartnerKey", 5, "PartnerKey (RSA with SHA1), usually starts with -----BEGIN RSA PRIVATE KEY-----")]
			public string PartnerKey { get; set; }
			/// <summary>
			/// 返回域名
			/// </summary>
			[TextBoxField("ReturnDomain", "keep empty will use the default domain")]
			public string ReturnDomain { get; set; }

			/// <summary>
			/// 获取商户密钥，除去头部和尾部
			/// </summary>
			/// <returns></returns>
			public string GetPartnerKeyBody() {
				return PartnerKey
					.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
					.Replace("-----END RSA PRIVATE KEY-----", "")
					.Trim();
			}
		}
	}
}
