using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Repositories;

namespace ZKWeb.Plugins.Shopping.Order.src.Managers {
	/// <summary>
	/// 购物车商品管理器
	/// </summary>
	[ExportMany]
	public class CartProductManager {
		/// <summary>
		/// 添加购物车商品到当前会话
		/// 如果商品已在购物车则增加里面的数量
		/// </summary>
		/// <param name="productId">商品Id</param>
		/// <param name="type">购物车商品类型</param>
		/// <param name="parameters">匹配参数</param>
		public virtual void AddCartProduct(
			long productId, CartProductType type, IDictionary<string, object> parameters) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			UnitOfWork.WriteRepository<CartProductRepository>(
				r => r.AddOrIncrease(sessionManager.GetSession(), productId, type, parameters));
		}

		/// <summary>
		/// 获取当前会话下的购物车商品列表
		/// </summary>
		/// <param name="type">购物车商品类型</param>
		/// <returns></returns>
		public virtual IList<CartProduct> GetCartProducts(CartProductType type) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			return UnitOfWork.ReadRepository<CartProductRepository, IList<CartProduct>>(
				r => r.GetManyBySession(sessionManager.GetSession(), type).ToList());
		}
	}
}
