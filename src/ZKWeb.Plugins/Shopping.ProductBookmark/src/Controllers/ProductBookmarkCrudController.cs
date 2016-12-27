using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.UserPanel.src.Controllers.Bases;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Controllers {
	using ProductBookmark = Domain.Entities.ProductBookmark;

	/// <summary>
	/// 前台用户管理收藏的商品的控制器
	/// </summary>
	[ExportMany]
	public class ProductBookmarkCrudController : CrudUserPanelControllerBase<ProductBookmark, Guid> {
		public override string Group { get { return "ProductBookmark"; } }
		public override string GroupIconClass { get { return "fa fa-bookmark"; } }
		public override string Name { get { return "ProductBookmark"; } }
		public override string Url { get { return "/user/product_bookmarks"; } }
		public override string IconClass { get { return "fa fa-star"; } }
		public override string AddUrl { get { return null; } }
		public override string EditUrl { get { return null; } }
		protected override IAjaxTableHandler<ProductBookmark, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }

		public class TableHandler : AjaxTableHandlerBase<ProductBookmark, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<ProductBookmarkCrudController>();
				searchBar.StandardSetupFor<ProductBookmarkCrudController>("Name/Seller");
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<ProductBookmark> query) {
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Product.Name.Contains(request.Keyword) ||
						q.Product.Seller.Username.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<ProductBookmark>> pairs) {
				foreach (var pair in pairs) {
					var matchedDatas = pair.Entity.Product.MatchedDatas.ToList();
					var seller = pair.Entity.Product.Seller;
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["ProductId"] = pair.Entity.Product.Id;
					pair.Row["Name"] = pair.Entity.Product.GetSummaryHtml().ToString();
					pair.Row["NameText"] = pair.Entity.Product.Name;
					pair.Row["Price"] = matchedDatas.GetPriceString();
					pair.Row["Seller"] = seller?.Username;
					pair.Row["SellerId"] = seller?.Id;
					pair.Row["BookmarkTime"] = pair.Entity.CreateTime.ToClientTimeString();
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<ProductBookmarkCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddHtmlColumn("Name", "30%");
				response.Columns.AddMemberColumn("Price");
				response.Columns.AddMemberColumn("Seller");
				response.Columns.AddMemberColumn("BookmarkTime");
				var actionColumn = response.Columns.AddActionColumn("5%");
				actionColumn.AddButtonForOpenLink(new T("View"),
					"btn btn-xs btn-success", "fa fa-eye", "/product/view?id=<%-row.ProductId%>", "_blank");
				actionColumn.StandardSetupFor<ProductBookmarkCrudController>(request);
			}
		}
	}
}
