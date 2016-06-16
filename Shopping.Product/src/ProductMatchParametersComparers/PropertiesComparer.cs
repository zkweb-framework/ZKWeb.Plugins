using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductMatchParametersComparers {
	/// <summary>
	/// 比较参数中的商品属性值列表
	/// 这个比较器不会比较名称，如果商品修改过别名但Id相同还是会返回相等
	/// </summary>
	[ExportMany]
	public class PropertiesComparer : IProductMatchParametersComparer {
		/// <summary>
		/// 判断商品匹配参数是否部分相等
		/// </summary>
		public bool IsPartialEqual(
			IDictionary<string, object> lhs, IDictionary<string, object> rhs) {
			var propertiesLhs = lhs.GetOrDefault<IList<ProductToPropertyValueForMatch>>("Properties");
			var propertiesRhs = rhs.GetOrDefault<IList<ProductToPropertyValueForMatch>>("Properties");
			if (propertiesLhs == null && propertiesRhs == null) {
				return true;
			} else if (propertiesLhs == null || propertiesRhs == null) {
				return false;
			}
			var disinctCount = propertiesLhs.Concat(propertiesRhs) // 计算并集去除重复后的大小
				.Select(p => new Pair<long, long?>(p.PropertyId, p.PropertyValueId)).Distinct().Count();
			return propertiesLhs.Count == disinctCount && propertiesLhs.Count == propertiesRhs.Count;
		}
	}
}
