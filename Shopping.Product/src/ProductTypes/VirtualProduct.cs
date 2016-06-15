using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductTypes {
	/// <summary>
	/// 虚拟商品
	/// </summary>
	[ExportMany]
	public class VirtualProduct : IProductType {
		/// <summary>
		/// 商品类型
		/// </summary>
		public string Type { get { return "VirtualProduct"; } }
	}
}
