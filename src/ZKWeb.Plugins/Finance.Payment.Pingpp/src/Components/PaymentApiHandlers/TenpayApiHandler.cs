using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentApiHandlers.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.UIComponents.Forms;
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
			ApiDataEditing.PingppRsaPublicKey = apiData.PingppRsaPublicKey;
			ApiDataEditing.PartnerRsaPrivateKey = apiData.PartnerRsaPrivateKey;
			ApiDataEditing.PaymentChannels = apiData.PaymentChannels;
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// 调用发货接口
		/// </summary>
		public void DeliveryGoods(
			PaymentTransaction transaction, string logisticsName, string invoiceNo) {
			throw new NotImplementedException();
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
			/// 支付渠道
			/// </summary>
			[CheckBoxGroupField("PaymentChannels", typeof(PaymentChannelListItemProvider))]
			public IList<string> PaymentChannels { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			public ApiData() {
				PaymentChannels = new List<string>();
			}
		}
	}
}
