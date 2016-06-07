using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.GenericTag.src.Scaffolding;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Product.src.GenericTags {
	/// <summary>
	/// 商品标签
	/// </summary>
	[ExportMany]
	public class ProductTag : GenericTagBuilder {
		public override string Name { get { return "ProductTag"; } }
	}
}
