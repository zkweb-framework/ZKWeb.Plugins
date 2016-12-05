using System.Linq;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders.Interfaces;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders.Extensions {
	/// <summary>
	/// 商品的扩展函数
	/// </summary>
	public static class ProductExtensions {
		/// <summary>
		/// 获取商品匹配参数的描述
		/// </summary>
		/// <param name="product">商品</param>
		/// <param name="parameters">商品匹配参数</param>
		/// <returns></returns>
		public static string GetMatchParametersDescription(
			this Domain.Entities.Product product,
			ProductMatchParameters parameters) {
			var descriptionProviders = Application.Ioc
				.ResolveMany<IProductMatchParametersDescriptionProvider>();
			return string.Join(" ", descriptionProviders
				.OrderBy(p => p.DisplayOrder)
				.Select(p => p.GetDescription(product, parameters))
				.Where(d => !string.IsNullOrEmpty(d)));
		}
	}
}
