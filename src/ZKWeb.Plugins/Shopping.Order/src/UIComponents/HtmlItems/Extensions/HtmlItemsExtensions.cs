using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.HtmlItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.HtmlItems.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.HtmlItems.Extensions {
	/// <summary>
	/// Html项列表的扩展函数
	/// </summary>
	public static class HtmlItemsExtensions {
		/// <summary>
		/// 添加订单列表过滤栏
		/// 适合添加到搜索栏的旁边
		/// </summary>
		/// <param name="items">html项列表</param>
		public static void AddOrderFilterBar(this List<HtmlItem> items) {
			var tabItems = new List<ListItem>();
			tabItems.Add(new ListItem(new T("All"), ""));
			foreach (var state in Enum.GetValues(typeof(OrderState)).OfType<OrderState>()) {
				tabItems.Add(new ListItem(new T(state.GetDescription()), ((int)state).ToString()));
			}
			items.AddAjaxTableFilterBar("State", tabItems);
		}
	}
}
