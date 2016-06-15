using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品匹配参数描述的提供器
	/// 用于构建商品匹配参数的字符串描述
	/// 可以用于
	/// - 显示在订单的商品信息中
	/// </summary>
	public interface IProductMatchParametersDescriptionProvider {
		/// <summary>
		/// 获取描述，没有时返回null
		/// </summary>
		/// <param name="product">商品Id</param>
		/// <param name="parameters">商品匹配参数</param>
		/// <returns></returns>
		string GetDescription(Database.Product product, IDictionary<string, object> parameters);
	}
}
