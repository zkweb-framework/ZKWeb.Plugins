using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.src.Components.PaymentTransactionHandlers.Bases;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.PaymentTransactionHandlers {
	/// <summary>
	/// 订单合并交易处理器
	/// 
	/// 重复支付 情况a
	/// - 创建订单A和订单B
	/// - 打开合并交易支付页面
	/// - 打开订单A交易支付页面
	/// - 支付合并交易 (处理成功)
	/// - 支付订单A交易 (重复处理，这时外部序列号会改变但交易状态不会被改变，可以人工确认)
	///
	/// 重复支付 情况b
	/// - 创建订单A和订单B
	/// - 打开合并交易支付页面
	/// - 打开订单A交易支付页面
	/// - 支付订单A交易 (处理成功, 合并交易被终止)
	/// - 支付合并交易 (处理支付失败，可以看到记录，可以人工确认)
	/// 
	/// 对于重复支付合并交易和普通交易的处理
	/// - 修改价格时自动终止合并交易, 使用EnsureParentTransactionAborted
	/// - 普通交易支付后终止合并交易，使用EnsureParentTransactionAbortedIfProcessNotFromParent
	/// 
	/// 对于非同时支付的冲突，不需要人工处理可以事先防止，但同时支付的冲突仍需要人工确认
	/// </summary>
	[ExportMany]
	public class MergedOrderTransactionHandler : MergedTransactionHandlerBase {
		/// <summary>
		/// 交易类型
		/// </summary>
		public override string Type { get { return ConstType; } }
		public const string ConstType = "MergedOrderTransaction";

		/// <summary>
		/// 获取显示交易结果的Html
		/// </summary>
		public override void GetResultHtml(PaymentTransaction transaction, IList<HtmlString> html) {
			// 显示内容
			// 如果交易本身不可支付
			// - 如果交易已支付，延迟跳转到订单列表
			// - 否则显示交易状态信息
			// 如果交易本身可支付
			// - 如果所有订单可支付，则显示支付按钮
			// - 否则显示交易状态信息
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var orderIds = transaction.ReleatedTransactions.Select(t => t.ReleatedId);
			var orders = orderManager
				.GetMany(o => orderIds.Contains(o.Id))
				.Select(o => new {
					id = o.Id,
					serial = o.Serial,
					state = o.State.ToString(),
					amount = currencyManager.GetCurrency(o.Currency).Format(o.TotalCost),
					payable = o.Check(c => c.CanPay).First
				})
				.ToList();
			html.Add(new HtmlString(templateManager.RenderTemplate(
				"shopping.order/order_checkout_merged.html", new {
					transactionSerial = transaction.Serial,
					transactionState = transaction.State.ToString(),
					transactionAmount = currencyManager
						.GetCurrency(transaction.CurrencyType).Format(transaction.Amount),
					orders = orders,
					isPayable = transaction.Check(x => x.IsPayable).First,
					isAllOrderPayable = orders.Any() && orders.All(o => o.payable),
					apiName = new T(transaction.Api.Name),
					paymentUrl = transactionManager.GetPaymentUrl(transaction.Id)
				})));
		}
	}
}
