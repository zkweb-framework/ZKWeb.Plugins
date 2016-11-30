using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.SerialGenerate.src.Components.SerialGenerate;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderCreators.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.PaymentTransactionHandlers;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderCreators {
	using Product = Product.src.Domain.Entities.Product;

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
	/// - 有一个以上的订单时创建合并订单交易 (CreateMergedTransaction)
	/// </summary>
	[ExportMany]
	public class DefaultOrderCreator : IOrderCreator {
		/// <summary>
		/// 当前的创建订单的参数
		/// </summary>
		protected CreateOrderParameters Parameters { get; set; }
		/// <summary>
		/// 创建订单的结果
		/// </summary>
		protected CreateOrderResult Result { get; set; }

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
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var sellerToLogisticsId = Parameters.OrderParameters.GetSellerToLogistics();
			var shippingAddress = Parameters.OrderParameters.GetShippingAddress();
			foreach (var productParameters in Parameters.OrderProductParametersList) {
				// 检查商品是否存在
				var product = productManager.GetWithCache(productParameters.ProductId);
				if (product == null) {
					throw new BadRequestException(new T("Order contains product that not exist or deleted"));
				}
				// 检查库存是否足够
				var orderCount = productParameters.MatchParameters.GetOrderCount();
				var data = product.MatchedDatas
					.Where(d => d.Stock != null)
					.WhereMatched(productParameters.MatchParameters).FirstOrDefault();
				if (data == null || data.Stock < orderCount) {
					throw new BadRequestException(
						new T("Insufficient stock of product [{0}]", new T(product.Name)));
				}
				// 是否有数量等于或小于0的商品
				if (orderCount <= 0) {
					throw new BadRequestException(new T("Order count must larger than 0"));
				}
				// 如果有实体商品，必须选择物流
				var isRealProduct = product.GetProductType() is IAmRealProduct;
				var sellerId = product.Seller?.Id ?? Guid.Empty;
				if (isRealProduct && sellerToLogisticsId.GetOrDefault(sellerId) == Guid.Empty) {
					throw new BadRequestException(
						new T("Order contains real products, please select a logistics"));
				}
				// 如果有实体商品，必须填写收货地址
				if (!isRealProduct) {
				} else if (string.IsNullOrEmpty(shippingAddress.DetailedAddress)) {
					throw new BadRequestException(new T("Please provide detailed address"));
				} else if (string.IsNullOrEmpty(shippingAddress.ReceiverName)) {
					throw new BadRequestException(new T("Please provide receiver name"));
				} else if (string.IsNullOrEmpty(shippingAddress.ReceiverTel)) {
					throw new BadRequestException(new T("Please provide receiver tel or mobile"));
				}
			}
			// 检查是否支持非会员下单
			var userManager = Application.Ioc.Resolve<UserManager>();
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<OrderSettings>();
			if (!settings.AllowAnonymousVisitorCreateOrder &&
				(Parameters.UserId == null ||
				userManager.Count(u => u.Id == Parameters.UserId) <= 0)) {
				throw new BadRequestException(new T("To create order please login first"));
			}
			// 检查选择的物流是否允许使用
			if (sellerToLogisticsId.Any(s => !orderManager
				.GetAvailableLogistics(Parameters.UserId, (s.Key == Guid.Empty) ? null : (Guid?)s.Key)
				.Any(l => l.Id == s.Value))) {
				throw new BadRequestException(new T("Selected logistics is not allowed to use"));
			}
			// 检查选择的支付接口是否允许使用
			var paymentApiId = Parameters.OrderParameters.GetPaymentApiId();
			if (!orderManager.GetAvailablePaymentApis(Parameters.UserId).Any(a => a.Id == paymentApiId)) {
				throw new BadRequestException(new T("Selected payment api is not allowed to use"));
			}
		}

		/// <summary>
		/// 按卖家分别创建订单
		/// </summary>
		protected virtual void CreateOrdersBySellers() {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var userManager = Application.Ioc.Resolve<UserManager>();
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var buyerOrderManager = Application.Ioc.Resolve<BuyerOrderManager>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var products = new Dictionary<Guid, Product>();
			var groupedProductParameters = Parameters.OrderProductParametersList.Select(p => new {
				productParameters = p,
				product = products.GetOrCreate(p.ProductId, () => productManager.Get(p.ProductId))
			}).GroupBy(p => p.product.Seller?.Id).ToList(); // { 卖家: [ (参数, 商品) ] }
			var now = DateTime.UtcNow;
			foreach (var group in groupedProductParameters) {
				// 计算订单的价格
				// 支付手续费需要单独提取出来
				var orderPrice = orderManager.CalculateOrderPrice(
					Parameters.CloneWith(group.Select(o => o.productParameters).ToList()));
				var paymentFee = orderPrice.Parts.FirstOrDefault(p => p.Type == "PaymentFee");
				orderPrice.Parts.Remove(paymentFee);
				// 生成卖家订单
				var sellerOrder = new SellerOrder() {
					Buyer = Parameters.UserId == null ? null : userManager.Get(Parameters.UserId.Value),
					Owner = (group.Key == null) ? null : userManager.Get(group.Key.Value),
					OrderParameters = Parameters.OrderParameters,
					TotalCost = orderPrice.Parts.Sum(),
					Currency = orderPrice.Currency,
					TotalCostCalcResult = orderPrice,
					OriginalTotalCostCalcResult = orderPrice
				};
				// 添加关联的订单商品
				// 这里会重新计算单价，但应该和之前的计算结果一样
				foreach (var obj in group) {
					var unitPrice = orderManager.CalculateOrderProductUnitPrice(
						Parameters.UserId, obj.productParameters);
					var orderCount = obj.productParameters.MatchParameters.GetOrderCount();
					var properties = obj.productParameters.MatchParameters.GetProperties();
					var orderProduct = new OrderProduct() {
						Id = GuidUtils.SequentialGuid(now),
						Order = sellerOrder,
						Product = obj.product,
						MatchParameters = obj.productParameters.MatchParameters,
						Count = orderCount,
						UnitPrice = unitPrice.Parts.Sum(),
						Currency = unitPrice.Currency,
						UnitPriceCalcResult = unitPrice,
						OriginalUnitPriceCalcResult = unitPrice,
						CreateTime = now,
						UpdateTime = now
					};
					// 添加关联的属性
					foreach (var productToPropertyValue in obj.product
						.FindPropertyValuesFromPropertyParameters(properties)) {
						orderProduct.PropertyValues.Add(new OrderProductToPropertyValue() {
							Id = GuidUtils.SequentialGuid(now),
							OrderProduct = orderProduct,
							Category = obj.product.Category,
							Property = productToPropertyValue.Property,
							PropertyValue = productToPropertyValue.PropertyValue,
							PropertyValueName = productToPropertyValue.PropertyValueName
						});
					}
					sellerOrder.OrderProducts.Add(orderProduct);
				}
				// 生成订单编号
				sellerOrder.Serial = SerialGenerator.GenerateFor(sellerOrder);
				// 设置初始状态，这里会触发注册的处理器
				sellerOrder.SetState(OrderState.WaitingBuyerPay);
				// 保存卖家订单
				orderManager.Save(ref sellerOrder);
				Result.CreatedSellerOrders.Add(sellerOrder);
				// 添加关联的订单留言
				var comment = Parameters.OrderParameters.GetOrderComment();
				if (!string.IsNullOrEmpty(comment)) {
					var orderCommentManager = Application.Ioc.Resolve<OrderCommentManager>();
					orderCommentManager.AddComment(sellerOrder,
						sellerOrder.Buyer?.Id, OrderCommentSide.BuyerComment, comment);
				}
				// 生成买家订单
				var buyerOrder = new BuyerOrder() {
					Owner = sellerOrder.Buyer,
					SellerOrder = sellerOrder,
					BuyerSessionId = (sellerOrder.Buyer != null) ? null : (Guid?)Parameters.SessionId
				};
				// 保存买家订单
				buyerOrderManager.Save(ref buyerOrder);
				Result.CreatedBuyerOrders.Add(buyerOrder);
				// 创建订单交易
				// 因为目前只能使用后台的支付接口，所以收款人应该是null
				var paymentApiId = Parameters.OrderParameters.GetPaymentApiId();
				var transaction = transactionManager.CreateTransaction(
					OrderTransactionHandler.ConstType,
					paymentApiId,
					sellerOrder.TotalCost,
					paymentFee?.Delta ?? 0,
					sellerOrder.Currency,
					sellerOrder.Buyer?.Id,
					null,
					sellerOrder.Id,
					new T("Order Serial: {0}", sellerOrder.Serial));
				Result.CreatedTransactions.Add(transaction);
			}
		}

		/// <summary>
		/// 删除相应的购物车商品
		/// </summary>
		protected virtual void RemoveCartProducts() {
			var cartProducts = Parameters.OrderParameters.GetCartProducts();
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			cartProductManager.BatchDeleteForever(cartProducts.Keys);
		}

		/// <summary>
		/// 保存收货地址的修改
		/// </summary>
		protected virtual void SaveShippingAddress() {
			var addressId = Parameters.OrderParameters.GetSaveShipppingAddressId();
			if (addressId == null || Parameters.UserId == null) {
				return;
			}
			// 获取原地址，获取时同时检查所属用户
			var addressManager = Application.Ioc.Resolve<ShippingAddressManager>();
			var existAddress = addressId == null ? null : addressManager.Get(addressId.Value);
			if (existAddress == null) {
				var userManager = Application.Ioc.Resolve<UserManager>();
				existAddress = new ShippingAddress() {
					Owner = userManager.Get(Parameters.UserId.Value)
				};
			}
			// 更新或创建收货地址
			var newAddress = Parameters.OrderParameters.GetShippingAddress();
			addressManager.Save(ref existAddress, address => {
				address.ReceiverName = newAddress.ReceiverName;
				address.ReceiverTel = newAddress.ReceiverTel;
				address.Country = newAddress.Country;
				address.RegionId = newAddress.RegionId;
				address.ZipCode = newAddress.ZipCode;
				address.DetailedAddress = newAddress.DetailedAddress;
				address.Summary = address.GenerateSummary();
			});
		}

		/// <summary>
		/// 有一个以上的订单时创建合并订单交易
		/// </summary>
		protected virtual void CreateMergedTransaction() {
			if (Result.CreatedTransactions.Count <= 1) {
				return;
			}
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			var transaction = transactionManager.CreateMergedTransaction(
				MergedOrderTransactionHandler.ConstType,
				Result.CreatedTransactions);
			Result.CreatedTransactions.Add(transaction);
		}

		/// <summary>
		/// 创建订单
		/// </summary>
		public virtual CreateOrderResult CreateOrder(CreateOrderParameters parameters) {
			Parameters = parameters;
			Result = new CreateOrderResult();
			CheckOrderParameters();
			CreateOrdersBySellers();
			RemoveCartProducts();
			SaveShippingAddress();
			CreateMergedTransaction();
			return Result;
		}
	}
}
