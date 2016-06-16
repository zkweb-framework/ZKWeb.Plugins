using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 描画页面关键词
	/// 如果有指定关键词时使用指定的关键词，否则使用网站设置中的关键词
	/// <example>
	/// {% render_meta_keywords %}
	/// </example>
	/// </summary>
	public class RenderMetaKeywords : Tag {
		/// <summary>
		/// 变量名
		/// </summary>
		public const string Key = "__meta_keywords";

		/// <summary>
		/// 描画页面关键词
		/// </summary>
		public override void Render(Context context, TextWriter result) {
			var keywords = (context[Key] ?? "").ToString();
			if (string.IsNullOrEmpty(keywords)) {
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var settings = configManager.GetData<WebsiteSettings>();
				keywords = settings.PageKeywords;
			}
			result.Write(string.Format("<meta name='keywords' content='{0}' />",
				HttpUtils.HtmlEncode(keywords)));
		}
	}
}
