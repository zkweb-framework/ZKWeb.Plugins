using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using System.ComponentModel.DataAnnotations;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// 商品属性管理
	/// </summary>
	[ExportMany]
	public class ProductPropertyCrudController :
		CrudAdminAppControllerBase<ProductProperty, Guid> {
		public override string Group { get { return "Shop Manage"; } }
		public override string GroupIconClass { get { return "fa fa-building"; } }
		public override string Name { get { return "ProductPropertyManage"; } }
		public override string Url { get { return "/admin/product_properties"; } }
		public override string TileClass { get { return "tile bg-red"; } }
		public override string IconClass { get { return "fa fa-check-square-o"; } }
		protected override IAjaxTableHandler<ProductProperty, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductPropertyCrudController() {
			IncludeJs.Add("/static/shopping.product.js/product-property-edit.min.js");
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<ProductProperty, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<ProductPropertyCrudController>();
				searchBar.StandardSetupFor<ProductPropertyCrudController>("Name/Remark");
				searchBar.Conditions.Add(new FormField(new CheckBoxFieldAttribute("IsSalesProperty")));
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<ProductProperty> query) {
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Name.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
				// 按是否销售属性
				if (request.Conditions.ContainsKey("IsSalesProperty")) {
					var isSalesProperty = request.Conditions.GetOrDefault<string>("IsSalesProperty") == "on";
					query = query.Where(q => q.IsSalesProperty == isSalesProperty);
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public override void OnSort(
				AjaxTableSearchRequest request, ref IQueryable<ProductProperty> query) {
				query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.UpdateTime);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<ProductProperty>> pairs) {
				foreach (var pair in pairs) {
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["IsSalesProperty"] = pair.Entity.IsSalesProperty ? EnumBool.True : EnumBool.False;
					pair.Row["ControlType"] = new T(pair.Entity.ControlType.GetDescription());
					pair.Row["PropertyValues"] = string.Join(",",
						pair.Entity.OrderedPropertyValues().Select(p => p.Name));
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["UpdateTime"] = pair.Entity.UpdateTime.ToClientTimeString();
					pair.Row["DisplayOrder"] = pair.Entity.DisplayOrder;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<ProductPropertyCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Name", "15%");
				response.Columns.AddEnumLabelColumn("IsSalesProperty", typeof(EnumBool));
				response.Columns.AddMemberColumn("ControlType");
				response.Columns.AddMemberColumn("PropertyValues", "20%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("UpdateTime");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupFor<ProductPropertyCrudController>(request);
			}
		}

		/// <summary>
		/// 添加和编辑商品属性使用的表单
		/// </summary>
		public class Form : TabEntityFormBuilder<ProductProperty, Guid, Form> {
			/// <summary>
			/// 名称
			/// </summary>
			[Required]
			[StringLength(100, MinimumLength = 1)]
			[TextBoxField("Name", "Name")]
			public string Name { get; set; }
			/// <summary>
			/// 是否销售属性
			/// </summary>
			[CheckBoxField("IsSalesProperty")]
			public bool IsSalesProperty { get; set; }
			/// <summary>
			/// 控件类型
			/// </summary>
			[Required]
			[DropdownListField("ControlType", typeof(ListItemFromEnum<ProductPropertyControlType>))]
			public ProductPropertyControlType ControlType { get; set; }
			/// <summary>
			/// 属性值
			/// </summary>
			[ProductPropertyValuesEditor("PropertyValues", Group = "PropertyValues")]
			public IList<ProductPropertyValueForEdit> PropertyValues { get; set; }
			/// <summary>
			/// 显示顺序
			/// </summary>
			[Required]
			[TextBoxField("DisplayOrder", "Order from small to large")]
			public long DisplayOrder { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[TextAreaField("Remark", 5, "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(ProductProperty bindFrom) {
				Name = bindFrom.Name;
				IsSalesProperty = bindFrom.IsSalesProperty;
				ControlType = bindFrom.ControlType;
				PropertyValues = bindFrom.PropertyValues.ToEditList();
				DisplayOrder = bindFrom.DisplayOrder;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(ProductProperty saveTo) {
				saveTo.Name = Name;
				saveTo.IsSalesProperty = IsSalesProperty;
				saveTo.ControlType = ControlType;
				saveTo.PropertyValues = PropertyValues.ToDatabaseSet(saveTo);
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Remark = Remark;
				// 编辑后清除类目管理器的缓存
				Application.Ioc.Resolve<ProductCategoryManager>().ClearCache();
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
