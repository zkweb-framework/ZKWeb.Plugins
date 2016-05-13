using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductMatchedDataMatchers {
	/// <summary>
	/// 匹配规格(销售属性)
	/// </summary>
	[ExportMany]
	public class PropertyMatcher : IProductMatchedDataMatcher {
		/// <summary>
		/// 属性条件或参数的格式
		/// </summary>
		class PropertyCondition { public long PropertyId; public long PropertyValueId; }

		/// <summary>
		/// 判断是否匹配
		/// </summary>
		public bool IsMatched(IDictionary<string, object> parameters, ProductMatchedData data) {
			// 获取规格的条件
			// 格式 [ { PropertyId: ..., PropertyValueId: ... }, ... ]
			var exceptedProperties = data.Conditions.GetOrDefault<IList<PropertyCondition>>("Properties");
			if (exceptedProperties == null || !exceptedProperties.Any()) {
				return true; // 没有指定条件
			}
			// 判断参数中的规格值列表是否包含条件中的所有规格值
			// 例如 参数 { 颜色: 黑色, 尺码: XXS, 款式: 2013 }, 条件 { 颜色: 黑色, 尺码: XXS }时匹配成功
			// 参数的格式同上
			var incomeProperties = parameters.GetOrDefault<IList<PropertyCondition>>("Properties");
			if (incomeProperties == null || !incomeProperties.Any()) {
				return false; // 有指定条件，但参数中没有包含任何规格值
			}
			var incomePropertiesMapping = new Dictionary<long, long?>();
			foreach (var obj in incomeProperties) {
				incomePropertiesMapping[obj.PropertyId] = obj.PropertyValueId;
			}
			return exceptedProperties.All(obj => {
				return incomePropertiesMapping.GetOrDefault(obj.PropertyId) == obj.PropertyValueId;
			});
		}

		/// <summary>
		/// 获取使用Javascript判断是否匹配的函数
		/// </summary>
		/// <returns></returns>
		public string GetJavascriptMatchFunction() {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			return File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "matched_data_matchers", "property.matcher.js"));
		}
	}
}
