using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Scaffolding;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Product.src.ImageBrowsers {
	/// <summary>
	/// 商品图片浏览器
	/// </summary>
	[ExportMany]
	public class ProductImageBrowser : ImageBrowserBuilder {
		public override string Category { get { return "Product"; } }
	}
}
