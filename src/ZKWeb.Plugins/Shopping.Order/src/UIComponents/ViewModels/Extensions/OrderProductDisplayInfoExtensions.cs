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
		/// 获取编辑订单商品数量的Html
		/// </summary>
		/// <param name="info">订单商品的信息</param>
		/// <returns></returns>
		public static HtmlString GetCountEditor(this OrderProductDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"shopping.order/tmpl.order_product_count_editor.html", new { info });
			return new HtmlString(html);
		}
	}
}
