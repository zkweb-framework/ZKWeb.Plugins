using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 使用Api获取的商品信息
	/// </summary>
	public class ProductApiInfo : ILiquidizable {
		/// <summary>
		/// 商品名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 支持显示到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new { Name };
		}
	}
}
