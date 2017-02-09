using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchParametersDescriptionProviders.Extensions;
using ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Services;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.UIComponents.AjaxTableHandlers {
	/// <summary>
	/// 商品评价的表格处理器
	/// </summary>
	public class ProductRatingTableHandler : AjaxTableHandlerBase<Domain.Entities.ProductRating, Guid> {
		/// <summary>
		/// 过滤数据
		/// </summary>
		public override void OnQuery(
			AjaxTableSearchRequest request,
			ref IQueryable<Domain.Entities.ProductRating> query) {
			// 根据商品Id过滤
			var id = request.Conditions.GetOrDefault<Guid>("id");
			query = query.Where(r => r.Product.Id == id);
			// 不允许查询已删除的数据, 防止客户端传Deleted参数
			query = query.Where(r => !r.Deleted);
		}

		/// <summary>
		/// 选择字段
		/// </summary>
		public override void OnSelect(
			AjaxTableSearchRequest request,
			IList<EntityToTableRow<Domain.Entities.ProductRating>> pairs) {
			var ratingManager = Application.Ioc.Resolve<ProductRatingManager>();
			foreach (var pair in pairs) {
				var rankDescription = new T(pair.Entity.Rank.GetDescription());
				pair.Row["Id"] = pair.Entity.Id;
				pair.Row["Username"] = ratingManager.StripUsername(pair.Entity.Owner.Username);
				pair.Row["Rank"] = (int)pair.Entity.Rank;
				pair.Row["RankDescription"] = rankDescription;
				pair.Row["Comment"] = string.IsNullOrEmpty(pair.Entity.Comment) ?
					rankDescription : pair.Entity.Comment;
				pair.Row["MatchParametersDescription"] = pair.Entity.Product.GetMatchParametersDescription(
					pair.Entity.OrderProduct.MatchParameters);
				pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
			}
		}

		/// <summary>
		/// 构建回应
		/// </summary>
		public override void OnResponse(
			AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
			response.Columns.AddIdColumn("Id");
			response.Columns.AddMemberColumn("Username");
			response.Columns.AddMemberColumn("Rank");
			response.Columns.AddMemberColumn("RankDescription");
			response.Columns.AddMemberColumn("Comment");
			response.Columns.AddMemberColumn("CreateTime");
		}
	}
}
