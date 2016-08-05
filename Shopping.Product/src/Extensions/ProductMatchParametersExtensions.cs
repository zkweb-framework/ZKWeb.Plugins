using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Extensions {
	/// <summary>
	/// 商品匹配参数的扩展函数
	/// </summary>
	public static class ProductMatchParametersExtensions {
		/// <summary>
		/// 商品属性参数
		/// </summary>
		public class PropertyParameter {
			public long PropertyId { get; set; }
			public long PropertyValueId { get; set; }
		}

		/// <summary>
		/// 获取商品属性参数
		/// </summary>
		/// <param name="parameters">商品匹配参数</param>
		/// <returns></returns>
		public static IList<PropertyParameter> GetProperties(this ProductMatchParameters parameters) {
			return parameters.GetOrDefault<IList<PropertyParameter>>("Properties");
		}

		/// <summary>
		/// 获取订购数量
		/// </summary>
		/// <param name="parameters">商品匹配参数</param>
		/// <returns></returns>
		public static long GetOrderCount(this ProductMatchParameters parameters) {
			return parameters.GetOrDefault<long?>("OrderCount") ?? 0;
		}

		/// <summary>
		/// 设置订购数量
		/// </summary>
		/// <param name="parameters">商品匹配参数</param>
		/// <param name="orderCount">订购数量</param>
		public static void SetOrderCount(this ProductMatchParameters parameters, long orderCount) {
			parameters["OrderCount"] = orderCount;
		}
	}
}
