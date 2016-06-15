using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Managers;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 描画页面描述
	/// 如果有指定关键词时使用指定的描述，否则使用网站设置中的描述
	/// <example>
	/// {% render_meta_description %}
	/// </example>
	/// </summary>
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
				HttpUtility.HtmlAttributeEncode(description)));
		}
	}
}
