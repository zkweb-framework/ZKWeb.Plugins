using DotLiquid;
using ZKWeb.Plugin;
using ZKWeb.Plugins.Common.QRCoder.src.UIComponents.TemplateTags;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.QRCoder.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册模板标签
			Template.RegisterTag<QRCode>("qrcode");
		}
	}
}
