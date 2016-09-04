using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;

namespace ZKWeb.Plugins.Shopping.Product.src.Components.ProductMatchedDataMatchers.Interfaces {
	/// <summary>
	/// 商品匹配数据的匹配器
	/// </summary>
	public interface IProductMatchedDataMatcher {
		/// <summary>
		/// 判断是否匹配
		/// </summary>
		/// <param name="parameters">商品匹配参数</param>
		/// <param name="data">商品匹配数据</param>
		/// <returns></returns>
		bool IsMatched(ProductMatchParameters parameters, ProductMatchedData data);

		/// <summary>
		/// 获取使用Javascript判断是否匹配的函数
		/// 用于商品详情页的价格和库存显示
		/// </summary>
		/// <returns></returns>
		string GetJavascriptMatchFunction();
	}
}
