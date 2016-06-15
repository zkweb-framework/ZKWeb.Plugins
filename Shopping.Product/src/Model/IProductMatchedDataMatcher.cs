using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Shopping.Product.src.Database;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
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
		bool IsMatched(IDictionary<string, object> parameters, ProductMatchedData data);

		/// <summary>
		/// 获取使用Javascript判断是否匹配的函数
		/// 用于商品详情页的价格和库存显示
		/// </summary>
		/// <returns></returns>
		string GetJavascriptMatchFunction();
	}
}
