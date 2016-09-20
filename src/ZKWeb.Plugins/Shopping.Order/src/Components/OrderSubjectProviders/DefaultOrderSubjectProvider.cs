using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderSubjectProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderSubjectProviders {
	/// <summary>
	/// 默认的订单详情内容提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderSubjectProvider : IOrderSubjectProvider {
		/// <summary>
		/// 添加工具按钮
		/// </summary>
		public void AddToolButtons(
			SellerOrder order, IList<HtmlString> toolButtons, OrderOperatorType operatorType) {
			// TODO:
			// 复制收货人地址的按钮

		}

		/// <summary>
		/// 添加详细信息
		/// </summary>
		public void AddSubjects(
			SellerOrder order, IList<HtmlString> subjects, OrderOperatorType operatorType) {
			// TODO:
			// 收货地址

			// 买家留言和卖家留言

			// 订单编号

			// 下单时间

			// 支付方式

			// 配送方式
			// 这里只显示买家指定的，不显示后台实际发货使用的

		}
	}
}
