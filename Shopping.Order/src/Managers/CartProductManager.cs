using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Shopping.Order.src.Config;
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
			// 检查是否允许非会员下单
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<OrderSettings>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			if (session.GetUser() == null && !settings.AllowAnonymousVisitorCreateOrder) {
				throw new HttpException(403, new T("Create order require user logged in"));
			}
			// 调用仓储添加购物车商品
			UnitOfWork.WriteRepository<CartProductRepository>(
				r => r.AddOrIncrease(session, productId, type, parameters));
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
