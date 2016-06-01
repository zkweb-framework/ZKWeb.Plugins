using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Shopping.Product.src.Database;

namespace ZKWeb.Plugins.Shopping.Product.src.AdminApps {
	/// <summary>
	/// 商品类目管理
	/// </summary>
	[ExportMany]
	public class ProductCategoryManageApp : AdminAppBuilder<ProductCategory, ProductPropertyManageApp> {
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
				throw new NotImplementedException();
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<ProductCategory> query) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<ProductCategory> query) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<ProductCategory, Dictionary<string, object>>> pairs) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				throw new NotImplementedException();
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
