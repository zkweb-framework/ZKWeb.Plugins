using ZKWeb.Plugin;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src {
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
			// 商品详情页
			areaManager.GetArea("product_gallery").DefaultWidgets.Add("shopping.productbookmark.widgets/product_add_bookmark");
		}
	}
}
