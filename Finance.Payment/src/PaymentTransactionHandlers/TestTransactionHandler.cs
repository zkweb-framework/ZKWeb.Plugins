using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Logging;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.PaymentTransactionHandlers {
	/// <summary>
	/// 测试交易的处理器
	/// </summary>
	[ExportMany]
	public class TestTransactionHandler : IPaymentTransactionHandler {
		/// <summary>
		/// 交易类型
		/// </summary>
		public string Type { get { return "TestTransaction"; } }

		/// <summary>
		/// 交易创建后
		/// </summary>
		public void OnCreated(
			IDatabaseContext context, PaymentTransaction transaction) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format("TestTransaction Created: {0}", transaction.Serial));
		}

		/// <summary>
		/// 等待付款时
		/// </summary>
		public void OnWaitingPaying(
			IDatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format("TestTransaction Waiting Paying: {0}", transaction.Serial));
		}

		/// <summary>
		/// 担保交易付款后
		/// 付款后自动发货
		/// </summary>
		public void OnSecuredPaid(
			IDatabaseContext context, PaymentTransaction transaction,
			PaymentTransactionState previousState, IList<AutoSendGoodsParameters> parameters) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format("TestTransaction Secured Paid: {0}", transaction.Serial));
			parameters.Add(new AutoSendGoodsParameters() { LogisticsName = "TestLogistics", InvoiceNo = "00000000" });
		}

		/// <summary>
		/// 交易成功时
		/// </summary>
		public void OnSuccess(
			IDatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format("TestTransaction Success: {0}", transaction.Serial));
		}

		/// <summary>
		/// 交易终止时
		/// </summary>
		public void OnAbort(
			IDatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogTransaction(string.Format("TestTransaction Aborted: {0}", transaction.Serial));
		}

		/// <summary>
		/// 获取显示交易结果的Html
		/// </summary>
		public void GetResultHtml(PaymentTransaction transaction, IList<HtmlString> html) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var args = new { serial = transaction.Serial, state = transaction.State.GetDescription() };
			html.Add(new HtmlString(
				templateManager.RenderTemplate("finance.payment/test_transaction_result.html", args)));
		}
	}
}
