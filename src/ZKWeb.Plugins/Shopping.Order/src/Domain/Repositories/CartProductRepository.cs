using System;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Repositories;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductMatchParametersComparers.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Repositories;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Repositories {
	/// <summary>
	/// 购物车商品的仓储
	/// </summary>
	[ExportMany]
	public class CartProductRepository : RepositoryBase<CartProduct, Guid> {
		/// <summary>
		/// 获取会话对应的购物车商品列表
		/// </summary>
		/// <param name="session">会话</param>
		/// <param name="type">购物车商品类型，传入null时获取所有类型</param>
		/// <returns></returns>
		public virtual IQueryable<CartProduct> GetManyBySession(
			Session session, CartProductType? type) {
			var user = session.GetUser();
			var query = Query();
			if (user != null) {
				query = query.Where(c => c.Owner.Id == user.Id);
			} else {
				query = query.Where(c => c.OwnerSessionId == session.Id);
			}
			if (type != null) {
				query = query.Where(q => q.Type == type);
			}
			return query;
		}

		/// <summary>
		/// 添加购物车商品
		/// 如果商品已在购物车则增加里面的数量
		/// </summary>
		/// <param name="session">会话</param>
		/// <param name="productId">商品Id</param>
		/// <param name="type">购物车商品类型</param>
		/// <param name="parameters">商品匹配参数</param>
		public virtual void AddOrIncrease(Session session,
			Guid productId, CartProductType type, ProductMatchParameters parameters) {
			// 判断商品是否可以购买（只判断商品本身，不判断规格等匹配参数）
			var productRepository = Application.Ioc.Resolve<ProductRepository>();
			var product = productRepository.Get(p => p.Id == productId);
			if (product == null) {
				throw new BadRequestException(new T("The product you are try to purchase does not exist."));
			} else if (!(product.GetProductState() is IAmPurchasable)) {
				throw new BadRequestException(new T("The product you are try to purchase does not purchasable."));
			}
			// 获取订购数量
			var orderCount = parameters.GetOrderCount();
			if (orderCount <= 0) {
				throw new BadRequestException(new T("Order count must larger than 0"));
			}
			// 立刻购买时删除原有的购物车商品列表
			// 加入购物车时获取现有的购物车商品列表，判断是否可以增加已有的数量
			CartProduct increaseTo = null;
			var existCartProducts = GetManyBySession(session, type).ToList();
			if (type == CartProductType.Buynow) {
				existCartProducts.ForEach(p => Delete(p));
			} else {
				var comparers = Application.Ioc.ResolveMany<IProductMatchParametersComparer>();
				increaseTo = existCartProducts.FirstOrDefault(cartProduct => {
					// 比较商品是否相同
					if (cartProduct.Product.Id != product.Id) {
						return false;
					}
					// 比较匹配参数是否相同
					var existParameters = cartProduct.MatchParameters;
					return comparers.All(c => c.IsPartialEqual(parameters, existParameters));
				});
			}
			// 修改到数据库
			if (increaseTo != null) {
				// 修改已有的数量
				Save(ref increaseTo, p => p.UpdateOrderCount(checked(p.Count + orderCount)));
			} else {
				// 添加新的购物车商品
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var orderSettings = configManager.GetData<OrderSettings>();
				var userRepository = Application.Ioc.Resolve<UserRepository>();
				var now = DateTime.UtcNow;
				var cartProduct = new CartProduct() {
					Type = type,
					Owner = userRepository.Get(u => u.Id == session.ReleatedId),
					OwnerSessionId = (session.ReleatedId == null) ? null : (Guid?)session.Id,
					Product = product,
					Count = orderCount,
					MatchParameters = parameters,
					CreateTime = now,
					ExpireTime = now.AddDays(
						(type == CartProductType.Buynow) ?
						orderSettings.BuynowCartProductExpiresDays :
						orderSettings.NormalCartProductExpiresDays)
				};
				Save(ref cartProduct);
			}
		}

		/// <summary>
		/// 把属于会话的购物车商品整合到用户中
		/// 用于允许非会员下单时，未登录前添加的商品可以在登陆后整合到登陆后的用户
		/// </summary>
		/// <param name="sessionId">会话Id</param>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual void MergeToUser(Guid sessionId, Guid userId) {
			var cartProducts = Query().Where(c => c.OwnerSessionId == sessionId).ToList();
			var userRepository = Application.Ioc.Resolve<UserRepository>();
			var user = userRepository.Get(u => u.Id == userId);
			if (user == null) {
				throw new ArgumentException("merge cart products failed: user not exist");
			}
			var sessionMock = new Session() { Id = sessionId, ReleatedId = userId };
			foreach (var cartProduct in cartProducts) {
				AddOrIncrease(sessionMock,
					cartProduct.Product.Id, cartProduct.Type, cartProduct.MatchParameters);
				Delete(cartProduct);
			}
		}
	}
}
