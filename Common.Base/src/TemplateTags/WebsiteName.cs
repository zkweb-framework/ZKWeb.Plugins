using DotLiquid;
using System.IO;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Localize;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 显示当前网站名称
	/// <example>
	/// {% website_name %}
	/// </example>
	/// </summary>
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
