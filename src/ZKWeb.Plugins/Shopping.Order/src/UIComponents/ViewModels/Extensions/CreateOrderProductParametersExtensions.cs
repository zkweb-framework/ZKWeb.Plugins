using System;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders.Interfaces;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 订单商品创建参数的扩展函数
	/// </summary>
	public static class CreateOrderProductParametersExtensions {
		/// <summary>
		/// 从订单商品创建参数生成显示信息
		/// </summary>
		/// <param name="parameters">订单商品创建参数</param>
		/// <param name="userId">用户Id，未登录时等于null</param>
		/// <returns></returns>
		public static OrderProductDisplayInfo ToOrderProductDisplayInfo(
			this CreateOrderProductParameters parameters, Guid? userId) {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var productAlbumManager = Application.Ioc.Resolve<ProductAlbumManager>();
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var descriptionProviders = Application.Ioc.ResolveMany<IProductMatchParametersDescriptionProvider>();
			var unitPrice = orderManager.CalculateOrderProductUnitPrice(userId, parameters);
			var info = new OrderProductDisplayInfo();
			var product = productManager.GetWithCache(parameters.ProductId);
			if (product == null) {
				throw new BadRequestException(new T("The product you are try to purchase does not exist."));
			}
			info.ProductId = product.Id;
			info.OrderProductId = null;
			info.Name = new T(product.Name);
			info.ImageWebPath = productAlbumManager.GetAlbumImageWebPath(
				product.Id, null, ProductAlbumImageType.Thumbnail);
			info.MatchedParameters = parameters.MatchParameters;
			info.MatchedParametersDescription = string.Join(" ", descriptionProviders
				.Select(p => p.GetDescription(product, parameters.MatchParameters))
				.Where(d => !string.IsNullOrEmpty(d)));
			info.UnitPrice = unitPrice.Parts.Sum();
			info.OriginalUnitPrice = info.UnitPrice;
			info.Currency = currencyManager.GetCurrency(unitPrice.Currency);
			info.UnitPriceString = info.Currency.Format(info.UnitPrice);
			info.UnitPriceDescription = unitPrice.Parts.GetDescription();
			info.OriginalUnitPriceString = info.UnitPriceString;
			info.OriginalUnitPriceDescription = info.UnitPriceDescription;
			info.Count = parameters.MatchParameters.GetOrderCount();
			info.ShippedCount = 0;
			info.SellerId = product.Seller?.Id;
			info.Seller = product.Seller?.Username;
			info.State = product.State;
			info.Type = product.Type;
			info.IsRealProduct = product.GetProductType() is IAmRealProduct;
			return info;
		}
	}
}
