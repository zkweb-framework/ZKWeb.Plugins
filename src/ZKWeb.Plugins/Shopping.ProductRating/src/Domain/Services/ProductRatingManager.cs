using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services {
	/// <summary>
	/// 商品评价管理器
	/// </summary>
	[ExportMany]
	public class ProductRatingManager : DomainServiceBase<Entities.ProductRating, Guid> {
		/// <summary>
		/// 判断是否可以评价这个订单
		/// 如果订单中的订单商品有至少一个未评价则可以评价这个订单
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <returns></returns>
		public virtual bool CanRateOrder(SellerOrder order) {
			var orderProductIds = order.OrderProducts.Select(p => p.Id).ToList();
			var count = Count(r => orderProductIds.Contains(r.OrderProduct.Id));
			return count < orderProductIds.Count;
		}

		/// <summary>
		/// 获取评价订单商品的Url
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <returns></returns>
		public virtual string GetRatingUrl(SellerOrder order) {
			return $"/user/orders/rate?serial={order.Serial}";
		}

		/// <summary>
		/// 获取订单中未评价的订单商品列表
		/// </summary>
		/// <param name="orderId">卖家订单Id</param>
		/// <returns></returns>
		public virtual IList<OrderProduct> GetUnratedOrderProducts(Guid orderId) {
			using (UnitOfWork.Scope()) {
				var ratedOrderProductIds = Repository.Query()
					.Where(r => r.OrderProduct.Order.Id == orderId)
					.Select(r => r.OrderProduct.Id)
					.ToList();
				var orderRepository = Application.Ioc.Resolve<IRepository<SellerOrder, Guid>>();
				var orderProducts = orderRepository.Query()
					.Where(o => o.Id == orderId)
					.SelectMany(o => o.OrderProducts)
					.Where(p => !ratedOrderProductIds.Contains(p.Id))
					.ToList();
				return orderProducts;
			}
		}

		/// <summary>
		/// 获取订单中未评价的订单商品的显示信息列表
		/// </summary>
		/// <param name="orderId">卖家订单Id</param>
		/// <returns></returns>
		public virtual IList<OrderProductDisplayInfo> GetUnratedOrderProductDisplayInfos(Guid orderId) {
			using (UnitOfWork.Scope()) {
				return GetUnratedOrderProducts(orderId).Select(p => p.ToDisplayInfo()).ToList();
			}
		}
	}
}
