using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.HtmlItems;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Templating;
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
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var states = Enum.GetValues(typeof(OrderState)).OfType<OrderState>()
				.Select(s => new { name = new T(s.GetDescription()), value = (int)s });
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_list.status_filter.html", new { states });
			items.Add(new HtmlItem(html));
		}
	}
}
