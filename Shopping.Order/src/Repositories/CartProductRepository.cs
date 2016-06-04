using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Model;

namespace ZKWeb.Plugins.Shopping.Order.src.Repositories {
	using Localize;
	using Product.src.Extensions;
	using System.Web;
	using Utils.Extensions;
	using Product = Product.src.Database.Product;

	/// <summary>
	/// 购物车商品的仓储
	/// </summary>
	[ExportMany]
	public class CartProductRepository : GenericRepository<CartProduct> {
		/// <summary>
		/// 添加购物车商品
		/// 如果商品已在购物车则增加里面的数量
		/// </summary>
		/// <param name="session">会话</param>
		/// <param name="productId">商品Id</param>
		/// <param name="type">购物车商品类型</param>
		/// <param name="matchParameters">匹配参数</param>
		public virtual void AddOrIncrease(Session session,
			long productId, CartProductType type, IDictionary<string, object> matchParameters) {
			// 判断商品是否可以购买（只判断商品本身，不判断规格等匹配参数）
			var productRepository = RepositoryResolver.Resolve<Product>(Context);
			var product = productRepository.GetById(productId);
			if (product == null) {
				throw new HttpException(400, new T("The product you are try to purchase does not exist."));
			} else if (product.GetStateTrait().IsPurchasable) {
				throw new HttpException(400, new T("The product you are try to purchase does not purchasable."));
			}
			// 获取订购数量
			var orderCount = matchParameters.GetOrDefault<long>("OrderCount");
			if (orderCount <= 0) {
				throw new HttpException(400, new T("Order count must large than 0"));
			}
			// 立刻购买时删除原有的购物车商品列表
			// 加入购物车时获取现有的购物车商品列表，判断是否可以增加已有的数量
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取会话对应的购物车商品列表
		/// </summary>
		/// <param name="session">会话</param>
		/// <returns></returns>
		public virtual IQueryable<CartProduct> GetManyBySession(
			Session session, CartProductType type) {
			var user = session.GetUser();
			return (user != null) ?
				GetMany(c => c.Buyer.Id == user.Id && c.Type == type) :
				GetMany(c => c.BuyerSession == session.Id && c.Type == type);
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
