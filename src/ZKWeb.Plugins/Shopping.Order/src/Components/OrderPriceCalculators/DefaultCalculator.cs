using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderPriceCalculators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderPriceCalculators {
	/// <summary>
	/// 默认的订单价格计算器
	/// </summary>
	[ExportMany]
	public class DefaultCalculator : IOrderPriceCalculator {
		/// <summary>
		/// 设置订单价格等于以下的合计
		/// - 商品总价
		/// - 运费
		/// - 支付手续费
		/// 注意
		/// - 传入的参数中可能会包含多个卖家
		/// </summary>
		public void Calculate(CreateOrderParameters parameters, OrderPriceCalcResult result) {
			// 计算商品总价
			if (!parameters.OrderProductParametersList.Any()) {
				throw new BadRequestException(new T("Please select the products you want to purchase"));
			}
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
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
					throw new BadRequestException(new T("Create order contains multi currency is not supported"));
				}
				// 添加到商品总价
				var orderCount = productParameters.MatchParameters.GetOrderCount();
				if (orderCount <= 0) {
					throw new BadRequestException(new T("Order count must larger than 0"));
				}
				orderProductTotalPrice = checked(orderProductTotalPrice + productResult.Parts.Sum() * orderCount);
			}
			result.Parts.Add(new OrderPriceCalcResult.Part("ProductTotalPrice", orderProductTotalPrice));
			// 计算运费
			// 非实体商品不需要计算运费
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var sellerToLogistics = parameters.OrderParameters.GetSellerToLogistics();
			var sellerIds = parameters.OrderProductParametersList.GetSellerIdsHasRealProducts();
			var totalLogisticsCost = 0M;
			foreach (var sellerId in sellerIds) {
				var logisticsId = sellerToLogistics.GetOrDefault(sellerId);
				var logisticsCost = orderManager.CalculateLogisticsCost(logisticsId, sellerId, parameters);
				if (!string.IsNullOrEmpty(logisticsCost.Second)) {
					// 计算运费错误
					throw new BadRequestException(logisticsCost.Second);
				} else if (logisticsCost.First.First != 0 &&
					logisticsCost.First.Second != result.Currency) {
					// 货币不一致
					throw new BadRequestException(new T("Create order contains multi currency is not supported"));
				}
				totalLogisticsCost = checked(totalLogisticsCost + logisticsCost.First.First);
			}
			if (totalLogisticsCost != 0) {
				result.Parts.Add(new OrderPriceCalcResult.Part("LogisticsCost", totalLogisticsCost));
			}
			// 计算支付手续费
			ReCalculatePaymentFee(parameters, result);
		}

		/// <summary>
		/// 重新计算支付手续费
		/// 删除原有的手续费并按当前的总价重新计算
		/// 手续费不会按各个卖家分别计算
		/// - 如果使用合并交易可以在合并交易中设置整体的手续费
		/// - 例: (交易A: 手续费0.5, 交易B: 手续费0.3, 合并交易: 手续费0.6)
		/// </summary>
		public static void ReCalculatePaymentFee(
			CreateOrderParameters parameters, OrderPriceCalcResult result) {
			var oldPartIndex = result.Parts.FindIndex(p => p.Type == "PaymentFee");
			if (oldPartIndex >= 0) {
				result.Parts.RemoveAt(oldPartIndex);
			}
			var paymentApiManager = Application.Ioc.Resolve<PaymentApiManager>();
			var paymentApiId = parameters.OrderParameters.GetPaymentApiId();
			var paymentFee = paymentApiManager.CalculatePaymentFee(paymentApiId, result.Parts.Sum());
			if (paymentFee != 0) {
				result.Parts.Add(new OrderPriceCalcResult.Part("PaymentFee", paymentFee));
			}
		}
	}
}
