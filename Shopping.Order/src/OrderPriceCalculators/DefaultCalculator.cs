using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Utils.Extensions;

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
					throw new HttpException(400, new T("Order count must large than 0"));
				}
				orderProductTotalPrice = checked(orderProductTotalPrice + productResult.Sum() * orderCount);
			}
			// 添加商品总价到订单价格的组成部分
			result.Parts.Add(new OrderPriceCalcResult.Part("ProductTotalPrice", orderProductTotalPrice));
			// 计算运费
			// 添加运费到订单价格的组成部分
			// TODO: 添加这里的处理
		}
	}
}
