using DotLiquid;
using System.IO;
using ZKWeb.Plugins.Common.Base.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.TemplateTags {
	/// <summary>
	/// 描画页面描述
	/// 如果有指定关键词时使用指定的描述，否则使用网站设置中的描述
	/// </summary>
	/// <example>
	/// {% render_meta_description %}
	/// </example>
	public class RenderMetaDescription : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__meta_description";

		/// <summary>
		/// 描画页面描述
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var description = (context[Key] ?? "").ToString();
			if (string.IsNullOrEmpty(description)) {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<WebsiteSettings>();
				description = settings.PageDescription;
			}
			result.Write(string.Format("<meta name='description' content='{0}' />",
				HttpUtils.HtmlEncode(description)));
		}
	}
}
