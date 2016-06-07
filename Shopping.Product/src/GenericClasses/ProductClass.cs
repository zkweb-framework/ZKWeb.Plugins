using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Product.src.GenericClasses {
	/// <summary>
	/// 商品分类
	/// </summary>
	[ExportMany]
	public class ProductClass : GenericClassBuilder {
		public override string Name { get { return "ProductClass"; } }
	}
}
