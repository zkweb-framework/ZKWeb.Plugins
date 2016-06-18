using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Logistics.src.Manager;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Shopping.Order.src.Managers {
	/// <summary>
	/// 订单管理器
	/// </summary>
	[ExportMany]
	public class OrderManager {
		/// <summary>
		/// 计算订单商品的单价
		/// 返回价格保证大于或等于0
		/// </summary>
		/// <param name="userId">用户Id，未登录时等于null</param>
		/// <param name="parameters">创建订单商品的参数</param>
		/// <returns></returns>
		public virtual OrderPriceCalcResult CalculateOrderProductUnitPrice(
			long? userId, CreateOrderProductParameters parameters) {
			var result = new OrderPriceCalcResult();
			var calculators = Application.Ioc.ResolveMany<IOrderProductUnitPriceCalculator>();
			foreach (var calculator in calculators) {
				calculator.Calculate(userId, parameters, result);
			}
			if (result.Parts.Sum() < 0) {
				throw new HttpException(400, new T("Order product unit price must not be negative"));
			}
			return result;
		}

		/// <summary>
		/// 计算订单的价格
		/// 返回价格保证大于0
		/// </summary>
		/// <param name="parameters">创建订单的参数</param>
		/// <returns></returns>
		public virtual OrderPriceCalcResult CalculateOrderPrice(
			CreateOrderParameters parameters) {
			var result = new OrderPriceCalcResult();
			var calculators = Application.Ioc.ResolveMany<IOrderPriceCalculator>();
			foreach (var calculator in calculators) {
				calculator.Calculate(parameters, result);
			}
			if (result.Parts.Sum() <= 0) {
				throw new HttpException(400, new T("Order cost must larger than 0"));
			}
			return result;
		}

		/// <summary>
		/// 计算订单的运费
		/// 返回 ((运费, 货币), 错误信息)
		/// </summary>
		/// <returns></returns>
		public virtual Pair<Pair<decimal, string>, string> CalculateLogisticsCost(
			long logisticsId, CreateOrderParameters parameters) {
			// 获取收货地址中的国家和地区
			var shippingAddress = (parameters.OrderParameters
				.GetOrDefault<IDictionary<string, object>>("ShippingAddress") ??
				new Dictionary<string, object>());
			var country = shippingAddress.GetOrDefault<string>("Country");
			var regionId = shippingAddress.GetOrDefault<long?>("RegionId");
			// 获取订单商品的总重量
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var matchers = Application.Ioc.ResolveMany<IProductMatchedDataMatcher>();
			var totalWeight = 0M;
			foreach (var orderProductParameter in parameters.OrderProductParametersList) {
				var product = productManager.GetProduct(orderProductParameter.ProductId);
				var orderCount = orderProductParameter.MatchParameters.GetOrDefault<long>("OrderCount");
				foreach (var data in product.MatchedDatas) {
					if (data.Weight != null &&
						matchers.All(m => m.IsMatched(orderProductParameter.MatchParameters, data))) {
						totalWeight += checked(totalWeight + data.Weight.Value * orderCount);
						break;
					}
				}
			}
			// 使用物流管理器计算运费
			var logisticsManager = Application.Ioc.Resolve<LogisticsManager>();
			return logisticsManager.CalculateCost(logisticsId, country, regionId, totalWeight);
		}
	}
}
