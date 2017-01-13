using ZKWeb.Plugin;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src {
	/// <summary>
	/// 插件入口点
	/// </summary>
	[ExportMany]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			// 商品评价页
			areaManager.GetArea("order_rate_contents")
				.DefaultWidgets.Add("shopping.productrating.widgets/order-rate-form");
		}
	}
}
