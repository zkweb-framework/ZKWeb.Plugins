using System;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentApiHandlers.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.UIComponents.Forms;
using ZKWeb.Plugins.Finance.Payment.WeChat.src.Domain.Services;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.Wechat.src.Components.PaymentApiHandlers {
	/// <summary>
	/// 微信接口的处理器
	/// </summary>
	[ExportMany]
	public class WechatApiHandler : IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		public string Type { get { return "WechatPay"; } }
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
			ApiDataEditing.AppId = apiData.AppId;
			ApiDataEditing.PartnerId = apiData.PartnerId;
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
			var wxPayManager = Application.Ioc.Resolve<WxPayManager>();
			html = wxPayManager.GetQRCodePaymentHtml(transaction);
		}

		/// <summary>
		/// 调用发货接口
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
			/// 绑定支付的APPID
			/// </summary>
			[Required]
			[TextBoxField("AppId", "AppId")]
			public string AppId { get; set; }
			/// <summary>
			/// 商户号
			/// </summary>
			[Required]
			[TextBoxField("PartnerId", "PartnerId")]
			public string PartnerId { get; set; }
			/// <summary>
			/// 商户支付密钥
			/// </summary>
			[Required]
			[TextBoxField("PartnerKey", "PartnerKey")]
			public string PartnerKey { get; set; }
			/// <summary>
			/// 返回域名
			/// </summary>
			[TextBoxField("ReturnDomain", "keep empty will use the default domain")]
			public string ReturnDomain { get; set; }
		}
	}
}
