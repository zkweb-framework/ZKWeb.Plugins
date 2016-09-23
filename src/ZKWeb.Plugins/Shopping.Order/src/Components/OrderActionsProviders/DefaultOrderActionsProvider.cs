using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderActionsProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderActionsProviders {
	/// <summary>
	/// 默认的订单操作提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderActionsProvider : IOrderActionProvider {
		/// <summary>
		/// 添加订单操作
		/// </summary>
		public void AddActions(
			SellerOrder order, IList<HtmlString> actions, OrderOperatorType operatorType) {
			actions.Add(new HtmlString("<a>xxx</a>"));
			if (operatorType == OrderOperatorType.Buyer) {
				// TODO:
				// 立刻付款

				// 取消订单

				// 确认收货

			} else {
				// 修改价格

				// 修改地址

				// 发货

				// 代确认收货

				// 作废

				actions.Add(new HtmlString("<a>asdasdas</a>"));
			}
		}
	}
}
