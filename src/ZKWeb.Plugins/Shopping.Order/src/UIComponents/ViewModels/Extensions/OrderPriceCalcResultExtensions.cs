using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 订单价格计算结果的扩展函数
	/// </summary>
	public static class OrderPriceCalcResultExtensions {
		/// <summary>
		/// 获取组成部分的影响量的编辑器
		/// </summary>
		public static HtmlString GetDeltaEditor(this OrderPriceCalcResult.Part part) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.number_input_negative_decimal.html",
				new { extraClass = "part-delta", value = part.Delta });
			return new HtmlString(html);
		}
	}
}
