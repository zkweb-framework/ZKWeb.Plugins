using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;

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
		public string GetDescription(Database.Product product, ProductMatchParameters parameters) {
			var properties = parameters.GetProperties();
			if (properties == null || properties.Count <= 0) {
				return null;
			}
			var parts = new List<string>();
			foreach (var propertyValue in product.FindPropertyValuesFromPropertyParameters(properties)) {
				parts.Add(string.Format("{0}: {1}",
					new T(propertyValue.Property.Name),
					new T(propertyValue.PropertyValueName)));
			}
			return string.Join(" ", parts);
		}
	}
}
