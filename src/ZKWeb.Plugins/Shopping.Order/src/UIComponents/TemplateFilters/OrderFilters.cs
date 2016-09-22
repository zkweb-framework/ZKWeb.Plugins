using DotLiquid;
using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.TemplateFilters {
	/// <summary>
	/// 订单使用的模板过滤器
	/// </summary>
	public static class OrderFilters {
		/// <summary>
		/// 获取订单商品概述的Html
		/// 包含 主图缩略图, 名称, 匹配参数的描述
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString OrderProductSummary(object info) {
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
		public static HtmlString OrderProductSeller(object info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_seller.html", new { info });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品类型的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString OrderProductType(object info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_type.html", new { info });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取订单商品价格的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString OrderProductPrice(object info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_price.html", new { info });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取编辑订单商品数量的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString OrderProductCountEditor(object info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_count_editor.html", new { info });
			return new HtmlString(html);
		}

		/// <summary>
		/// 获取指定的订单状态在进度条中的css类
		/// 如果状态和当前状态一致，返回active
		/// 如果状态已经触发过，返回completed
		/// 如果状态没有触发过，返回空值
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <param name="state">指定状态的名称</param>
		/// <returns></returns>
		public static string OrderStateProgressClass(Hash info, string state) {
			var inState = info.Get<string>(nameof(OrderDisplayInfo.State));
			var stateTimes = info.Get<IDictionary<string, string>>(nameof(OrderDisplayInfo.StateTimes));
			if (state == inState) {
				return "active";
			} else if (stateTimes.ContainsKey(state)) {
				return "completed";
			}
			return "";
		}
	}
}
