using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Localize;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 显示版权信息文本
	/// <example>
	/// {% copyright_text %}
	/// </example>
	/// </summary>
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
