using System.Text;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 订单显示信息的扩展函数
	/// </summary>
	public static class OrderDisplayInfoExtensions {
		/// <summary>
		/// 获取订单列表页的表格头部
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetTableHeadingHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_list.table_heading.html", new { info }));
		}

		/// <summary>
		/// 获取订单总价的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetTotalCostHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_list.total_cost.html", new { info }));
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
				"shopping.order/tmpl.order_list.total_cost_with_parts.html", new { info }));
		}

		/// <summary>
		/// 获取订单基本信息的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetBaseInformationHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.tab_base_information.html", new { info }));
		}
		
		/// <summary>
		/// 获取订单留言的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetOrderCommentsHtml(this OrderDisplayInfo info) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate(
				"shopping.order/tmpl.order_view.tab_comments.html", new { info }));
		}

		/// <summary>
		/// 获取订单操作在表格单元格(订单列表页)中的Html
		/// </summary>
		/// <param name="info">订单显示信息</param>
		/// <returns></returns>
		public static HtmlString GetOrderActionsTableCellHtml(this OrderDisplayInfo info) {
			var result = new StringBuilder();
			foreach (var action in info.ActionHtmls) {
				var actionHtml = action.ToString();
				if (actionHtml.Contains("btn-danger") || actionHtml.Contains("action-disabled")) {
					continue;
				}
				result.AppendLine(actionHtml);
			}
			return new HtmlString(result.ToString());
		}
	}
}
