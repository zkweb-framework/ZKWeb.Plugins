using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductMatchParametersDescriptionProviders {
	/// <summary>
	/// 获取属性值的描述
	/// 例如 "颜色: 红色 尺码: L"
	/// </summary>
	[ExportMany]
	public class PropertiesDescriptionProvider : IProductMatchParametersDescriptionProvider {
		/// <summary>
		/// 获取描述，没有时返回null
		/// </summary>
		public string GetDescription(Database.Product product, IDictionary<string, object> parameters) {
			var properties = parameters.GetOrDefault<IList<ProductToPropertyValueForMatch>>("Properties");
			if (properties == null) {
				return null;
			}
			var propertiesMap = properties.ToDictionary(p => p.PropertyId);
			var parts = new List<string>();
			foreach (var property in product.Category.OrderedProperties()) {
				var value = propertiesMap.GetOrDefault(property.Id);
				if (value == null) {
					continue;
				}
				parts.Add(string.Format("{0}: {1}", new T(property.Name), new T(value.PropertyValueName)));
			}
			return string.Join(" ", parts);
		}
	}
}
