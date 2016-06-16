using ZKWeb.Plugin;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.LanguageSwitcher.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			var navbarRight = areaManager.GetArea("header_navbar_right");
			var adminNavBar = areaManager.GetArea("admin_navbar");
			navbarRight.DefaultWidgets.Add("common.languageswitcher.widgets/language_switch_menu");
			adminNavBar.DefaultWidgets.Add("common.languageswitcher.widgets/admin_language_switch_menu");
		}
	}
}
