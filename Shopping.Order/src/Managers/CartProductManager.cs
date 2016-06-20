using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Config;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Repositories;
using ZKWeb.Server;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Shopping.Order.src.Managers {
	using Logistics = Logistics.src.Database.Logistics;

	/// <summary>
	/// 购物车商品管理器
	/// </summary>
	[ExportMany]
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
			long productId, CartProductType type, IDictionary<string, object> parameters) {
			// 检查是否允许非会员下单
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<OrderSettings>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var user = session.GetUser();
			if (user == null && !settings.AllowAnonymousVisitorCreateOrder) {
				throw new HttpException(403, new T("Create order require user logged in"));
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
		/// <param name="cartProductId"></param>
		/// <returns></returns>
		public virtual bool DeleteCartProduct(long cartProductId) {
			// 从数据库删除，只删除当前会话下的购物车商品
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			bool result = UnitOfWork.WriteRepository<CartProductRepository, bool>(r => {
				var cartProduct = r.GetManyBySession(
					sessionManager.GetSession(), CartProductType.Default)
					.FirstOrDefault(c => c.Id == cartProductId);
				if (cartProduct != null) {
					r.Delete(cartProduct);
					return true;
				}
				return false;
			});
			// 删除相关的缓存
			CartProductTotalCountCache.Remove(CartProductType.Default);
			return result;
		}

		/// <summary>
		/// 获取当前会话下的购物车商品列表
		/// 为了保证数据的实时性，这个函数不使用缓存
		/// </summary>
		/// <param name="type">购物车商品类型</param>
		/// <returns></returns>
		public virtual IList<CartProduct> GetCartProducts(CartProductType type) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			return UnitOfWork.ReadRepository<CartProductRepository, IList<CartProduct>>(
				r => r.GetManyBySession(sessionManager.GetSession(), type).ToList());
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
			// 收货地址列表
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var orderManager = Application.Ioc.Resolve<OrderManager>();
			var user = sessionManager.GetSession().GetUser();
			var userId = (user == null) ? null : (long?)user.Id;
			var shippingAddresses = orderManager.GetAvailableShippingAddress(userId);
			// 物流列表，各个卖家都有单独的列表
			var sellerIds = displayInfos.Select(d => d.SellerId).Distinct().ToList();
			var logisticsWithSellers = sellerIds.Select(sellerId => new {
				sellerId,
				logisticsList = orderManager.GetAvailableLogistics(userId, sellerId)
			}).ToList();
			// 支付接口列表，各个卖家使用同一个列表
			// 卖家不应该提供单独的支付接口，应该使用结算处理
			var paymentApis = orderManager.GetAvailablePaymentApis(userId);
			return new { displayInfos, shippingAddresses, logisticsWithSellers, paymentApis };
		}
	}
}
