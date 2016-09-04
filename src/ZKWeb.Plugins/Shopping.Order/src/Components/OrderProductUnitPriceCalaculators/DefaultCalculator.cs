using System;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderProductUnitPriceCalaculators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderProductUnitPriceCalaculators {
	/// <summary>
	/// 默认的订单商品单价计算器
	/// </summary>
	[ExportMany]
	public class DefaultCalculator : IOrderProductUnitPriceCalculator {
		/// <summary>
		/// 从商品匹配数据获取匹配的价格和货币单位
		/// </summary>
		public void Calculate(
			Guid? userId, CreateOrderProductParameters parameters, OrderPriceCalcResult result) {
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var product = productManager.GetWithCache(parameters.ProductId);
			if (product == null) {
				throw new BadRequestException(new T("The product you are try to purchase does not exist."));
			}
			var data = product.MatchedDatas
				.Where(d => d.Price != null)
				.WhereMatched(parameters.MatchParameters).FirstOrDefault();
			var basePrice = (data == null) ? 0M : data.Price.Value;
			var currency = (data == null) ? currencyManager.GetDefaultCurrency() : data.GetCurrency();
			result.Currency = currency.Type;
			result.Parts.Add(new OrderPriceCalcResult.Part("ProductUnitPrice", basePrice));
		}
	}
}
