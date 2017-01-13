using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.GenericRecord.src.Domain.Entities;
using ZKWeb.Plugins.Common.GenericRecord.src.Domain.Services;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderCreators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderLogisticsProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderPaymentApiProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderPriceCalculators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderProductUnitPriceCalaculators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderShippingAddressProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.PaymentTransactionHandlers;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	using Logistics = Logistics.src.Domain.Entities.Logistics;

	/// <summary>
	/// 卖家订单管理器
	/// 包含费用计算，订单创建和卖家使用的订单操作
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SellerOrderManager : DomainServiceBase<SellerOrder, Guid> {
		/// <summary>
		/// 订单关联交易的缓存，只在同一个Http上下文中有效
		/// </summary>
		private IKeyValueCache<Guid, IList<PaymentTransaction>> ReleatedTransactionsCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public SellerOrderManager() {
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			ReleatedTransactionsCache = cacheFactory.CreateCache<Guid, IList<PaymentTransaction>>(
				CacheFactoryOptions.Default.WithLifetime(CacheLifetime.PerHttpContext));
		}

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

		/// <summary>
		/// 修改订单，订单商品和订单交易的金额
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="operatorId">操作人的Id</param>
		/// <param name="parameters">修改参数</param>
		public virtual void EditCost(
			SellerOrder order, Guid? operatorId, OrderEditCostParameters parameters) {
			// 检查是否可以修改
			var result = order.Check(a => a.CanEditCost);
			if (!result.First) {
				throw new BadRequestException(result.Second);
			}
			// 修改订单总价
			var previousTotalCost = order.TotalCost;
			order.TotalCost = parameters.OrderTotalCostCalcResult.Sum();
			order.TotalCostCalcResult = new OrderPriceCalcResult() {
				Currency = order.TotalCostCalcResult.Currency,
				Parts = parameters.OrderTotalCostCalcResult
			};
			// 修改订单商品数量和价格
			var orderProducts = order.OrderProducts.ToDictionary(p => p.Id);
			if (parameters.OrderProductCountMapping.Any(p => !orderProducts.ContainsKey(p.Key)) ||
				parameters.OrderProductUnitPriceMapping.Any(p => !orderProducts.ContainsKey(p.Key))) {
				throw new BadRequestException(new T("Order product not exist"));
			} else if (parameters.OrderProductCountMapping.Any() &&
				parameters.OrderProductCountMapping.All(p => p.Value <= 0)) {
				throw new BadRequestException(new T("Can't delete all products in the order"));
			}
			var removeProducts = new HashSet<OrderProduct>();
			foreach (var orderProduct in orderProducts.Values) {
				var count = parameters.OrderProductCountMapping.GetOrDefault(
					orderProduct.Id, orderProduct.Count);
				var unitPrice = parameters.OrderProductUnitPriceMapping.GetOrDefault(
					orderProduct.Id, orderProduct.UnitPrice);
				if (count == orderProduct.Count && unitPrice == orderProduct.UnitPrice) {
					// 没有修改时跳过更新
					continue;
				}
				if (count <= 0) {
					// 数量等于0时删除订单商品
					order.OrderProducts.Remove(orderProduct);
				} else {
					// 修改数量和单价
					orderProduct.Count = count;
					orderProduct.UnitPrice = unitPrice;
					// 当前价格不等于原始价格时，添加"手动改价"项
					var parts = orderProduct.UnitPriceCalcResult.Parts
						.Where(p => p.Type != "ManuallyEditPrice").ToList();
					var difference = orderProduct.UnitPrice - parts.Sum();
					if (difference != 0) {
						parts.Add(new OrderPriceCalcResult.Part("ManuallyEditPrice", difference));
						orderProduct.UnitPriceCalcResult = new OrderPriceCalcResult() {
							Currency = orderProduct.UnitPriceCalcResult.Currency,
							Parts = parts
						};
					}
				}
			}
			// 修改订单交易金额
			var releatedTransactionIds = GetReleatedTransactions(order.Id).Select(t => t.Id).ToList();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			foreach (var entry in parameters.TransactionAmountMapping) {
				var transaction = transactionManager.Get(entry.Key);
				if (transaction == null || !releatedTransactionIds.Contains(transaction.Id)) {
					throw new BadRequestException(new T("Payment transaction not found"));
				}
				// 没有修改时跳过更新
				if (entry.Value == transaction.Amount) {
					continue;
				}
				var previousAmount = transaction.Amount;
				transactionManager.Save(ref transaction, t => t.Amount = entry.Value);
				// 添加交易记录
				transactionManager.AddDetailRecord(transaction.Id, operatorId,
					new T("Amount changed by edit order cost, previous value is {0}", previousAmount));
			}
			// 终止合并交易
			transactionManager.EnsureParentTransactionAborted(releatedTransactionIds, operatorId,
				new T("Child transactions amount changed by edit order cost, this merge transaction should be aborted"));
			// 添加订单记录
			AddDetailRecord(order.Id, operatorId,
				new T("Order total cost changed, previous value is {0}", previousTotalCost));
		}

		/// <summary>
		/// 编辑订单收货地址
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="operatorId">操作人的Id</param>
		/// <param name="address">收货地址</param>
		public virtual void EditShippingAddress(
			SellerOrder order, Guid? operatorId, ShippingAddress address) {
			var previousAddress = order.OrderParameters.GetShippingAddress();
			Repository.Save(ref order, o => {
				o.OrderParameters.SetShippingAddress(address);
				o.OrderParameters = order.OrderParameters; // 触发setter
			});
			// 添加订单记录
			AddDetailRecord(order.Id, operatorId,
				new T("Order shipping address changed, previous value is {0}",
				previousAddress.GenerateSummary()));
		}

		/// <summary>
		/// 订单明细记录类型
		/// </summary>
		public const string RecordType = "OrderDetail";

		/// <summary>
		/// 添加订单明细记录
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <param name="creatorId">创建人Id</param>
		/// <param name="content">内容</param>
		/// <param name="extraData">附加数据</param>
		public virtual void AddDetailRecord(
			Guid orderId, Guid? creatorId, string content, object extraData = null) {
			var recordManager = Application.Ioc.Resolve<GenericRecordManager>();
			recordManager.AddRecord(
				RecordType, orderId, creatorId, content, null, extraData);
		}

		/// <summary>
		/// 获取指定订单的所有明细记录
		/// </summary>
		/// <param name="orderId">交易Id</param>
		/// <returns></returns>
		public virtual IList<GenericRecord> GetDetailRecords(Guid orderId) {
			var recordManager = Application.Ioc.Resolve<GenericRecordManager>();
			return recordManager.FindRecords(RecordType, orderId);
		}

		/// <summary>
		/// 获取订单关联的交易列表
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <returns></returns>
		public virtual IList<PaymentTransaction> GetReleatedTransactions(Guid orderId) {
			return ReleatedTransactionsCache.GetOrCreate(orderId, () => {
				var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
				return transactionManager.GetMany(OrderTransactionHandler.ConstType, orderId);
			}, TimeSpan.FromSeconds(1));
		}

		/// <summary>
		/// 获取用于支付订单的Url
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <returns></returns>
		public virtual string GetCheckoutUrl(Guid orderId) {
			var transaction = GetReleatedTransactions(orderId)
				.FirstOrDefault(t => t.Check(c => c.IsPayable).First);
			if (transaction == null) {
				throw new BadRequestException(new T("No payable transaction releated to this order"));
			}
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			return transactionManager.GetResultUrl(transaction.Id);
		}

		/// <summary>
		/// 根据订单中订购的商品减少库存
		/// </summary>
		/// <param name="order">订单</param>
		public virtual void ReduceStock(SellerOrder order) {
			var productManager = Application.Ioc.Resolve<ProductManager>();
			foreach (var orderProduct in order.OrderProducts) {
				var product = orderProduct.Product;
				var data = product.MatchedDatas
					.Where(d => d.Stock != null)
					.WhereMatched(orderProduct.MatchParameters).FirstOrDefault();
				if (data != null) {
					productManager.Save(ref product, _ => data.ReduceStock(orderProduct.Count));
				}
			}
		}

		/// <summary>
		/// 处理订单已付款
		/// 处理失败时记录到订单记录
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <returns>是否处理成功</returns>
		public virtual bool ProcessOrderPaid(Guid orderId) {
			// 获取订单
			var order = Get(orderId);
			if (order == null) {
				throw new BadRequestException(new T("Order not exist"));
			}
			// 检查是否可以处理订单已付款
			var canProcessOrderPaid = order.Check(c => c.CanProcessOrderPaid);
			if (canProcessOrderPaid.First) {
				// 设置订单状态为等待发货
				Save(ref order, o => o.SetState(OrderState.WaitingSellerDeliveryGoods));
				// 添加成功的订单记录
				AddDetailRecord(orderId, null, new T("Order is paid"));
			} else {
				// 添加失败的订单记录
				AddDetailRecord(orderId, null,
					new T("Can't process order paid, reason is {0}", canProcessOrderPaid.Second));
			}
			return canProcessOrderPaid.First;
		}

		/// <summary>
		/// 处理订单全部商品已发货
		/// 处理失败时记录到订单记录
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <returns>是否处理成功</returns>
		public virtual bool ProcessAllGoodsShipped(Guid orderId) {
			// 获取订单
			var order = Get(orderId);
			if (order == null) {
				throw new BadRequestException(new T("Order not exist"));
			}
			// 判断是否可以处理订单全部商品已发货
			var canProcessAllGoodsShipped = order.Check(c => c.CanProcessAllGoodsShipped);
			if (canProcessAllGoodsShipped.First) {
				// 修改订单状态
				Save(ref order, o => o.SetState(OrderState.WaitingBuyerConfirm));
				// 添加成功的订单记录
				AddDetailRecord(orderId, null, new T("All goods under order is shipped"));
			} else {
				// 添加失败的订单记录
				AddDetailRecord(orderId, null,
					new T("Can't process order shipped, reason is {0}", canProcessAllGoodsShipped.Second));
			}
			return canProcessAllGoodsShipped.First;
		}

		/// <summary>
		/// 处理订单交易成功
		/// 处理失败时记录到订单记录
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <returns>是否处理成功</returns>
		public virtual bool ProcessSuccess(Guid orderId) {
			// 获取订单
			var order = Get(orderId);
			if (order == null) {
				throw new BadRequestException(new T("Order not exist"));
			}
			// 判断是否可以处理订单交易成功
			var canProcessSuccess = order.Check(c => c.CanProcessSuccess);
			if (canProcessSuccess.First) {
				// 修改订单状态
				Save(ref order, o => o.SetState(OrderState.OrderSuccess));
				// 添加成功的订单记录
				AddDetailRecord(orderId, null, new T("Order is successed"));
			} else {
				// 添加失败的订单记录
				AddDetailRecord(orderId, null,
					new T("Can't process order successed, reason is {0}", canProcessSuccess.Second));
			}
			return canProcessSuccess.First;
		}

		/// <summary>
		/// 取消订单
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <param name="operatorId">操作人Id</param>
		/// <param name="reason">作废理由，必填</param>
		/// <returns>是否处理成功</returns>
		public virtual bool CancelOrder(Guid orderId, Guid? operatorId, string reason) {
			// 检查原因不能为空
			if (string.IsNullOrEmpty(reason)) {
				throw new BadRequestException(new T("Reason can't be empty"));
			}
			// 获取订单
			var order = Get(orderId);
			if (order == null) {
				throw new BadRequestException(new T("Order not exist"));
			}
			// 检查是否可以取消
			var canSetCancelled = order.Check(c => c.CanSetCancelled);
			if (canSetCancelled.First) {
				// 修改订单状态
				Save(ref order, o => o.SetState(OrderState.OrderCancelled));
				// 添加成功的订单记录
				AddDetailRecord(orderId, operatorId,
					new T("Order cancelled, reason is {0}", reason));
			} else {
				// 添加失败的订单记录
				AddDetailRecord(orderId, operatorId,
					new T("Can't cancel order, reason is {0}", canSetCancelled.Second));
			}
			return canSetCancelled.First;
		}

		/// <summary>
		/// 作废订单
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <param name="operatorId">操作人Id</param>
		/// <param name="reason">作废理由，必填</param>
		/// <returns>是否处理成功</returns>
		public virtual bool SetOrderInvalid(Guid orderId, Guid? operatorId, string reason) {
			// 检查原因不能为空
			if (string.IsNullOrEmpty(reason)) {
				throw new BadRequestException(new T("Reason can't be empty"));
			}
			// 获取订单
			var order = Get(orderId);
			if (order == null) {
				throw new BadRequestException(new T("Order not exist"));
			}
			// 检查是否可以作废
			var canSetInvalid = order.Check(c => c.CanSetInvalid);
			if (canSetInvalid.First) {
				// 修改订单状态
				Save(ref order, o => o.SetState(OrderState.OrderInvalid));
				// 同时作废所有关联交易
				var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
				foreach (var transaction in GetReleatedTransactions(order.Id)) {
					transactionManager.Process(transaction.Id, null, PaymentTransactionState.Aborted);
				}
				// 添加成功的订单记录
				AddDetailRecord(orderId, operatorId,
					new T("Order become invalid, reason is {0}", reason));
			} else {
				// 添加失败的订单记录
				AddDetailRecord(orderId, operatorId,
					new T("Can't set order invalid, reason is {0}", canSetInvalid.Second));
			}
			return canSetInvalid.First;
		}

		/// <summary>
		/// 确认收货
		/// </summary>
		/// <param name="orderId">订单Id</param>
		/// <param name="operatorId">操作人Id</param>
		/// <param name="fromBuyer">是否从买家确认</param>
		/// <returns></returns>
		public virtual bool ConfirmOrder(Guid orderId, Guid? operatorId, bool fromBuyer) {
			// 获取订单
			var order = Get(orderId);
			if (order == null) {
				throw new BadRequestException(new T("Order not exist"));
			}
			// 检查是否可以确认
			var canConfirm = order.Check(c => c.CanConfirm);
			if (canConfirm.First) {
				// 添加确认的订单记录
				if (fromBuyer) {
					AddDetailRecord(orderId, operatorId,
						new T("Buyer confirm all goods shipped, order should be success"));
				} else {
					AddDetailRecord(orderId, operatorId,
						new T("Seller confirm order insetead of buyer, order should be success"));
				}
				// 处理交易成功
				ProcessSuccess(orderId);
			} else {
				// 添加失败的订单记录
				AddDetailRecord(orderId, operatorId,
					new T("Can't confirm order, reason is {0}", canConfirm.Second));
			}
			return canConfirm.First;
		}

		/// <summary>
		/// 自动取消未付款的订单
		/// </summary>
		/// <returns></returns>
		public virtual long AutoCancelOrder() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var orderSettings = configManager.GetData<OrderSettings>();
			if (orderSettings.AutoCancelOrderAfterDays <= 0) {
				// 没有设置自动取消订单的天数
				return 0;
			}
			// 获取可能需要自动取消的订单
			using (UnitOfWork.Scope()) {
				var before = DateTime.UtcNow.AddDays(-orderSettings.AutoCancelOrderAfterDays);
				var orders = GetMany(o => o.State == OrderState.WaitingBuyerPay && o.CreateTime < before);
				var count = 0L;
				foreach (var order in orders) {
					// 判断是否可以取消
					if (!order.Check(c => c.CanSetCancelled).First) {
						continue;
					}
					// 取消订单
					CancelOrder(order.Id, null,
						new T("Auto cancel order after {0} days unpaid",
						orderSettings.AutoCancelOrderAfterDays));
					count += 1;
				}
				return count;
			}
		}

		/// <summary>
		/// 自动确认收货
		/// </summary>
		/// <returns></returns>
		public virtual long AutoConfirmOrder() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var orderSettings = configManager.GetData<OrderSettings>();
			if (orderSettings.AutoConfirmOrderAfterDays <= 0) {
				// 没有设置自动确认收货的天数
				return 0;
			}
			// 获取可能需要自动确认收货的订单
			using (UnitOfWork.Scope()) {
				var before = DateTime.UtcNow.AddDays(-orderSettings.AutoConfirmOrderAfterDays);
				var orders = GetMany(o => o.State == OrderState.WaitingBuyerConfirm && o.CreateTime < before);
				var count = 0L;
				foreach (var order in orders) {
					// 判断发货时间是否在(当前-天数)之前
					if (order.StateTimes.GetOrDefault(OrderState.WaitingBuyerConfirm) >= before) {
						continue;
					}
					// 判断是否可以确认
					if (!order.Check(c => c.CanConfirm).First) {
						continue;
					}
					// 添加纪录并确认订单
					AddDetailRecord(order.Id, null,
						new T("Auto confirm order after {0} days", orderSettings.AutoConfirmOrderAfterDays));
					ConfirmOrder(order.Id, null, false);
					count += 1;
				}
				return count;
			}
		}
	}
}
