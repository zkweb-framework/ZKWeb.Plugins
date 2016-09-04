using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Currency.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Currency.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ScriptStrings;

namespace ZKWeb.Plugins.Finance.Payment.src.UIComponents.Forms {
	/// <summary>
	/// 用于测试支付接口是否可以正常使用的表单
	/// </summary>
	public class TestPaymentForm : ModelFormBuilder {
		/// <summary>
		/// 接口名称
		/// </summary>
		[LabelField("ApiName")]
		public string ApiName { get; set; }
		/// <summary>
		/// 金额
		/// </summary>
		[Required]
		[RegularExpression(RegexUtils.Expressions.Decimal)]
		[TextBoxField("Amount")]
		public decimal Amount { get; set; }
		/// <summary>
		/// 货币
		/// </summary>
		[Required]
		[DropdownListField("Currency", typeof(CurrencyListItemProvider))]
		public string Currency { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		[Required]
		[TextAreaField("Description", 5)]
		public string Description { get; set; }

		/// <summary>
		/// 从请求中获取支付接口，不存在时抛出错误
		/// </summary>
		/// <returns></returns>
		protected PaymentApi GetApiFromRequest() {
			var id = Request.Get<Guid>("id");
			var apiManager = Application.Ioc.Resolve<PaymentApiManager>();
			var api = apiManager.Get(id);
			if (api == null) {
				throw new NotFoundException(new T("Payment api not exist"));
			}
			return api;
		}

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() {
			var api = GetApiFromRequest();
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var currencySettings = configManager.GetData<CurrencySettings>();
			ApiName = api.Name;
			Amount = 0.1M;
			Currency = currencySettings.DefaultCurrency;
			Description = new T("Test Payment Api");
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			// 创建交易
			var api = GetApiFromRequest();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var payerId = sessionManager.GetSession().GetUser().Id;
			var payeeId = api.Owner?.Id;
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var apiManager = Application.Ioc.Resolve<PaymentApiManager>();
			var paymentFee = apiManager.CalculatePaymentFee(api.Id, Amount);
			var transaction = transactionManager.CreateTransaction(
				"TestTransaction", api.Id, Amount, paymentFee,
				Currency, payerId, payeeId, payerId, Description);
			// 跳转到支付url地址
			var url = transactionManager.GetPaymentUrl(transaction.Id);
			return new {
				message = new T("Create test transaction success, redirecting to payment page..."),
				script = BaseScriptStrings.Redirect(url, 3000)
			};
		}
	}
}
