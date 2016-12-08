using System;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.Pingpp.src.UIComponents.Forms {
	using Common.Base.src.Components.GenericConfigs;
	using Common.Base.src.Domain.Services;
	using global::Pingpp.Models;
	using Payment.src.Domain.Entities;
	using Payment.src.Domain.Structs;
	using System.Collections.Generic;
	using static Components.PaymentApiHandlers.PingppApiHandler;
	using Pingpp = global::Pingpp.Pingpp;

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
			var unitOfWork = Application.Ioc.Resolve<IUnitOfWork>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			PaymentTransaction transaction;
			PaymentApi api;
			ApiData apiData;
			using (unitOfWork.Scope()) {
				// 获取交易和支付接口
				transaction = transactionManager.Get(TransactionId);
				api = transaction == null ? null : transaction.Api;
				// 检查交易和接口是否存在
				if (transaction == null) {
					throw new BadRequestException(new T("Payment transaction not found"));
				} else if (api == null) {
					throw new BadRequestException(new T("Payment api not exist"));
				}
				// 检查当前登录用户是否可以支付
				var result = transaction.Check(c => c.IsPayerLoggedIn);
				if (!result.First) {
					throw new BadRequestException(result.Second);
				}
				result = transaction.Check(c => c.IsPayable);
				if (!result.First) {
					throw new BadRequestException(result.Second);
				}
				apiData = transaction.Api.ExtraData.GetOrDefault<ApiData>("ApiData") ?? new ApiData();
			}
			// 检查货币，虽然Ping++有提供货币参数但是支付渠道里面都只能支付人民币
			if (transaction.CurrencyType != "CNY") {
				throw new BadRequestException(new T("Ping++ only support cny payment"));
			}
			// 获取Ping++的支付凭据
			// 注意这里非线程安全，如果添加多个Ping++接口可能会出问题
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var websiteSettings = configManager.GetData<WebsiteSettings>();
			Pingpp.SetApiKey(apiData.TradeSecretKey);
			Pingpp.SetPrivateKey(apiData.PartnerRsaPrivateKey);
			var extra = new Dictionary<string, object>();
			var chargeParams = new Dictionary<string, object>
			{
				{ "order_no", transaction.Serial },
				{ "amount", checked((int)(transaction.Amount * 100)) },
				{ "channel", PaymentChannel },
				{ "currency", "cny" },
				{ "subject", $"{new T(websiteSettings.WebsiteName)} {transaction.Serial}" },
				{ "body", transaction.Description },
				{ "client_ip", Request.RemoteIpAddress.MapToIPv4().ToString() },
				{ "app", new Dictionary<string, string> {{ "id", apiData.PingppAppId }}},
				{ "extra", extra }
			};
			var charge = Charge.Create(chargeParams);
			return charge;
		}
	}
}
