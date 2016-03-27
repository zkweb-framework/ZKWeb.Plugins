using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.ListItemProviders;
using ZKWeb.Utils.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.AdminApps;
using ZKWeb.Plugins.Shopping.Product.src.GenericClasses;
using ZKWeb.Plugins.Common.GenericTag.src.ListItemProvider;
using ZKWeb.Plugins.Shopping.Product.src.GenericTags;
using ZKWeb.Plugins.Common.GenericClass.src.ListItemProviders;

namespace ZKWeb.Plugins.Shopping.Product.src.AdminApps {
	/// <summary>
	/// 商品管理
	/// </summary>
	[ExportMany]
	public class ProductManageApp : AdminAppBuilder<Database.Product, ProductManageApp> {
		public override string Name { get { return "ProductManage"; } }
		public override string Url { get { return "/admin/products"; } }
		public override string TileClass { get { return "tile bg-green"; } }
		public override string IconClass { get { return "fa fa-diamond"; } }
		protected override IAjaxTableCallback<Database.Product> GetTableCallback() { return new TableCallback(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<Database.Product> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditActionForAdminApp<ProductManageApp>();
				table.MenuItems.AddAddActionForAdminApp<ProductManageApp>();
				searchBar.KeywordPlaceHolder = new T("Name/Remark");
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddActionForAdminApp<ProductManageApp>();
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"ProductType", typeof(ListItemsWithOptional<ProductTypeListItemProvider>))));
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"ProductState", typeof(ListItemsWithOptional<ProductStateListItemProvider>))));
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"ProductClass", typeof(ListItemsWithOptional<GenericClassListItemProvider<ProductClass>>))));
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"ProductTag", typeof(ListItemsWithOptional<GenericTagListItemProvider<ProductTag>>))));
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.Product> query) {
				// 按回收站
				query = query.FilterByRecycleBin(request);
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Name.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
				// 按类型
				var productType = request.Conditions.GetOrDefault<string>("ProductType");
				if (!string.IsNullOrEmpty(productType)) {
					query = query.Where(q => q.Type == productType);
				}
				// 按状态
				var productState = request.Conditions.GetOrDefault<string>("ProductState");
				if (!string.IsNullOrEmpty(productState)) {
					query = query.Where(q => q.State == productState);
				}
				// 按分类
				var productClass = request.Conditions.GetOrDefault<long>("ProductClass");
				if (productClass > 0) {
					query = query.Where(q => q.Classes.Any(c => c.Id == productClass));
				}
				// 按标签
				var productTag = request.Conditions.GetOrDefault<long>("ProductTag");
				if (productTag > 0) {
					query = query.Where(q => q.Tags.Any(t => t.Id == productTag));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.Product> query) {
				query = query.OrderByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<Database.Product, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					var matchedDatas = pair.Key.MatchedDatas.ToList();
					var seller = pair.Key.Seller;
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Name"] = pair.Key.GetSummaryHtml();
					pair.Value["NameText"] = pair.Key.Name;
					pair.Value["Price"] = matchedDatas.GetPriceString();
					pair.Value["Stock"] = matchedDatas.GetTotalStockString();
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
					pair.Value["LastUpdated"] = pair.Key.LastUpdated.ToClientTimeString();
					pair.Value["Type"] = new T(pair.Key.Type);
					pair.Value["State"] = new T(pair.Key.State);
					pair.Value["Seller"] = seller == null ? "" : seller.Username;
					pair.Value["SellerId"] = seller == null ? null : (long?)seller.Id;
					pair.Value["DisplayOrder"] = pair.Key.DisplayOrder;
					pair.Value["Deleted"] = pair.Key.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddNoColumn();
				response.Columns.AddHtmlColumn("Name", "30%");
				response.Columns.AddMemberColumn("Price");
				response.Columns.AddMemberColumn("Stock");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddMemberColumn("State");
				response.Columns.AddEditColumnForAdminApp<UserManageApp>("Seller", "SellerId");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddEditActionForAdminApp<ProductManageApp>();
				idColumn.AddDivider();
				idColumn.AddDeleteActionsForAdminApp<ProductManageApp>(request);
			}
		}
	}
}
