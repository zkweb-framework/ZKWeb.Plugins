using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderActionsProviders.Interfaces {
	/// <summary>
	/// 订单操作提供器
	/// </summary>
	public interface IOrderActionProvider {
		/// <summary>
		/// 添加订单操作的Html
		/// </summary>
		/// <param name="order"></param>
		/// <param name="action"></param>
		/// <param name="operatorType"></param>
		void AddActions(SellerOrder order, IList<HtmlString> action, OrderOperatorType operatorType);
	}
}
