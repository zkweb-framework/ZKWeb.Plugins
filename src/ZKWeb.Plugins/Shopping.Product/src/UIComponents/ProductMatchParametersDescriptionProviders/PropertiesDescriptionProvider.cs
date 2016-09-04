using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders.Interfaces;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders {
	using Product = Domain.Entities.Product;

	/// <summary>
	/// 获取属性值的描述
	/// 例如 "颜色: 红色 尺码: L"
	/// </summary>
	[ExportMany]
	public class PropertiesDescriptionProvider : IProductMatchParametersDescriptionProvider {
		/// <summary>
		/// 获取描述，没有时返回null
		/// </summary>
		public string GetDescription(Product product, ProductMatchParameters parameters) {
			var properties = parameters.GetProperties();
			if (properties == null || properties.Count <= 0) {
				return null;
			}
			var parts = new List<string>();
			foreach (var productToPropertyValue in product.FindPropertyValuesFromPropertyParameters(properties)) {
				parts.Add(string.Format("{0}: {1}",
					new T(productToPropertyValue.Property.Name),
					new T(productToPropertyValue.PropertyValueName)));
			}
			return string.Join(" ", parts);
		}
	}
}
