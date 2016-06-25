using System;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderPriceCalculators {
	/// <summary>
	/// 默认的订单价格计算器
	/// </summary>
	[ExportMany]
	public class DefaultCalculator : IOrderPriceCalculator {
		/// <summary>
		/// 设置订单价格等于以下的合计
		/// - 商品总价
		/// - 运费
		/// </summary>
		public void Calculate(CreateOrderParameters parameters, OrderPriceCalcResult result) {
			// 计算商品总价
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var orderProductTotalPrice = 0.0M;
			var currencyIsSet = false;
			foreach (var productParameters in parameters.OrderProductParametersList) {
				// 计算商品单价
				var productResult = orderManager.CalculateOrderProductUnitPrice(
					parameters.UserId, productParameters);
				// 货币按第一个商品设置，后面检查货币是否一致
				if (!currencyIsSet) {
					currencyIsSet = true;
					result.Currency = productResult.Currency;
				} else if (result.Currency != productResult.Currency) {
					throw new HttpException(400, new T("Create order contains multi currency is not supported"));
				}
				// 添加到商品总价
				var orderCount = productParameters.MatchParameters.GetOrDefault<long>("OrderCount");
				if (orderCount <= 0) {
					throw new HttpException(400, new T("Order count must larger than 0"));
				}
				orderProductTotalPrice = checked(orderProductTotalPrice + productResult.Parts.Sum() * orderCount);
			}
			// 添加商品总价到订单价格的组成部分
			result.Parts.Add(new OrderPriceCalcResult.Part("ProductTotalPrice", orderProductTotalPrice));
			// 计算运费
			// 根据真实的订单商品的卖家选择
			// TODO: LogisticsWithSeller
			throw new NotImplementedException();
			var logisticsId = parameters.OrderParameters.GetOrDefault<long?>("LogisticsId");
			if (logisticsId != null) {
				var logisticsCost = orderManager.CalculateLogisticsCost(logisticsId.Value, parameters);
				if (!string.IsNullOrEmpty(logisticsCost.Second)) {
					// 计算运费错误
					throw new HttpException(400, logisticsCost.Second);
				} else if (logisticsCost.First.Second != result.Currency) {
					// 货币不一致
					throw new HttpException(400, new T("Create order contains multi currency is not supported"));
				}
				// 添加运费到订单价格的组成部分
				result.Parts.Add(new OrderPriceCalcResult.Part("LogisticsCost", logisticsCost.First.First));
			}
		}
	}
}
