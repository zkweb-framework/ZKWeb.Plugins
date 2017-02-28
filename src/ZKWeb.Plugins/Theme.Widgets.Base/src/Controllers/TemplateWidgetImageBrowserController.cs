using ZKWeb.Plugins.CMS.ImageBrowser.src.Controllers.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.Widgets.Base.src.Controllers {
	/// <summary>
	/// 模板模块图片浏览器
	/// </summary>
	[ExportMany]
	public class TemplateWidgetImageBrowserController : ImageBrowserControllerBase {
		public override string Category { get { return "TemplateWidget"; } }
	}
}
