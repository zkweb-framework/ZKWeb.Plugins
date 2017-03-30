using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions;
using ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Enums;
using ZKWebStandard.Collection;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services {
	/// <summary>
	/// 商品评价管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductRatingManager : DomainServiceBase<Entities.ProductRating, Guid> {
		/// <summary>
		/// 判断是否可以评价这个订单
		/// 如果订单状态是交易成功, 并且有至少一个未评价的订单商品则可以评价这个订单
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <returns></returns>
		public virtual bool CanRateOrder(SellerOrder order) {
			if (order.State != OrderState.OrderSuccess) {
				return false;
			}
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
		/// <param name="orderId">买家订单Id</param>
		/// <returns></returns>
		public virtual IList<OrderProduct> GetUnratedOrderProducts(Guid orderId) {
			using (UnitOfWork.Scope()) {
				var orderRepository = Application.Ioc.Resolve<IRepository<BuyerOrder, Guid>>();
				var ratings = Repository.Query();
				var orderProducts = orderRepository.Query()
					.Where(o => o.Id == orderId)
					.SelectMany(o => o.SellerOrder.OrderProducts)
					.Where(p => !ratings.Any(r => r.OrderProduct.Id == p.Id))
					.ToList();
				return orderProducts;
			}
		}

		/// <summary>
		/// 获取订单中未评价的订单商品的显示信息列表
		/// </summary>
		/// <param name="orderId">买家订单Id</param>
		/// <returns></returns>
		public virtual IList<OrderProductDisplayInfo> GetUnratedOrderProductDisplayInfos(Guid orderId) {
			using (UnitOfWork.Scope()) {
				return GetUnratedOrderProducts(orderId).Select(p => p.ToDisplayInfo()).ToList();
			}
		}

		/// <summary>
		/// 从请求参数添加商品评价
		/// </summary>
		/// <param name="orderId">买家订单Id</param> 
		/// <param name="arguments">请求参数</param>
		/// <param name="files">提交文件列表</param>
		public virtual void AddRatingsFromRequestArguments(
			Guid orderId,
			IDictionary<string, IList<string>> arguments,
			IEnumerable<Pair<string, IHttpPostedFile>> files) {
			// 获取三项评分
			var descriptionMatchScore = arguments
				.GetOrDefault("DescriptionMatchScore")?[0]
				.ConvertOrDefault<int?>();
			var serviceQualityScore = arguments
				.GetOrDefault("ServiceQualityScore")?[0]
				.ConvertOrDefault<int?>();
			var deliverySpeedScore = arguments
				.GetOrDefault("DeliverySpeedScore")?[0]
				.ConvertOrDefault<int?>();
			if (descriptionMatchScore == null) {
				throw new BadRequestException(new T("Please provide description match score"));
			} else if (!Entities.ProductRating.CheckScore(descriptionMatchScore.Value)) {
				throw new BadRequestException(new T("Invalid description match score"));
			} else if (serviceQualityScore == null) {
				throw new BadRequestException(new T("Please provide service quality score"));
			} else if (!Entities.ProductRating.CheckScore(serviceQualityScore.Value)) {
				throw new BadRequestException(new T("Invalid service quality score"));
			} else if (deliverySpeedScore == null) {
				throw new BadRequestException(new T("Please provide delivery speed score"));
			} else if (!Entities.ProductRating.CheckScore(deliverySpeedScore.Value)) {
				throw new BadRequestException(new T("Invalid delivery speed score"));
			}
			// 添加各个商品的评价
			var rateCount = 0;
			var addedRatings = new List<Entities.ProductRating>();
			using (UnitOfWork.Scope()) {
				UnitOfWork.Context.BeginTransaction();
				var orderProducts = GetUnratedOrderProducts(orderId);
				foreach (var orderProduct in orderProducts) {
					if (orderProduct.Order.State != OrderState.OrderSuccess) {
						throw new BadRequestException(new T("Invalid order state for rating"));
					}
					var rateString = arguments.GetOrDefault("Rate_" + orderProduct.Id)?[0];
					if (string.IsNullOrEmpty(rateString))
						continue;
					var rate = rateString.ConvertOrDefault<ProductRatingRank>();
					var comment = arguments.GetOrDefault("Comment_" + orderProduct.Id)?[0];
					++rateCount;
					var rating = new Entities.ProductRating() {
						OrderProduct = orderProduct,
						Product = orderProduct.Product,
						Owner = orderProduct.Order.Buyer,
						Rank = rate,
						Comment = comment,
						DescriptionMatchScore = descriptionMatchScore.Value,
						ServiceQualityScore = serviceQualityScore.Value,
						DeliverySpeedScore = deliverySpeedScore.Value
					};
					Save(ref rating);
					addedRatings.Add(rating);
				}
				UnitOfWork.Context.FinishTransaction();
			}
			if (rateCount == 0) {
				throw new BadRequestException(new T("Please provide rating for atleast one product"));
			}
			// 保存上传的评价图片
			foreach (var rating in addedRatings) {
				var orderProductId = rating.OrderProduct.Id.ToString();
				var photoFiles = files
					.Where(p => p.First.Contains(orderProductId))
					.OrderBy(p => p.First).ToList();
				
			}
		}

		/// <summary>
		/// 选择一个可评价的买家订单Id
		/// 返回的Id不固定，也可能返回Guid.Empty
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual Guid SelectOneRatableBuyerOrder(Guid userId) {
			using (UnitOfWork.Scope()) {
				// 按创建时间倒序遍历，返回第一个可评价的买家订单Id
				var orderRepository = Application.Ioc.Resolve<IRepository<BuyerOrder, Guid>>();
				var buyerOrders = orderRepository.Query()
					.Where(o => o.Owner.Id == userId)
					.OrderByDescending(o => o.CreateTime);
				foreach (var buyerOrder in buyerOrders) {
					if (CanRateOrder(buyerOrder.SellerOrder)) {
						return buyerOrder.Id;
					}
				}
			}
			return Guid.Empty;
		}

		/// <summary>
		/// 处理评价人的用户名
		/// 例如 qwert => q***t
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public virtual string StripUsername(string username) {
			if (username.Length < 3) {
				return new string('*', username.Length);
			}
			return username[0] +
				new string('*', username.Length - 2) +
				username[username.Length - 1];
		}

		/// <summary>
		/// 获取商品评价的html
		/// </summary>
		/// <param name="order">卖家订单</param>
		/// <returns></returns>
		public virtual HtmlString GetProductRatingsHtml(SellerOrder order) {
			var table = new StaticTableBuilder();
			table.Columns.Add("Product");
			table.Columns.Add("Rate", "150");
			table.Columns.Add("ProductRating", "300");
			foreach (var product in order.OrderProducts) {
				var rating = Get(r => r.OrderProduct.Id == product.Id);
				table.Rows.Add(new {
					Product = product.ToDisplayInfo().GetSummaryHtml(),
					Rate = new T(rating?.Rank.GetDescription() ?? "No Rating"),
					ProductRating = rating?.Comment
				});
			}
			return (HtmlString)table.ToLiquid();
		}
	}
}
