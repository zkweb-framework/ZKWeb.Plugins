using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderSubjectProviders.Interfaces {
	/// <summary>
	/// 订单详情内容的提供器
	/// </summary>
	public interface IOrderSubjectProvider {
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
	}
}
