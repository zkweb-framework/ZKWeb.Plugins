using DotLiquid;
using ZKWebStandard.Ioc;
using ZKWeb.Plugin;
using ZKWeb.Plugins.Common.Currency.src.UIComponents.TemplateFilters;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;

namespace ZKWeb.Plugins.Common.Currency.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册模板标签和过滤器
			Template.RegisterFilter(typeof(CurrencyFilters));
			// 允许描画货币
			Template.RegisterSafeType(typeof(ICurrency), Hash.FromAnonymousObject);
		}
	}
}
