using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderWarningProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderWarningProviders {
	/// <summary>
	/// 默认的订单警告提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderWarningProvider : IOrderWarningProvider {
		/// <summary>
		/// 添加警告信息
		/// </summary>
		public void AddWarnings(
			SellerOrder order, IList<HtmlString> warnings, OrderOperatorType operatorType) {
			// 警告担保交易未确认收款
			// TODO: 换成HtmlString.Encode
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var transactions = orderManager.GetReleatedTransactions(order.Id);
			if (transactions.Any(t => t.State == PaymentTransactionState.SecuredPaid)) {
				if (operatorType == OrderOperatorType.Buyer) {
					warnings.Add(new HtmlString(HttpUtils.HtmlEncode(
						new T("Buyer is using secured paid, please tell the buyer confirm transaction on payment platform after received goods"))));
				} else {
					warnings.Add(new HtmlString(HttpUtils.HtmlEncode(
						new T("You're using secured paid, please confirm transaction on payment platform after received goods"))));
				}
			}
			// 警告关联交易的最后发生错误
			// TODO: 换成HtmlString.Encode
			var lastErrors = transactions.Select(t => t.LastError).Where(e => !string.IsNullOrEmpty(e));
			foreach (var lastError in lastErrors) {
				warnings.Add(new HtmlString(HttpUtils.HtmlEncode(
					string.Format(new T("Releated transaction contains error: {0}"), new T(lastError)))));
			}
		}
	}
}
