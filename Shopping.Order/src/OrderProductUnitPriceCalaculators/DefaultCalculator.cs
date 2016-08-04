using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderProductUnitPriceCalaculators {
	/// <summary>
	/// 默认的订单商品单价计算器
	/// </summary>
	[ExportMany]
	public class DefaultCalculator : IOrderProductUnitPriceCalculator {
		/// <summary>
		/// 从商品匹配数据获取匹配的价格和货币单位
		/// </summary>
		public void Calculate(
			long? userId, CreateOrderProductParameters parameters, OrderPriceCalcResult result) {
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var product = productManager.GetProduct(parameters.ProductId);
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
