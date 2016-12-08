using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.Forms;
using ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentApiHandlers.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.UIComponents.Forms;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.Pingpp.src.Components.PaymentApiHandlers {
	/// <summary>
	/// Ping++接口的处理器
	/// </summary>
	[ExportMany]
	public class PingppApiHandler : IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		public string Type { get { return "Pingpp"; } }
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
			ApiDataEditing.TradeSecretKey = apiData.TradeSecretKey;
			ApiDataEditing.PingppAppId = apiData.PingppAppId;
			ApiDataEditing.PingppRsaPublicKey = apiData.PingppRsaPublicKey;
			ApiDataEditing.PartnerRsaPrivateKey = apiData.PartnerRsaPrivateKey;
			ApiDataEditing.ReturnDomain = apiData.ReturnDomain;
			ApiDataEditing.PaymentChannels = apiData.PaymentChannels.ToList();
			ApiDataEditing.WeChatOpenId = apiData.WeChatOpenId;
			ApiDataEditing.WeChatNoCredit = apiData.WeChatNoCredit;
			ApiDataEditing.FqlChildMerchantId = apiData.FqlChildMerchantId;
			ApiDataEditing.BfbRequireLogin = apiData.BfbRequireLogin;
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
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var apiData = transaction.Api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			var paymentChannels = new PaymentChannelListItemProvider().GetItems()
				.Where(c => apiData.PaymentChannels.Contains(c.Value))
				.ToList();
			var form = new PingppPayForm();
			form.Bind();
			html = new HtmlString(templateManager.RenderTemplate(
				"finance.payment.pingpp/pingpp_pay.html", new {
					transactionId = transaction.Id,
					paymentChannels,
					form
				}));
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
			/// 交易密钥
			/// </summary>
			[Required]
			[TextBoxField("TradeSecretKey", "You can provide test key or live key")]
			public string TradeSecretKey { get; set; }
			/// <summary>
			/// App Id
			/// </summary>
			[Required]
			[TextBoxField("PingppAppId", "Starts with app_")]
			public string PingppAppId { get; set; }
			/// <summary>
			/// Ping++RSA公钥
			/// </summary>
			[Required]
			[TextAreaField("PingppRsaPublicKey", 5, "Starts with -----BEGIN PUBLIC KEY-----")]
			public string PingppRsaPublicKey { get; set; }
			/// <summary>
			/// 商户RSA私钥
			/// </summary>
			[Required]
			[TextAreaField("PartnerRsaPrivateKey", 5, "Starts with -----BEGIN RSA PRIVATE KEY-----")]
			public string PartnerRsaPrivateKey { get; set; }
			/// <summary>
			/// 返回域名
			/// </summary>
			[TextBoxField("ReturnDomain", "keep empty will use the default domain")]
			public string ReturnDomain { get; set; }
			/// <summary>
			/// 支付渠道
			/// </summary>
			[CheckBoxGroupField("PaymentChannels", typeof(PaymentChannelListItemProvider))]
			public IList<string> PaymentChannels { get; set; }
			/// <summary>
			/// 微信Open Id
			/// </summary>
			[TextBoxField("WeChatOpenId", "WeChatOpenId", Group = "ExtraPaymentArguments")]
			public string WeChatOpenId { get; set; }
			/// <summary>
			/// 微信限制使用信用卡
			/// </summary>
			[CheckBoxField("WeChatNoCredit", Group = "ExtraPaymentArguments")]
			public bool WeChatNoCredit { get; set; }
			/// <summary>
			/// 分期乐子商户编号
			/// </summary>
			[TextBoxField("FqlChildMerchantId", "FqlChildMerchantId", Group = "ExtraPaymentArguments")]
			public string FqlChildMerchantId { get; set; }
			/// <summary>
			/// 百度钱包需要登陆
			/// </summary>
			[CheckBoxField("BfbRequireLogin", Group = "ExtraPaymentArguments")]
			public bool BfbRequireLogin { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public ApiData() {
				PaymentChannels = new List<string>();
			}
		}
	}
}
