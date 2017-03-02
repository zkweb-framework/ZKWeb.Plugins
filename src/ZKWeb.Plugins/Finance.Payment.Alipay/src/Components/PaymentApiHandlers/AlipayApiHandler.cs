using System.ComponentModel.DataAnnotations;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Finance.Payment.Alipay.src.Domain.Services;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentApiHandlers.Interfaces;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.UIComponents.Forms;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.Alipay.src.Components.PaymentApiHandlers {
	/// <summary>
	/// 支付宝接口的处理器
	/// </summary>
	[ExportMany]
	public class AlipayApiHandler : IPaymentApiHandler {
		/// <summary>
		/// 接口类型
		/// </summary>
		public string Type { get { return "Alipay"; } }
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
			ApiDataEditing.PartnerKey = apiData.PartnerKey;
			ApiDataEditing.ServiceType = apiData.ServiceType;
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
			var alipayManager = Application.Ioc.Resolve<AlipayManager>();
			html = alipayManager.GetPaymentHtml(transaction);
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
			var alipayManager = Application.Ioc.Resolve<AlipayManager>();
			alipayManager.DeliveryGoods(transaction, logisticsName, invoiceNo);
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
			/// 商户密钥
			/// </summary>
			[Required]
			[TextAreaField("PartnerKey", 5, "PartnerKey, usually starts with -----BEGIN RSA PRIVATE KEY-----")]
			public string PartnerKey { get; set; }
			/// <summary>
			/// 服务类型
			/// </summary>
			[Required]
			[RadioButtonsField("ServiceType", typeof(ListItemFromEnum<AlipayServiceTypes>))]
			public int ServiceType { get; set; }
			/// <summary>
			/// 返回域名
			/// </summary>
			[TextBoxField("ReturnDomain", "keep empty will use the default domain")]
			public string ReturnDomain { get; set; }
		}
	}
}
