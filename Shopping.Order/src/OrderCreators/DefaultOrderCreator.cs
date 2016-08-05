using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.SerialGenerate.src.Generator;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Config;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Finance.Payment.src.Repositories;
using ZKWeb.Plugins.Shopping.Order.src.PaymentTransactionHandlers;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderCreators {
	using Product = Product.src.Database.Product;

	/// <summary>
	/// 默认的订单创建器
	/// 实现功能
	/// - 检查订单参数 (CheckOrderParameters)
	/// - 按卖家分别创建订单 (CreateOrdersBySellers)
	///   - 计算订单的价格
	///   - 添加关联的订单商品
	///   - 添加关联的订单留言
	///   - 生成订单编号
	///   - 创建订单交易
	/// - 删除相应的购物车商品 (RemoveCartProducts)
	/// - 保存收货地址的修改 (SaveShippingAddress)
	/// - 如果设置了下单时扣减库存，减少对应商品的库存 (ReduceProductsStock)
	/// - 有一个以上的订单时创建合并订单交易 (CreateMergedTransaction)
	/// </summary>
	[ExportMany]
	public class DefaultOrderCreator : IOrderCreator {
		/// <summary>
		/// 当前的创建订单的参数
		/// </summary>
		protected CreateOrderParameters Parameters { get; set; }
		/// <summary>
		/// 当前的数据库上下文
		/// </summary>
		protected DatabaseContext Context { get; set; }
		/// <summary>
		/// 已创建的订单
		/// </summary>
		protected List<Database.Order> CreatedOrders { get; set; }
		/// <summary>
		/// 已创建的交易
		/// </summary>
		protected List<PaymentTransaction> CreatedTransactions { get; set; }

		/// <summary>
		/// 检查订单参数
		/// - 商品是否存在
		/// - 库存是否足够
		/// - 是否有数量等于或小于0的商品
		/// - 如果有实体商品，必须选择物流
		/// - 如果有实体商品，必须填写收货地址
		/// - 检查是否支持非会员下单
		/// - 检查选择的物流是否允许使用
		/// - 检查选择的支付接口是否允许使用
		/// </summary>
		protected virtual void CheckOrderParameters() {
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var sellerToLogisticsId = Parameters.OrderParameters.GetSellerToLogistics();
			var shippingAddress = Parameters.OrderParameters.GetShippingAddress();
			foreach (var productParameters in Parameters.OrderProductParametersList) {
				// 检查商品是否存在
				var product = productManager.GetProduct(productParameters.ProductId);
				if (product == null) {
					throw new BadRequestException(new T("Order contains product that not exist or deleted"));
				}
				// 检查库存是否足够
				var orderCount = productParameters.MatchParameters.GetOrderCount();
				var data = product.MatchedDatas
					.Where(d => d.Stock != null)
					.WhereMatched(productParameters.MatchParameters).FirstOrDefault();
				if (data == null || data.Stock < orderCount) {
					throw new BadRequestException(string.Format(
						new T("Insufficient stock of product [{0}]"), new T(product.Name)));
				}
				// 是否有数量等于或小于0的商品
				if (orderCount <= 0) {
					throw new BadRequestException(new T("Order count must larger than 0"));
				}
				// 如果有实体商品，必须选择物流
				var typeTrait = product.GetTypeTrait();
				var sellerId = (product.Seller == null) ? 0 : product.Seller.Id;
				if (typeTrait.IsReal && sellerToLogisticsId.GetOrDefault(sellerId) <= 0) {
					throw new BadRequestException(
						new T("Order contains real products, please select a logistics"));
				}
				// 如果有实体商品，必须填写收货地址
				if (!typeTrait.IsReal) {
				} else if (string.IsNullOrEmpty(shippingAddress.DetailedAddress)) {
					throw new BadRequestException(new T("Please provide detailed address"));
				} else if (string.IsNullOrEmpty(shippingAddress.ReceiverName)) {
					throw new BadRequestException(new T("Please provide receiver name"));
				} else if (string.IsNullOrEmpty(shippingAddress.ReceiverTel)) {
					throw new BadRequestException(new T("Please provide receiver tel or mobile"));
				}
			}
			// 检查是否支持非会员下单
			var userRepository = RepositoryResolver.Resolve<User>(Context);
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<OrderSettings>();
			if (!settings.AllowAnonymousVisitorCreateOrder &&
				(Parameters.UserId == null ||
				userRepository.Count(u => u.Id == Parameters.UserId) <= 0)) {
				throw new BadRequestException(new T("To create order please login first"));
			}
			// 检查选择的物流是否允许使用
			if (sellerToLogisticsId.Any(s => !orderManager
				.GetAvailableLogistics(Parameters.UserId, (s.Key == 0) ? null : (long?)s.Key)
				.Any(l => l.Id == s.Value))) {
				throw new BadRequestException(new T("Selected logistics is not allowed to use"));
			}
			// 检查选择的支付接口是否允许使用
			var paymentApiId = Parameters.GetPaymentApiId();
			if (!orderManager.GetAvailablePaymentApis(Parameters.UserId).Any(a => a.Id == paymentApiId)) {
				throw new BadRequestException(new T("Selected payment api is not allowed to use"));
			}
		}

		/// <summary>
		/// 按卖家分别创建订单
		/// </summary>
		protected virtual void CreateOrdersBySellers() {
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var userRepository = RepositoryResolver.Resolve<User>(Context);
			var productRepository = RepositoryResolver.Resolve<Product>(Context);
			var orderRepository = RepositoryResolver.Resolve<Database.Order>(Context);
			var transactionRepository = RepositoryResolver
				.ResolveRepository<PaymentTransactionRepository>(Context);
			var products = new Dictionary<long, Product>();
			var groupedProductParameters = Parameters.OrderProductParametersList.Select(p => new {
				productParameters = p,
				product = products.GetOrCreate(p.ProductId, () => productRepository.GetById(p.ProductId))
			}).GroupBy(p => (p.product.Seller == null) ? null : (long?)p.product.Seller.Id).ToList();
			var now = DateTime.UtcNow;
			foreach (var group in groupedProductParameters) {
				// 计算订单的价格
				// 支付手续费需要单独提取出来
				var orderPrice = orderManager.CalculateOrderPrice(
					Parameters.CloneWith(group.Select(o => o.productParameters).ToList()));
				var paymentFee = orderPrice.Parts.FirstOrDefault(p => p.Type == "PaymentFee");
				orderPrice.Parts.Remove(paymentFee);
				// 生成订单
				var order = new Database.Order() {
					Buyer = userRepository.GetById(Parameters.UserId),
					BuyerSessionId = Parameters.SessionId,
					Seller = (group.Key == null) ? null : userRepository.GetById(group.Key),
					State = OrderState.WaitingBuyerPay,
					OrderParameters = Parameters.OrderParameters,
					TotalCost = orderPrice.Parts.Sum(),
					Currency = orderPrice.Currency,
					TotalCostCalcResult = orderPrice,
					OriginalTotalCostCalcResult = orderPrice,
					CreateTime = now,
					LastUpdated = now,
					StateTimes = new OrderStateTimes() { { OrderState.WaitingBuyerPay, now } }
				};
				// 添加关联的订单商品
				// 这里会重新计算单价，但应该和之前的计算结果一样
				foreach (var obj in group) {
					var unitPrice = orderManager.CalculateOrderProductUnitPrice(
						Parameters.UserId, obj.productParameters);
					var orderCount = obj.productParameters.MatchParameters.GetOrderCount();
					var properties = obj.productParameters.MatchParameters.GetProperties();
					var orderProduct = new Database.OrderProduct() {
						Order = order,
						Product = obj.product,
						MatchParameters = obj.productParameters.MatchParameters,
						Count = orderCount,
						UnitPrice = unitPrice.Parts.Sum(),
						Currency = unitPrice.Currency,
						UnitPriceCalcResult = unitPrice,
						OriginalUnitPriceCalcResult = unitPrice,
						CreateTime = now,
						LastUpdated = now
					};
					// 添加关联的属性
					foreach (var productToPropertyValue in obj.product
						.FindPropertyValuesFromPropertyParameters(properties)) {
						orderProduct.PropertyValues.Add(new Database.OrderProductToPropertyValue() {
							OrderProduct = orderProduct,
							Category = obj.product.Category,
							Property = productToPropertyValue.Property,
							PropertyValue = productToPropertyValue.PropertyValue,
							PropertyValueName = productToPropertyValue.PropertyValueName
						});
					}
					order.OrderProducts.Add(orderProduct);
				}
				// 添加关联的订单留言
				var comment = Parameters.OrderParameters.GetOrderComment();
				if (!string.IsNullOrEmpty(comment)) {
					order.OrderComments.Add(new Database.OrderComment() {
						Order = order,
						Creator = order.Buyer,
						Side = OrderCommentSide.BuyerComment,
						Content = comment,
						CreateTime = now
					});
				}
				// 生成订单编号
				order.Serial = SerialGenerator.GenerateFor(order);
				// 保存订单
				orderRepository.Save(ref order);
				CreatedOrders.Add(order);
				// 创建订单交易
				// 因为目前只能使用后台的支付接口，所以收款人应该是null
				var paymentApiId = Parameters.GetPaymentApiId();
				var transaction = transactionRepository.CreateTransaction(
					OrderTransactionHandler.ConstType, paymentApiId, order.TotalCost,
					(paymentFee == null) ? 0 : paymentFee.Delta, order.Currency,
					(order.Buyer == null) ? null : (long?)order.Buyer.Id, null, order.Id, order.Serial);
				CreatedTransactions.Add(transaction);
			}
		}

		/// <summary>
		/// 删除相应的购物车商品
		/// </summary>
		protected virtual void RemoveCartProducts() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 保存收货地址的修改
		/// </summary>
		protected virtual void SaveShippingAddress() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 如果设置了下单时扣减库存，减少对应商品的库存
		/// </summary>
		protected virtual void ReduceProductsStock() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 有一个以上的订单时创建合并订单交易
		/// </summary>
		protected virtual void CreateMergedTransaction() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 创建订单
		/// </summary>
		public virtual CreateOrderResult CreateOrder(CreateOrderParameters parameters) {
			var result = new CreateOrderResult();
			UnitOfWork.Write(context => {
				Parameters = parameters;
				Context = context;
				CreatedOrders = new List<Database.Order>();
				CreatedTransactions = new List<PaymentTransaction>();
				CheckOrderParameters();
				CreateOrdersBySellers();
				RemoveCartProducts();
				SaveShippingAddress();
				ReduceProductsStock();
				CreateMergedTransaction();
			});
			foreach (var order in CreatedOrders) {
				result.CreatedOrderIds.Add(order.Id);
			}
			foreach (var transaction in CreatedTransactions) {
				result.CreatedTransactionIds.Add(transaction.Id);
			}
			return result;
		}
	}
}
