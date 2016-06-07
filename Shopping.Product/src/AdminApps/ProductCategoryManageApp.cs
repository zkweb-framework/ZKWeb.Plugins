using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.ListItemProviders;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

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
				table.StandardSetupForAdminApp<ProductCategoryManageApp>();
				searchBar.StandardSetupForAdminApp<ProductCategoryManageApp>("Name/Remark");
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
				AjaxTableSearchRequest request, List<EntityToTableRow<ProductCategory>> pairs) {
				foreach (var pair in pairs) {
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["SalesProperties"] = string.Join(",",
						pair.Entity.OrderedProperties().Where(p => p.IsSalesProperty).Select(p => p.Name));
					pair.Row["NonSalesProperties"] = string.Join(",",
						pair.Entity.OrderedProperties().Where(p => !p.IsSalesProperty).Select(p => p.Name));
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["LastUpdated"] = pair.Entity.LastUpdated.ToClientTimeString();
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupForAdminApp<ProductCategoryManageApp>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Name", "15%");
				response.Columns.AddMemberColumn("SalesProperties", "15%");
				response.Columns.AddMemberColumn("NonSalesProperties", "15%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupForAdminApp<ProductCategoryManageApp>(request);
			}
		}

		/// <summary>
		/// 添加和编辑商品类目使用的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<ProductCategory, Form> {
			/// <summary>
			/// 名称
			/// </summary>
			[Required]
			[StringLength(100, MinimumLength = 1)]
			[TextBoxField("Name", "Name")]
			public string Name { get; set; }
			/// <summary>
			/// 销售属性
			/// </summary>
			[CheckBoxGroupField("SalesProperties", typeof(ProductSalesPropertyListItemProvider))]
			public HashSet<long> SalesProperties { get; set; }
			/// <summary>
			/// 非销售属性
			/// </summary>
			[CheckBoxGroupField("NonSalesProperties", typeof(ProductNonSalesPropertyListItemProvider))]
			public HashSet<long> NonSalesProperties { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[TextAreaField("Remark", 5, "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, ProductCategory bindFrom) {
				Name = bindFrom.Name;
				SalesProperties = new HashSet<long>(
					bindFrom.OrderedProperties().Where(p => p.IsSalesProperty).Select(p => p.Id));
				NonSalesProperties = new HashSet<long>(
					bindFrom.OrderedProperties().Where(p => !p.IsSalesProperty).Select(p => p.Id));
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, ProductCategory saveTo) {
				if (saveTo.Id <= 0) {
					saveTo.CreateTime = DateTime.UtcNow;
				}
				saveTo.Name = Name;
				var propertyRepository = RepositoryResolver.Resolve<ProductProperty>(context);
				var selected = new List<long>();
				if (SalesProperties != null) {
					selected.AddRange(SalesProperties);
				}
				if (NonSalesProperties != null) {
					selected.AddRange(NonSalesProperties);
				}
				saveTo.Properties.Clear();
				saveTo.Properties.AddRange(propertyRepository.GetMany(p => selected.Contains(p.Id)));
				saveTo.Remark = Remark;
				saveTo.LastUpdated = DateTime.UtcNow;
				// 编辑后清除类目管理器的缓存
				Application.Ioc.Resolve<ProductCategoryManager>().ClearCache();
				return new {
					message = new T("Saved Successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}
		}
	}
}
