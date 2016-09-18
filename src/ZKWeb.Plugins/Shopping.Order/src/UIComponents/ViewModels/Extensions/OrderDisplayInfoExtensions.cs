using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 订单显示信息的扩展函数
	/// </summary>
	public static class OrderDisplayInfoExtensions {
		/// <summary>
		/// 获取后台订单管理使用的表格头部
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetTableHeadingHtmlForAdmin(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_table_heading_for_admin.html", new { info }));
		}

		/// <summary>
		/// 获取订单总价的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetTotalCostHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_total_cost.html", new { info }));
		}

		/// <summary>
		/// 获取订单总价和价格组成部分的Html
		/// 会同时显示订单总价和价格组成部分
		/// 但组成部分中的商品总价会忽略显示
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetTotalCostWithPartsHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_total_cost_with_parts.html", new { info }));
		}

		/// <summary>
		/// 获取管理员查看的订单基本信息Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetBaseInformationHtmlForAdmin(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_base_information_for_admin.html", new { info }));
		}

		/// <summary>
		/// 获取管理员查看的订单发货记录Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetDeliveryRecordsHtmlForAdmin(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_delivery_records_for_admin.html", new { info }));
		}

		/// <summary>
		/// 获取管理员查看的订单记录Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetOrderRecordsHtmlForAdmin(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_records_for_admin.html", new { info }));
		}

		/// <summary>
		/// 获取管理员查看的关联交易Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetReleatedTransactionsHtmlForAdmin(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_releated_transactions_for_admin.html", new { info }));
		}

		/// <summary>
		/// 获取管理员查看的订单留言Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetOrderCommentsHtmlForAdmin(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_comments_for_admin.html", new { info }));
		}
	}
}
