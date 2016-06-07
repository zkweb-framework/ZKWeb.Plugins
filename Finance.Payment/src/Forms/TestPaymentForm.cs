using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Currency.src.Config;
using ZKWeb.Plugins.Common.Currency.src.ListItemProviders;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Managers;
using ZKWeb.Plugins.Finance.Payment.src.Repositories;
using ZKWeb.Server;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Finance.Payment.src.Forms {
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
			var id = HttpContextUtils.CurrentContext.Request.Get<long>("id");
			var api = UnitOfWork.ReadData<PaymentApi, PaymentApi>(r => r.GetById(id));
			if (api == null) {
				throw new HttpException(404, new T("Payment api not exist"));
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
			var payeeId = api.Owner == null ? null : (long?)api.Owner.Id;
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var transaction = transactionManager.CreateTestTransaction(api.Id, Amount, Currency, payerId, payeeId, Description);
			// 跳转到支付url地址
			var url = transactionManager.GetPaymentUrl(transaction.Id);
			return new {
				message = new T("Create test transaction success, redirecting to payment page..."),
				script = ScriptStrings.Redirect(url, 3000)
			};
		}
	}
}
