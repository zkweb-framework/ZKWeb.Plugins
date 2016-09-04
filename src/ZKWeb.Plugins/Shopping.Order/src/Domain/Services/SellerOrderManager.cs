using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderCreators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderLogisticsProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderPaymentApiProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderPriceCalculators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderProductUnitPriceCalaculators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderShippingAddressProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	using Extensions;
	using Logistics = Logistics.src.Domain.Entities.Logistics;

	/// <summary>
	/// 卖家订单管理器
	/// 包含费用计算，订单创建和卖家使用的订单操作
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SellerOrderManager : DomainServiceBase<SellerOrder, Guid> {
		/// <summary>
		/// 计算订单商品的单价
		/// 返回价格保证大于或等于0
		/// </summary>
		/// <param name="userId">用户Id，未登录时等于null</param>
		/// <param name="parameters">创建订单商品的参数</param>
		/// <returns></returns>
		public virtual OrderPriceCalcResult CalculateOrderProductUnitPrice(
			Guid? userId, CreateOrderProductParameters parameters) {
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
			Guid logisticsId, Guid? sellerId, CreateOrderParameters parameters) {
			// 判断物流的所属人是否空或卖家
			var logisticsManager = Application.Ioc.Resolve<LogisticsManager>();
			var logistics = logisticsManager.GetWithCache(logisticsId);
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
				var product = productManager.GetWithCache(productParameters.ProductId);
				var productSellerId = product.Seller?.Id;
				if (sellerId != productSellerId) {
					// 跳过其他卖家的商品
					continue;
				} else if (!(product.GetProductType() is IAmRealProduct)) {
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
		public virtual IList<ShippingAddress> GetAvailableShippingAddress(Guid? userId) {
			var addresses = new List<ShippingAddress>();
			var providers = Application.Ioc.ResolveMany<IOrderShippingAddressProvider>();
			using (UnitOfWork.Scope()) {
				providers.ForEach(p => p.GetShippingAddresses(userId, addresses));
				return addresses;
			}
		}

		/// <summary>
		/// 获取创建订单可用的支付接口列表
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual IList<PaymentApi> GetAvailablePaymentApis(Guid? userId) {
			var apis = new List<PaymentApi>();
			var providers = Application.Ioc.ResolveMany<IOrderPaymentApiProvider>();
			using (UnitOfWork.Scope()) {
				providers.ForEach(p => p.GetPaymentApis(userId, apis));
				return apis;
			}
		}

		/// <summary>
		/// 获取创建订单可用的物流列表
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="sellerId">卖家Id</param>
		/// <returns></returns>
		public virtual IList<Logistics> GetAvailableLogistics(Guid? userId, Guid? sellerId) {
			var logisticsList = new List<Logistics>();
			var providers = Application.Ioc.ResolveMany<IOrderLogisticsProvider>();
			using (UnitOfWork.Scope()) {
				providers.ForEach(p => p.GetLogisticsList(userId, sellerId, logisticsList));
				return logisticsList;
			}
		}

		/// <summary>
		/// 创建订单
		/// </summary>
		/// <param name="parameters">创建订单的参数</param>
		/// <returns></returns>
		public virtual CreateOrderResult CreateOrder(CreateOrderParameters parameters) {
			var orderCreator = Application.Ioc.Resolve<IOrderCreator>();
			var uow = UnitOfWork;
			using (uow.Scope()) {
				uow.Context.BeginTransaction();
				var result = orderCreator.CreateOrder(parameters);
				uow.Context.FinishTransaction();
				return result;
			}
		}
	}
}
