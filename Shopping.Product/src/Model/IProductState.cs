using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品状态的接口
	/// </summary>
	public interface IProductState {
		/// <summary>
		/// 商品状态
		/// </summary>
		string State { get; }
	}
}
