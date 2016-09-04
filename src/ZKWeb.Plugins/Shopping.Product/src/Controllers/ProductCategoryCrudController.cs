using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ListItemProviders;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// 商品类目管理
	/// </summary>
	[ExportMany]
	public class ProductCategoryCrudController :
		CrudAdminAppControllerBase<ProductCategory, Guid> {
		public override string Group { get { return "Shop Manage"; } }
		public override string GroupIconClass { get { return "fa fa-building"; } }
		public override string Name { get { return "ProductCategoryManage"; } }
		public override string Url { get { return "/admin/product_categories"; } }
		public override string TileClass { get { return "tile bg-red"; } }
		public override string IconClass { get { return "fa fa-sitemap"; } }
		protected override IAjaxTableHandler<ProductCategory, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<ProductCategory, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<ProductCategoryCrudController>();
				searchBar.StandardSetupFor<ProductCategoryCrudController>("Name/Remark");
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<ProductCategory> query) {
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
			public override void OnSort(
				AjaxTableSearchRequest request, ref IQueryable<ProductCategory> query) {
				query = query.OrderByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<ProductCategory>> pairs) {
				foreach (var pair in pairs) {
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["SalesProperties"] = string.Join(",",
						pair.Entity.OrderedProperties().Where(p => p.IsSalesProperty).Select(p => p.Name));
					pair.Row["NonSalesProperties"] = string.Join(",",
						pair.Entity.OrderedProperties().Where(p => !p.IsSalesProperty).Select(p => p.Name));
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["UpdateTime"] = pair.Entity.UpdateTime.ToClientTimeString();
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<ProductCategoryCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Name", "15%");
				response.Columns.AddMemberColumn("SalesProperties", "15%");
				response.Columns.AddMemberColumn("NonSalesProperties", "15%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("UpdateTime");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupFor<ProductCategoryCrudController>(request);
			}
		}

		/// <summary>
		/// 添加和编辑商品类目使用的表单
		/// </summary>
		public class Form : TabEntityFormBuilder<ProductCategory, Guid, Form> {
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
			public HashSet<Guid> SalesProperties { get; set; }
			/// <summary>
			/// 非销售属性
			/// </summary>
			[CheckBoxGroupField("NonSalesProperties", typeof(ProductNonSalesPropertyListItemProvider))]
			public HashSet<Guid> NonSalesProperties { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[TextAreaField("Remark", 5, "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(ProductCategory bindFrom) {
				Name = bindFrom.Name;
				SalesProperties = new HashSet<Guid>(
					bindFrom.OrderedProperties().Where(p => p.IsSalesProperty).Select(p => p.Id));
				NonSalesProperties = new HashSet<Guid>(
					bindFrom.OrderedProperties().Where(p => !p.IsSalesProperty).Select(p => p.Id));
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(ProductCategory saveTo) {
				saveTo.Name = Name;
				var propertyManager = Application.Ioc.Resolve<ProductPropertyManager>();
				var selected = new List<Guid>();
				if (SalesProperties != null) {
					selected.AddRange(SalesProperties);
				}
				if (NonSalesProperties != null) {
					selected.AddRange(NonSalesProperties);
				}
				saveTo.Properties.Clear();
				saveTo.Properties.AddRange(propertyManager.GetMany(p => selected.Contains(p.Id)));
				saveTo.Remark = Remark;
				// 编辑后清除类目管理器的缓存
				Application.Ioc.Resolve<ProductCategoryManager>().ClearCache();
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
