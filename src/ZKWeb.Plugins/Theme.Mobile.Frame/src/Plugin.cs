using ZKWeb.Plugin;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.Mobile.Frame.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			var footerNavBar = areaManager.GetArea("mobile_footer_navbar");
			footerNavBar.DefaultWidgets.Add("theme.mobile.frame.widgets/mobile_footer_navbar");
		}
	}
}
