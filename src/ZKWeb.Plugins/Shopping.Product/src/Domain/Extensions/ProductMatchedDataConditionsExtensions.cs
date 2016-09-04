using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions {
	/// <summary>
	/// 商品匹配数据的匹配条件的扩展函数
	/// </summary>
	public static class ProductMatchedDataConditionsExtensions {
		/// <summary>
		/// 商品属性条件
		/// </summary>
		public class PropertyCondition {
			public Guid PropertyId { get; set; }
			public Guid PropertyValueId { get; set; }
		}

		/// <summary>
		/// 获取订购数量大于或等于指定数量的条件
		/// </summary>
		/// <param name="conditions">匹配条件</param>
		/// <returns></returns>
		public static long? GetOrderCountGE(this ProductMatchedDataConditions conditions) {
			return conditions.GetOrDefault<long?>("OrderCountGE");
		}

		/// <summary>
		/// 获取商品属性条件
		/// </summary>
		/// <param name="conditions">匹配条件</param>
		/// <returns></returns>
		public static IList<PropertyCondition> GetProperties(this ProductMatchedDataConditions conditions) {
			return conditions.GetOrDefault<IList<PropertyCondition>>("Properties");
		}
	}
}
