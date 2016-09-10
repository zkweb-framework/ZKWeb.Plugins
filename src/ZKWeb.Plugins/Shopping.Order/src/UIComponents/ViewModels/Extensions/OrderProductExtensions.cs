using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders.Interfaces;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 订单商品的扩展函数
	/// </summary>
	public static class OrderProductExtensions {
		/// <summary>
		/// 转换订单商品到显示信息
		/// </summary>
		/// <param name="orderProduct">订单商品</param>
		/// <returns></returns>
		public static OrderProductDisplayInfo ToDisplayInfo(this OrderProduct orderProduct) {
			var productAlbumManager = Application.Ioc.Resolve<ProductAlbumManager>();
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var descriptionProviders = Application.Ioc.ResolveMany<IProductMatchParametersDescriptionProvider>();
			var info = new OrderProductDisplayInfo();
			var product = orderProduct.Product;
			info.ProductId = product.Id;
			info.OrderProductId = null;
			info.Name = new T(product.Name);
			info.ImageWebPath = productAlbumManager.GetAlbumImageWebPath(
				product.Id, null, ProductAlbumImageType.Thumbnail);
			info.MatchedParameters = orderProduct.MatchParameters;
			info.MatchedParametersDescription = string.Join(" ", descriptionProviders
				.Select(p => p.GetDescription(product, orderProduct.MatchParameters))
				.Where(d => !string.IsNullOrEmpty(d)));
			info.UnitPrice = orderProduct.UnitPrice;
			info.OriginalUnitPrice = orderProduct.OriginalUnitPriceCalcResult.Parts.Sum();
			info.Currency = currencyManager.GetCurrency(orderProduct.Currency);
			info.UnitPriceString = info.Currency.Format(info.UnitPrice);
			info.UnitPriceDescription = orderProduct.UnitPriceCalcResult.Parts.GetDescription();
			info.OriginalUnitPriceString = info.Currency.Format(info.OriginalUnitPrice);
			info.OriginalUnitPriceDescription = orderProduct.OriginalUnitPriceCalcResult.Parts.GetDescription();
			info.Count = orderProduct.Count;
			info.ShippedCount = orderProduct.Deliveries.Sum(d => (long?)d.Count) ?? 0;
			info.SellerId = product.Seller?.Id;
			info.Seller = product.Seller?.Username;
			info.State = product.State;
			info.Type = product.Type;
			info.IsRealProduct = product.GetProductType() is IAmRealProduct;
			return info;
		}
	}
}
