using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Shopping.Order.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Repositories;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	/// <summary>
	/// 购物车商品管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CartProductManager :
		DomainServiceBase<CartProduct, Guid>, ICacheCleaner {
		/// <summary>
		/// 购物车商品的总数量的缓存时间
		/// 默认是3秒，可通过网站配置指定
		/// </summary>
		public TimeSpan CartProductTotalCountCacheTime { get; protected set; }
		/// <summary>
		/// 购物车商品的总数量的缓存
		/// </summary>
		public IKeyValueCache<CartProductType, long?> CartProductTotalCountCache { get; protected set; }
		/// <summary>
		/// 非会员添加购物车商品时，保留会话的天数
		/// 默认是1天，可通过网站配置指定
		/// </summary>
		public TimeSpan SessionExpireDaysForNonUserPurcharse { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CartProductManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			CartProductTotalCountCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				OrderExtraConfigKeys.CartProductTotalCountCacheTime, 3));
			CartProductTotalCountCache = cacheFactory.CreateCache<CartProductType, long?>(
				CacheFactoryOptions.Default.WithIsolationPolicies("Ident"));
			SessionExpireDaysForNonUserPurcharse = TimeSpan.FromDays(extra.GetOrDefault(
				OrderExtraConfigKeys.SessionExpireDaysForNonUserPurcharse, 1));
		}

		/// <summary>
		/// 添加购物车商品到当前会话
		/// 如果商品已在购物车则增加里面的数量
		/// </summary>
		/// <param name="productId">商品Id</param>
		/// <param name="type">购物车商品类型</param>
		/// <param name="parameters">匹配参数</param>
		public virtual void AddCartProduct(
			Guid productId, CartProductType type, ProductMatchParameters parameters) {
			// 检查是否允许非会员下单
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<OrderSettings>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var user = session.GetUser();
			if (user == null && !settings.AllowAnonymousVisitorCreateOrder) {
				throw new ForbiddenException(new T("Create order require user logged in"));
			}
			// 调用仓储添加购物车商品
			using (UnitOfWork.Scope()) {
				var repository = Application.Ioc.Resolve<CartProductRepository>();
				repository.AddOrIncrease(session, productId, type, parameters);
			}
			// 非会员登录时，在购物车商品添加成功后需要延长会话时间
			if (user == null) {
				session.SetExpiresAtLeast(SessionExpireDaysForNonUserPurcharse);
				sessionManager.SaveSession();
			}
			// 删除相关的缓存
			CartProductTotalCountCache.Remove(type);
		}

		/// <summary>
		/// 删除当前会话下的购物车商品
		/// </summary>
		/// <param name="cartProductId">购物车商品Id</param>
		/// <returns></returns>
		public virtual bool DeleteCartProduct(Guid cartProductId) {
			// 从数据库删除，只删除当前会话下的购物车商品
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var repository = Application.Ioc.Resolve<CartProductRepository>();
			bool result;
			using (UnitOfWork.Scope()) {
				var cartProduct = repository.GetManyBySession(sessionManager.GetSession(), null)
					.FirstOrDefault(c => c.Id == cartProductId);
				if (cartProduct != null) {
					repository.Delete(cartProduct);
					result = true;
				}
				result = false;
			}
			// 删除相关的缓存
			CartProductTotalCountCache.Remove(CartProductType.Default);
			CartProductTotalCountCache.Remove(CartProductType.Buynow);
			return result;
		}

		/// <summary>
		/// 更新当前会话下的购物车商品数量
		/// </summary>
		/// <param name="counts">{ 购物车商品Id: 数量 }</param>
		public virtual void UpdateCartProductCounts(IDictionary<Guid, long> counts) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var repository = Application.Ioc.Resolve<CartProductRepository>();
			using (UnitOfWork.Scope()) {
				var cartProducts = repository.GetManyBySession(session, null);
				foreach (var cartProduct in cartProducts) {
					var count = counts.GetOrDefault(cartProduct.Id);
					if (count > 0) {
						var cartProductRef = cartProduct;
						repository.Save(ref cartProductRef, p => p.UpdateOrderCount(count));
					}
				}
			}
		}

		/// <summary>
		/// 把属于会话的购物车商品整合到用户中
		/// 用于允许非会员下单时，未登录前添加的商品可以在登录后整合到自身的购物车中
		/// </summary>
		public virtual void MergeToUser(Guid sessionId, Guid userId) {
			var repository = Application.Ioc.Resolve<CartProductRepository>();
			using (UnitOfWork.Scope()) {
				repository.MergeToUser(sessionId, userId);
			}
		}

		/// <summary>
		/// 获取当前会话下的购物车商品列表
		/// 为了保证数据的实时性，这个函数不使用缓存
		/// </summary>
		/// <param name="type">购物车商品类型</param>
		/// <returns></returns>
		public virtual IList<CartProduct> GetCartProducts(CartProductType type) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var repository = Application.Ioc.Resolve<CartProductRepository>();
			using (UnitOfWork.Scope()) {
				return repository
					.GetManyBySession(sessionManager.GetSession(), type)
					.OrderBy(c => c.Id).ToList();
			}
		}

		/// <summary>
		/// 获取购物车商品的总数量(商品 * 订购数量)
		/// 结果会按当前用户Id和购物车商品类型缓存一定时间
		/// </summary>
		/// <param name="type">购物车商品类型</param>
		/// <returns></returns>
		public virtual long GetCartProductTotalCount(CartProductType type) {
			return CartProductTotalCountCache.GetOrCreate(type, () => {
				var sessionManager = Application.Ioc.Resolve<SessionManager>();
				var session = sessionManager.GetSession();
				var repository = Application.Ioc.Resolve<CartProductRepository>();
				using (UnitOfWork.Scope()) {
					return repository
						.GetManyBySession(sessionManager.GetSession(), type)
						.Sum(p => (long?)p.Count);
				}
			}, CartProductTotalCountCacheTime) ?? 0;
		}

		/// <summary>
		/// 获取购物车商品的总价
		/// 返回 { 货币: 价格 }
		/// </summary>
		/// <param name="cartProducts">购物车商品列表</param>
		/// <returns></returns>
		public virtual IDictionary<string, decimal> GetCartProductTotalPrice(
			IList<CartProduct> cartProducts) {
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var userId = user?.Id;
			var result = new Dictionary<string, decimal>();
			foreach (var cartProduct in cartProducts) {
				var parameters = cartProduct.ToCreateOrderProductParameters();
				var price = orderManager.CalculateOrderProductUnitPrice(userId, parameters);
				var totalPrice = result.GetOrCreate(price.Currency, () => 0);
				totalPrice = checked(totalPrice + price.Parts.Sum() * cartProduct.Count);
				result[price.Currency] = totalPrice;
			}
			return result;
		}

		/// <summary>
		/// 获取当前会话中的迷你购物车的信息
		/// 为了保证数据的实时性，这个函数不使用缓存
		/// </summary>
		/// <returns></returns>
		public virtual object GetMiniCartApiInfo() {
			var cartProducts = GetCartProducts(CartProductType.Default);
			var displayInfos = cartProducts.Select(c => c.ToOrderProductDisplayInfo()).ToList();
			var totalCount = displayInfos.Sum(d => (long?)d.Count) ?? 0;
			var totalPriceString = string.Join(", ", displayInfos
				.GroupBy(d => d.Currency.Type)
				.Select(g => g.First().Currency.Format(g.Sum(d => checked(d.UnitPrice * d.Count)))));
			return new { displayInfos, totalCount, totalPriceString };
		}

		/// <summary>
		/// 获取当前会话中的购物车的信息
		/// 为了保证数据的实时性，这个函数不使用缓存
		/// </summary>
		/// <param name="type">购物车类型</param>
		/// <returns></returns>
		public virtual object GetCartApiInfo(CartProductType type) {
			// 购物车商品显示信息
			var cartProducts = GetCartProducts(type);
			var displayInfos = cartProducts.Select(c => c.ToOrderProductDisplayInfo()).ToList();
			var anyRealProducts = displayInfos.Any(d => d.IsRealProduct);
			// 收货地址表单
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var user = sessionManager.GetSession().GetUser();
			var userId = user?.Id;
			var shippingAddressForm = new ShippingAddressForm();
			shippingAddressForm.Bind();
			// 订单留言表单
			var commentForm = new CreateOrderCommenForm();
			commentForm.Bind();
			// 物流列表，各个卖家都有单独的列表
			// 没有实体商品的卖家不包含在列表中
			var sellers = displayInfos
				.Where(d => d.IsRealProduct)
				.Select(d => new { d.SellerId, d.Seller }).GroupBy(d => d.SellerId);
			var logisticsWithSellers = sellers.Select(s => new {
				sellerId = s.Key,
				seller = s.First().Seller,
				logisticsList = orderManager.GetAvailableLogistics(userId, s.Key)
					.Select(l => l.ToLiquid()).ToList()
			}).ToList();
			// 支付接口列表，各个卖家使用同一个列表
			// 卖家不应该提供单独的支付接口，应该使用结算处理
			var paymentApis = orderManager.GetAvailablePaymentApis(userId)
				.Select(a => a.ToLiquid()).ToList();
			// 订单创建表单
			var createOrderForm = new CreateOrderForm();
			createOrderForm.Bind();
			return new {
				displayInfos, anyRealProducts,
				shippingAddressForm, commentForm,
				logisticsWithSellers, paymentApis, createOrderForm,
			};
		}

		/// <summary>
		/// 获取购物车商品计算价格的信息
		/// 为了保证数据的实时性，这个函数不使用缓存
		/// </summary>
		/// <param name="parameters">订单创建参数</param>
		/// <returns></returns>
		public virtual object GetCartCalculatePriceApiInfo(CreateOrderParameters parameters) {
			// 计算各个商品的单价（数量等改变后有可能会改变单价）
			var orderProductUnitPrices = new List<object>();
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			foreach (var productParameters in parameters.OrderProductParametersList) {
				var productResult = orderManager.CalculateOrderProductUnitPrice(
					parameters.UserId, productParameters);
				var currency = currencyManager.GetCurrency(productResult.Currency);
				var priceString = currency.Format(productResult.Parts.Sum());
				var description = productResult.Parts.GetDescription();
				orderProductUnitPrices.Add(new {
					priceString, description, extra = productParameters.Extra
				});
			}
			// 计算订单总价
			var orderResult = orderManager.CalculateOrderPrice(parameters);
			var orderCurrency = currencyManager.GetCurrency(orderResult.Currency);
			var orderPriceString = orderCurrency.Format(orderResult.Parts.Sum());
			var orderPriceDescription = orderResult.Parts.GetDescription();
			// 获取商品总价
			var orderProductTotalPricePart = orderResult.Parts
				.FirstOrDefault(p => p.Type == "ProductTotalPrice");
			var orderProductTotalPriceString = orderCurrency.Format(
				orderProductTotalPricePart == null ? 0 : orderProductTotalPricePart.Delta);
			// 计算各个物流的运费
			// 同时选择可用的物流（部分物流不能送到选择的地区）
			var availableLogistics = new Dictionary<Guid, IList<object>>();
			var sellerIds = parameters.OrderProductParametersList.GetSellerIdsHasRealProducts();
			foreach (var sellerId in sellerIds) {
				var sellerIdOrNull = sellerId == Guid.Empty ? null : (Guid?)sellerId;
				var logisticsList = orderManager
					.GetAvailableLogistics(parameters.UserId, sellerIdOrNull)
					.Select(l => {
						var result = orderManager.CalculateLogisticsCost(l.Id, sellerIdOrNull, parameters);
						if (!string.IsNullOrEmpty(result.Second)) {
							return (object)null;
						}
						var currency = currencyManager.GetCurrency(result.First.Second);
						return new { logisticsId = l.Id, costString = currency.Format(result.First.First) };
					})
					.Where(l => l != null).ToList();
				availableLogistics[sellerId] = logisticsList;
			}
			// 计算各个支付接口的手续费
			// 同时选择可用的支付接口
			var paymentApiManager = Application.Ioc.Resolve<PaymentApiManager>();
			var orderPriceWithoutPaymentFee = orderResult.Parts
				.Where(p => p.Type != "PaymentFee").ToList().Sum();
			var availablePaymentApis = orderManager
				.GetAvailablePaymentApis(parameters.UserId)
				.Select(a => {
					var paymentFee = paymentApiManager.CalculatePaymentFee(
						parameters.OrderParameters.GetPaymentApiId(), orderPriceWithoutPaymentFee);
					return new { apiId = a.Id, feeString = orderCurrency.Format(paymentFee) };
				})
				.Where(a => a != null).ToList<object>();
			return new {
				orderPriceString, orderPriceDescription,
				orderProductTotalPriceString, orderProductUnitPrices,
				availableLogistics, availablePaymentApis
			};
		}

		/// <summary>
		/// 删除过期的购物车商品
		/// 返回删除的数量
		/// </summary>
		/// <returns></returns>
		public long ClearExpiredCartProducts() {
			var now = DateTime.UtcNow;
			using (UnitOfWork.Scope()) {
				return Repository.BatchDelete(p => p.ExpireTime < now);
			}
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			CartProductTotalCountCache.Clear();
		}
	}
}
