using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Extensions {
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
			this CreateOrderProductParameters parameters, long? userId) {
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var productAlbumManager = Application.Ioc.Resolve<ProductAlbumManager>();
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var descriptionProviders = Application.Ioc.ResolveMany<IProductMatchParametersDescriptionProvider>();
			var unitPrice = orderManager.CalculateOrderProductUnitPrice(userId, parameters);
			var info = new OrderProductDisplayInfo();
			var product = productManager.GetProduct(parameters.ProductId);
			if (product == null) {
				throw new BadRequestException(new T("The product you are try to purchase does not exist."));
			}
			info.ProductId = product.Id;
			info.OrderProductId = 0;
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
			info.Count = parameters.MatchParameters.GetOrDefault<long>("OrderCount");
			info.SellerId = (product.Seller == null) ? null : (long?)product.Seller.Id;
			info.Seller = (product.Seller == null) ? null : product.Seller.Username;
			info.State = product.State;
			info.StateTrait = product.GetStateTrait();
			info.Type = product.Type;
			info.TypeTrait = product.GetTypeTrait();
			return info;
		}

		/// <summary>
		/// 获取包含了实体商品的卖家Id列表
		/// 无卖家的Id等于0
		/// </summary>
		/// <param name="parameters">订单商品创建参数</param>
		/// <returns></returns>
		public static IList<long> GetSellerIdsHasRealProducts(
			this IList<CreateOrderProductParameters> parametersList) {
			var productManager = Application.Ioc.Resolve<ProductManager>();
			return parametersList
				.Select(p => productManager.GetProduct(p.ProductId))
				.Where(p => p.GetTypeTrait().IsReal)
				.Select(p => p.Seller == null ? 0 : p.Seller.Id)
				.Distinct().ToList();
		}
	}
}
