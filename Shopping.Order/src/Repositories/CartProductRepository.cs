using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Shopping.Order.src.Config;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Shopping.Order.src.Repositories {
	using Product = Product.src.Database.Product;

	/// <summary>
	/// 购物车商品的仓储
	/// </summary>
	[ExportMany]
	public class CartProductRepository : GenericRepository<CartProduct> {
		/// <summary>
		/// 获取会话对应的购物车商品列表
		/// </summary>
		/// <param name="session">会话</param>
		/// <param name="type">购物车商品类型，传入null时获取所有类型</param>
		/// <returns></returns>
		public virtual IQueryable<CartProduct> GetManyBySession(
			Session session, CartProductType? type) {
			var user = session.GetUser();
			var query = (user != null) ?
				GetMany(c => c.Buyer.Id == user.Id) :
				GetMany(c => c.BuyerSession == session.Id);
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
		/// <param name="parameters">匹配参数</param>
		public virtual void AddOrIncrease(Session session,
			long productId, CartProductType type, IDictionary<string, object> parameters) {
			// 判断商品是否可以购买（只判断商品本身，不判断规格等匹配参数）
			var productRepository = RepositoryResolver.Resolve<Product>(Context);
			var product = productRepository.GetByIdWhereNotDeleted(productId);
			if (product == null) {
				throw new HttpException(400, new T("The product you are try to purchase does not exist."));
			} else if (!product.GetStateTrait().IsPurchasable) {
				throw new HttpException(400, new T("The product you are try to purchase does not purchasable."));
			}
			// 获取订购数量
			var orderCount = parameters.GetOrDefault<long>("OrderCount");
			if (orderCount <= 0) {
				throw new HttpException(400, new T("Order count must larger than 0"));
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
				var userRepository = RepositoryResolver.Resolve<User>(Context);
				var cartProduct = new CartProduct();
				Save(ref cartProduct, p => {
					p.Type = type;
					p.Buyer = userRepository.GetById(session.ReleatedId);
					p.BuyerSession = (session.ReleatedId > 0) ? null : session.Id;
					p.Product = product;
					p.Count = orderCount;
					p.MatchParameters = new Dictionary<string, object>(parameters);
					p.CreateTime = DateTime.UtcNow;
					p.ExpireTime = DateTime.UtcNow.AddDays(
						(type == CartProductType.Buynow) ?
						orderSettings.BuynowCartProductExpiresDays :
						orderSettings.NormalCartProductExpiresDays);
				});
			}
		}

		/// <summary>
		/// 把属于会话的购物车商品整合到用户中
		/// 用于允许非会员下单时，未登录前添加的商品可以在登陆后整合到登陆后的用户
		/// 目前不支持合并相同的商品
		/// </summary>
		/// <param name="sessionId">会话Id</param>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual void MergeToUser(string sessionId, long userId) {
			var cartProducts = GetMany(c => c.BuyerSession == sessionId).ToList();
			var user = Context.Get<User>(u => u.Id == userId);
			if (user == null) {
				throw new ArgumentException("merge cart products failed: user not exist");
			}
			foreach (var cartProduct in cartProducts) {
				var localCopy = cartProduct;
				Save(ref localCopy, p => { p.BuyerSession = null; p.Buyer = user; });
			}
		}
	}
}
