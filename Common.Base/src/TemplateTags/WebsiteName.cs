using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DryIoc;
using ZKWeb.Model;
using ZKWeb.Plugins.Common.Base.src.Config;
using ZKWeb.Core;

namespace ZKWeb.Plugins.Common.Base.src.TemplateTags {
	/// <summary>
	/// 显示当前网站名称
	/// 例子
	/// {{ "Website name is" | trans }}{% website_name %}
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
