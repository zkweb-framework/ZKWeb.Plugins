using System.Collections.Generic;
using System.Linq;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 订单商品的显示信息的扩展函数
	/// </summary>
	public static class OrderProductDisplayInfoExtensions {
		/// <summary>
		/// 获取订单商品概述的Html
		/// 包含 主图缩略图, 名称, 匹配参数的描述
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetSummaryHtml(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_summary.html", new { info });
			return new HtmlString(html);
		}


		/// <summary>
		/// 获取订单商品卖家的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetSellerHtml(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_seller.html", new { info });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品价格的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetPriceHtml(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_price.html", new { info });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取编辑订单商品价格的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetPriceEditor(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.number_input_decimal.html",
				new { extraClass = "price", value = info.UnitPrice });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取编辑订单商品数量的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetCountEditor(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.number_input_integer.html",
				new { extraClass = "order-count", value = info.Count });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品数量的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetOrderCountHtml(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_count.html", new { info });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品已发数量的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetShippedCountHtml(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_shipped_count.html", new { info });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取编辑订单商品发货数量的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetDeliveryCountEditor(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.number_input_integer.html",
				new { extraClass = "delivery-count", value = info.Count - info.ShippedCount });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品概述列表的Html
		/// </summary>
		/// <param name="infos">订单商品的信息列表</param>
		/// <returns></returns>
		public static HtmlString GetSummryListHtml(this IEnumerable<OrderProductDisplayInfo> infos) {
			var summaryHtmls = infos.Select(info => info.GetSummaryHtml());
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_summary_list.html", new { summaryHtmls });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品价格列表的Html
		/// </summary>
		/// <param name="infos">订单商品的信息列表</param>
		/// <returns></returns>
		public static HtmlString GetPriceListHtml(this IEnumerable<OrderProductDisplayInfo> infos) {
			var priceHtmls = infos.Select(info => info.GetPriceHtml());
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_price_list.html", new { priceHtmls });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品数量列表的Html
		/// </summary>
		/// <param name="infos">订单商品的信息列表</param>
		/// <returns></returns>
		public static HtmlString GetOrderCountListHtml(this IEnumerable<OrderProductDisplayInfo> infos) {
			var orderCountHtmls = infos.Select(info => info.GetOrderCountHtml());
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_count_list.html", new { orderCountHtmls });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品已发数量列表的Html
		/// </summary>
		/// <param name="infos"></param>
		/// <returns></returns>
		public static HtmlString GetShippedCountListHtml(this IEnumerable<OrderProductDisplayInfo> infos) {
			var shippedCountHtmls = infos.Select(info => info.GetShippedCountHtml());
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_shipped_count_list.html", new { shippedCountHtmls });
			return new HtmlString(html);
		}
	}
}
