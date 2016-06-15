using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.GenericClasses {
	/// <summary>
	/// 商品分类
	/// </summary>
	[ExportMany]
	public class ProductClass : GenericClassBuilder {
		public override string Name { get { return "ProductClass"; } }
	}
}
