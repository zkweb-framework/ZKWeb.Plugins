using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Product.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductStates {
	/// <summary>
	/// 准备上架
	/// </summary>
	public class WaitForSales : IProductState {
		/// <summary>
		/// 商品状态
		/// </summary>
		public string State { get { return "WaitForSales"; } }
	}
}
