using DotLiquid;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 显示版权信息文本
	/// </summary>
	/// <example>
	/// {% copyright_text %}
	/// </example>
	public class CopyrightText : Tag {
		/// <summary>
		/// 显示版权信息文本
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<WebsiteSettings>();
			result.Write(new T(settings.CopyrightText));
		}
	}
}
