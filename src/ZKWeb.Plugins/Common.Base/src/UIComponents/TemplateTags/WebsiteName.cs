using DotLiquid;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Components.GenericConfigs;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 显示当前网站名称
	/// </summary>
	/// <example>
	/// {% website_name %}
	/// </example>
	public class WebsiteName : Tag {
		/// <summary>
		/// 描画网站名称
		/// </summary>
		/// <param name="context"></param>
		/// <param name="result"></param>
		public override void Render(Context context, TextWriter result) {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<WebsiteSettings>();
			result.Write(new T(settings.WebsiteName));
		}
	}
}
