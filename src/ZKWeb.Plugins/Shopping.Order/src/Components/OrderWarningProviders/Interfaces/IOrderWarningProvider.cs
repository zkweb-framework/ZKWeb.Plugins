using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderWarningProviders.Interfaces {
	/// <summary>
	/// 订单警告信息的提供器接口
	/// </summary>
	public interface IOrderWarningProvider {
		/// <summary>
		/// 添加警告信息
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <param name="warnings">警告信息列表</param>
		/// <param name="operatorType">操作人类型</param>
		void AddWarnings(SellerOrder order, IList<HtmlString> warnings, OrderOperatorType operatorType);
	}
}
