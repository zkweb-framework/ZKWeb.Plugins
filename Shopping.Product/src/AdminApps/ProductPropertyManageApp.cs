using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Localize;
using ZKWeb.Utils.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;

namespace ZKWeb.Plugins.Shopping.Product.src.AdminApps {
	/// <summary>
	/// 商品属性管理
	/// </summary>
	[ExportMany]
	public class ProductPropertyManageApp : AdminAppBuilder<ProductProperty> {
		public override string Name { get { return "ProductPropertyManage"; } }
		public override string Url { get { return "/admin/product_properties"; } }
		public override string TileClass { get { return "tile bg-red"; } }
		public override string IconClass { get { return "fa fa-check-square-o"; } }
		protected override IAjaxTableCallback<ProductProperty> GetTableCallback() { return new TableCallback(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductPropertyManageApp() {
			IncludeJs.Add("/static/shopping.product.js/product-property-edit.min.js");
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<ProductProperty> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public void OnBuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditActionForAdminApp<ProductPropertyManageApp>();
				table.MenuItems.AddAddActionForAdminApp<ProductPropertyManageApp>();
				searchBar.KeywordPlaceHolder = new T("Name/Remark");
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddActionForAdminApp<ProductPropertyManageApp>();
				searchBar.Conditions.Add(new FormField(new CheckBoxFieldAttribute("IsSalesProperty")));
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<ProductProperty> query) {
				// 按回收站
				query = query.FilterByRecycleBin(request);
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
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<ProductProperty> query) {
				query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.LastUpdated);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<ProductProperty, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Name"] = pair.Key.Name;
					pair.Value["IsSalesProperty"] = pair.Key.IsSalesProperty ? EnumBool.True : EnumBool.False;
					pair.Value["ControlType"] = new T(pair.Key.ControlType.GetDescription());
					pair.Value["PropertyValues"] = string.Join(",",
						pair.Key.OrderedPropertyValues().Select(p => p.Name));
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
					pair.Value["LastUpdated"] = pair.Key.LastUpdated.ToClientTimeString();
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
				response.Columns.AddMemberColumn("Name", "15%");
				response.Columns.AddEnumLabelColumn("IsSalesProperty", typeof(EnumBool));
				response.Columns.AddMemberColumn("ControlType");
				response.Columns.AddMemberColumn("PropertyValues", "20%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddEditActionForAdminApp<ProductPropertyManageApp>();
				idColumn.AddDivider();
				idColumn.AddDeleteActionsForAdminApp<ProductPropertyManageApp>(request);
			}
		}

		/// <summary>
		/// 添加和编辑商品属性使用的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<ProductProperty, Form> {
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
			public List<ProductPropertyValueForEdit> PropertyValues { get; set; }
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
			protected override void OnBind(DatabaseContext context, ProductProperty bindFrom) {
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
			protected override object OnSubmit(DatabaseContext context, ProductProperty saveTo) {
				if (saveTo.Id <= 0) {
					saveTo.CreateTime = DateTime.UtcNow;
				}
				saveTo.Name = Name;
				saveTo.IsSalesProperty = IsSalesProperty;
				saveTo.ControlType = ControlType;
				saveTo.PropertyValues = PropertyValues.ToDatabaseSet(saveTo);
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.LastUpdated = DateTime.UtcNow;
				saveTo.Remark = Remark;
				// 编辑后清除类目管理器的缓存
				Application.Ioc.Resolve<ProductCategoryManager>().ClearCache();
				return new {
					message = new T("Saved successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}
		}
	}
}
