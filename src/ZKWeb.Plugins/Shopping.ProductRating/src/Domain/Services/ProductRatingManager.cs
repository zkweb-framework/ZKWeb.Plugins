using System;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
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
		public bool CanRateOrder(SellerOrder order) {
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
	}
}
