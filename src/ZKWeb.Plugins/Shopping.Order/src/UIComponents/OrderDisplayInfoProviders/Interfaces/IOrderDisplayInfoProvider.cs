using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.OrderDisplayInfoProviders.Interfaces {
	/// <summary>
	/// 订单显示信息提供器
	/// </summary>
	public interface IOrderDisplayInfoProvider {
		/// <summary>
		/// 添加订单操作的Html
		/// </summary>
		/// <param name="order"></param>
		/// <param name="action"></param>
		/// <param name="operatorType"></param>
		void AddActions(SellerOrder order, IList<HtmlString> action, OrderOperatorType operatorType);

		/// <summary>
		/// 添加工具按钮
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <param name="toolButtons">警告信息列表</param>
		/// <param name="operatorType">操作人类型</param>
		void AddToolButtons(SellerOrder order, IList<HtmlString> toolButtons, OrderOperatorType operatorType);

		/// <summary>
		/// 添加详细信息
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <param name="subjects">信息列表</param>
		/// <param name="operatorType">操作人类型</param>
		void AddSubjects(SellerOrder order, IList<HtmlString> subjects, OrderOperatorType operatorType);

		/// <summary>
		/// 添加警告信息
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <param name="warnings">警告信息列表</param>
		/// <param name="operatorType">操作人类型</param>
		void AddWarnings(SellerOrder order, IList<HtmlString> warnings, OrderOperatorType operatorType);
	}
}
