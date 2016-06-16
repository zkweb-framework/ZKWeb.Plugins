using System.Collections.Generic;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品匹配参数比较器
	/// 在所有比较器返回相等时才会判断参数相等
	/// 可以用于
	/// - 添加商品到购物车时判断是否需要合并商品
	/// </summary>
	public interface IProductMatchParametersComparer {
		/// <summary>
		/// 比较商品匹配参数是否部分相等
		/// </summary>
		/// <param name="lhs">商品匹配参数</param>
		/// <param name="rhs">商品匹配参数</param>
		/// <returns></returns>
		bool IsPartialEqual(IDictionary<string, object> lhs, IDictionary<string, object> rhs);
	}
}
