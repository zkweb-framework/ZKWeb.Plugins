using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Extensions {
	using Product = Product.src.Database.Product;

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
			var productAlbumManager = Application.Ioc.Resolve<ProductAlbumManager>();
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var descriptionProviders = Application.Ioc.ResolveMany<IProductMatchParametersDescriptionProvider>();
			var unitPrice = orderManager.CalculateOrderProductUnitPrice(userId, parameters);
			var info = new OrderProductDisplayInfo();
			UnitOfWork.ReadData<Product>(r => {
				var product = r.GetById(parameters.ProductId);
				if (product == null) {
					throw new HttpException(400, new T("The product you are try to purchase does not exist."));
				}
				info.ProductId = product.Id;
				info.OrderProductId = 0;
				info.Name = new T(product.Name);
				info.ImageWebPath = productAlbumManager.GetAlbumImageWebPath(
					product.Id, null, ProductAlbumImageType.Thumbnail);
				info.MatchedParametersDescription = string.Join(" ", descriptionProviders
					.Select(p => p.GetDescription(product, parameters.MatchParameters))
					.Where(d => !string.IsNullOrEmpty(d)));
			});
			info.Price = unitPrice.Parts.Sum();
			info.OriginalPrice = info.Price;
			info.Currency = currencyManager.GetCurrency(unitPrice.Currency);
			info.PriceDescription = unitPrice.Parts.GetDescription();
			info.OriginalPriceDescription = info.PriceDescription;
			info.Count = parameters.MatchParameters.GetOrDefault<long>("OrderCount");
			return info;
		}
	}
}
