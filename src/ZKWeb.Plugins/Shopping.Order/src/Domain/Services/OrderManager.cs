using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Shopping.Logistics.src.Manager;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	using Logistics = Logistics.src.Database.Logistics;

	/// <summary>
	/// 订单管理器
	/// </summary>
	[ExportMany, SingletonReuse]
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
				throw new BadRequestException(new T("Order product unit price must not be negative"));
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
				throw new BadRequestException(new T("Order cost must larger than 0"));
			}
			return result;
		}

		/// <summary>
		/// 计算订单使用指定的物流的运费
		/// 返回 ((运费, 货币), 错误信息)
		/// </summary>
		/// <param name="logisticsId">物流Id</param>
		/// <param name="sellerId">卖家Id</param>
		/// <param name="parameters">订单创建参数</param>
		/// <returns></returns>
		public virtual Pair<Pair<decimal, string>, string> CalculateLogisticsCost(
			long logisticsId, long? sellerId, CreateOrderParameters parameters) {
			// 判断物流的所属人是否空或卖家
			var logisticsManager = Application.Ioc.Resolve<LogisticsManager>();
			var logistics = logisticsManager.GetLogistics(logisticsId);
			if (logistics == null) {
				throw new BadRequestException(new T("Please select logistics"));
			} else if (logistics.Owner != null && logistics.Owner.Id != sellerId) {
				throw new ForbiddenException(new T("Selected logistics is not available for this seller"));
			}
			// 获取收货地址中的国家和地区
			var shippingAddress = (parameters.OrderParameters
				.GetOrDefault<IDictionary<string, object>>("ShippingAddress") ??
				new Dictionary<string, object>());
			var country = shippingAddress.GetOrDefault<string>("Country");
			var regionId = shippingAddress.GetOrDefault<long?>("RegionId");
			// 获取订单商品的总重量
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var totalWeight = 0M;
			foreach (var productParameters in parameters.OrderProductParametersList) {
				var product = productManager.GetProduct(productParameters.ProductId);
				var productSellerId = (product.Seller == null) ? null : (long?)product.Seller.Id;
				if (sellerId != productSellerId) {
					// 跳过其他卖家的商品
					continue;
				} else if (product.GetTypeTrait().IsReal) {
					// 跳过非实体商品
					continue;
				}
				var orderCount = productParameters.MatchParameters.GetOrderCount();
				var data = product.MatchedDatas
					.Where(d => d.Weight != null)
					.WhereMatched(productParameters.MatchParameters).FirstOrDefault();
				if (data != null) {
					totalWeight = checked(totalWeight + data.Weight.Value * orderCount);
				}
			}
			// 使用物流管理器计算运费
			return logisticsManager.CalculateCost(logisticsId, country, regionId, totalWeight);
		}

		/// <summary>
		/// 获取创建订单可用的收货地址列表
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual IList<UserShippingAddress> GetAvailableShippingAddress(long? userId) {
			var addresses = new List<UserShippingAddress>();
			var providers = Application.Ioc.ResolveMany<IOrderShippingAddressProvider>();
			providers.ForEach(p => p.GetShippingAddresses(userId, addresses));
			return addresses;
		}

		/// <summary>
		/// 获取创建订单可用的支付接口列表
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual IList<PaymentApi> GetAvailablePaymentApis(long? userId) {
			var apis = new List<PaymentApi>();
			var providers = Application.Ioc.ResolveMany<IOrderPaymentApiProvider>();
			providers.ForEach(p => p.GetPaymentApis(userId, apis));
			return apis;
		}

		/// <summary>
		/// 获取创建订单可用的物流列表
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="sellerId">卖家Id</param>
		/// <returns></returns>
		public virtual IList<Logistics> GetAvailableLogistics(long? userId, long? sellerId) {
			var logisticsList = new List<Logistics>();
			var providers = Application.Ioc.ResolveMany<IOrderLogisticsProvider>();
			providers.ForEach(p => p.GetLogisticsList(userId, sellerId, logisticsList));
			return logisticsList;
		}

		/// <summary>
		/// 创建订单
		/// </summary>
		/// <param name="parameters">创建订单的参数</param>
		/// <returns></returns>
		public virtual CreateOrderResult CreateOrder(CreateOrderParameters parameters) {
			var orderCreator = Application.Ioc.Resolve<IOrderCreator>();
			return UnitOfWork.Write(context => orderCreator.CreateOrder(context, parameters));
		}
	}
}
