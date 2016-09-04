using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Config;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Forms;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Repositories;
using ZKWeb.Server;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Services {
	/// <summary>
	/// 购物车商品管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CartProductManager {
		/// <summary>
		/// 购物车商品的总数量的缓存时间
		/// 默认是3秒，可通过网站配置指定
		/// </summary>
		public TimeSpan CartProductTotalCountCacheTime { get; protected set; }
		/// <summary>
		/// 购物车商品的总数量的缓存
		/// </summary>
		public IsolatedMemoryCache<CartProductType, long?> CartProductTotalCountCache { get; protected set; }
		/// <summary>
		/// 非会员添加购物车商品时，保留会话的天数
		/// 默认是1天，可通过网站配置指定
		/// </summary>
		public TimeSpan SessionExpireDaysForNonUserPurcharse { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CartProductManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			CartProductTotalCountCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.CartProductTotalCountCacheTime, 3));
			CartProductTotalCountCache = new IsolatedMemoryCache<CartProductType, long?>("Ident");
			SessionExpireDaysForNonUserPurcharse = TimeSpan.FromDays(
				configManager.WebsiteConfig.Extra.GetOrDefault(
				ExtraConfigKeys.SessionExpireDaysForNonUserPurcharse, 1));
		}

		/// <summary>
		/// 添加购物车商品到当前会话
		/// 如果商品已在购物车则增加里面的数量
		/// </summary>
		/// <param name="productId">商品Id</param>
		/// <param name="type">购物车商品类型</param>
		/// <param name="parameters">匹配参数</param>
		public virtual void AddCartProduct(
			long productId, CartProductType type, Product.src.Model.ProductMatchParameters parameters) {
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
			UnitOfWork.WriteRepository<CartProductRepository>(
				r => r.AddOrIncrease(session, productId, type, parameters));
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
		public virtual bool DeleteCartProduct(long cartProductId) {
			// 从数据库删除，只删除当前会话下的购物车商品
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			bool result = UnitOfWork.WriteRepository<CartProductRepository, bool>(r => {
				var cartProduct = r.GetManyBySession(sessionManager.GetSession(), null)
					.FirstOrDefault(c => c.Id == cartProductId);
				if (cartProduct != null) {
					r.Delete(cartProduct);
					return true;
				}
				return false;
			});
			// 删除相关的缓存
			CartProductTotalCountCache.Clear();
			return result;
		}

		/// <summary>
		/// 更新当前会话下的购物车商品数量
		/// </summary>
		/// <param name="counts">{ 购物车商品Id: 数量 }</param>
		public virtual void UpdateCartProductCounts(IDictionary<long, long> counts) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			UnitOfWork.WriteRepository<CartProductRepository>(r => {
				var cartProducts = r.GetManyBySession(sessionManager.GetSession(), null);
				foreach (var cartProduct in cartProducts) {
					var count = counts.GetOrDefault(cartProduct.Id);
					if (count > 0) {
						var cartProductRef = cartProduct;
						r.Save(ref cartProductRef, p => p.UpdateOrderCount(count));
					}
				}
			});
		}

		/// <summary>
		/// 把属于会话的购物车商品整合到用户中
		/// 用于允许非会员下单时，未登录前添加的商品可以在登录后整合到自身的购物车中
		/// </summary>
		public virtual void MergeToUser(string sessionId, long userId) {
			UnitOfWork.WriteRepository<CartProductRepository>(r => {
				r.MergeToUser(sessionId, userId);
			});
		}

		/// <summary>
		/// 获取当前会话下的购物车商品列表
		/// 为了保证数据的实时性，这个函数不使用缓存
		/// </summary>
		/// <param name="type">购物车商品类型</param>
		/// <returns></returns>
		public virtual IList<CartProduct> GetCartProducts(CartProductType type) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			return UnitOfWork.ReadRepository<CartProductRepository, IList<CartProduct>>(r =>
				r.GetManyBySession(sessionManager.GetSession(), type).OrderBy(c => c.Id).ToList());
		}

		/// <summary>
		/// 获取购物车商品的总数量(商品 * 订购数量)
		/// 结果会按当前用户Id和购物车商品类型缓存一定时间
		/// </summary>
		/// <param name="type">购物车商品类型</param>
		/// <returns></returns>
		public virtual long GetCartProductTotalCount(CartProductType type) {
			// 从缓存获取
			var count = CartProductTotalCountCache.GetOrDefault(type);
			if (count != null) {
				return count.Value;
			}
			// 从数据库获取
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			count = UnitOfWork.ReadRepository<CartProductRepository, long>(r =>
				r.GetManyBySession(sessionManager.GetSession(), type).Sum(p => (long?)p.Count) ?? 0);
			// 保存到缓存并返回
			CartProductTotalCountCache.Put(type, count, CartProductTotalCountCacheTime);
			return count.Value;
		}

		/// <summary>
		/// 获取购物车商品的总价
		/// 返回 { 货币: 价格 }
		/// </summary>
		/// <param name="cartProducts">购物车商品列表</param>
		/// <returns></returns>
		public virtual IDictionary<string, decimal> GetCartProductTotalPrice(
			IList<CartProduct> cartProducts) {
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var userId = user == null ? null : (long?)user.Id;
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
			var anyRealProducts = displayInfos.Any(d => d.TypeTrait.IsReal);
			// 收货地址表单
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var user = sessionManager.GetSession().GetUser();
			var userId = (user == null) ? null : (long?)user.Id;
			var shippingAddressForm = new UserShippingAddressForm();
			shippingAddressForm.Bind();
			// 订单留言表单
			var commentForm = new CreateOrderCommenForm();
			commentForm.Bind();
			// 物流列表，各个卖家都有单独的列表
			// 没有实体商品的卖家不包含在列表中
			var sellers = displayInfos
				.Where(d => d.TypeTrait.IsReal)
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
			var orderManager = Application.Ioc.Resolve<OrderManager>();
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
			var availableLogistics = new Dictionary<long, IList<object>>();
			var sellerIds = parameters.OrderProductParametersList.GetSellerIdsHasRealProducts();
			foreach (var sellerId in sellerIds) {
				var sellerIdOrNull = sellerId <= 0 ? null : (long?)sellerId;
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
	}
}
