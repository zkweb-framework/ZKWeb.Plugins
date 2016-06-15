using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductStates {
	/// <summary>
	/// 准备上架
	/// </summary>
	[ExportMany]
	public class WaitForSales : IProductState {
		/// <summary>
		/// 商品状态
		/// </summary>
		public string State { get { return "WaitForSales"; } }
	}
}
