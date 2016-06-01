using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.AdminApps {
	/// <summary>
	/// 商品类目管理
	/// </summary>
	[ExportMany]
	public class ProductCategoryManageApp : AdminAppBuilder<ProductCategory> {
		public override string Name { get { return "ProductCategoryManage"; } }
		public override string Url { get { return "/admin/product_categories"; } }
		public override string TileClass { get { return "tile bg-red"; } }
		public override string IconClass { get { return "fa fa-sitemap"; } }
		protected override IAjaxTableCallback<ProductCategory> GetTableCallback() { return new TableCallback(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<ProductCategory> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public void OnBuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditActionForAdminApp<ProductCategoryManageApp>();
				table.MenuItems.AddAddActionForAdminApp<ProductCategoryManageApp>();
				searchBar.KeywordPlaceHolder = new T("Name/Remark");
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddActionForAdminApp<ProductCategoryManageApp>();
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<ProductCategory> query) {
				// 按回收站
				query = query.FilterByRecycleBin(request);
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Name.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<ProductCategory> query) {
				query = query.OrderByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<ProductCategory, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Name"] = pair.Key.Name;
					pair.Value["SalesProperties"] = string.Join(",",
						pair.Key.Properties.Where(p => p.IsSalesProperty).Select(p => p.Name));
					pair.Value["NonSalesProperties"] = string.Join(",",
						pair.Key.Properties.Where(p => !p.IsSalesProperty).Select(p => p.Name));
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
					pair.Value["LastUpdated"] = pair.Key.LastUpdated.ToClientTimeString();
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
				response.Columns.AddMemberColumn("Name", "15%");
				response.Columns.AddMemberColumn("SalesProperties", "15%");
				response.Columns.AddMemberColumn("NonSalesProperties", "15%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddEditActionForAdminApp<ProductCategoryManageApp>();
				idColumn.AddDivider();
				idColumn.AddDeleteActionsForAdminApp<ProductCategoryManageApp>(request);
			}
		}

		/// <summary>
		/// 添加和编辑商品类目使用的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<ProductCategory, Form> {
			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, ProductCategory bindFrom) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, ProductCategory saveTo) {
				throw new NotImplementedException();
			}
		}
	}
}
