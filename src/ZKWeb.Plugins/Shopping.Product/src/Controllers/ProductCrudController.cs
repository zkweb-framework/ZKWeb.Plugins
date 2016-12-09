using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Entities;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Services;
using ZKWeb.Plugins.Common.GenericClass.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Common.GenericTag.src.Domain.Entities;
using ZKWeb.Plugins.Common.GenericTag.src.Domain.Services;
using ZKWeb.Plugins.Common.GenericTag.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Shopping.Product.src.Components.GenericConfigs;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ListItemProviders;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	using Product = Domain.Entities.Product;

	/// <summary>
	/// 商品管理
	/// </summary>
	[ExportMany]
	public class ProductCrudController : CrudAdminAppControllerBase<Product, Guid> {
		public override string Group { get { return "Shop Manage"; } }
		public override string GroupIconClass { get { return "fa fa-building"; } }
		public override string Name { get { return "ProductManage"; } }
		public override string Url { get { return "/admin/products"; } }
		public override string TileClass { get { return "tile bg-red"; } }
		public override string IconClass { get { return "fa fa-diamond"; } }
		protected override IAjaxTableHandler<Product, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductCrudController() {
			IncludeCss.Add("/static/shopping.product.css/product-list.css");
			IncludeCss.Add("/static/shopping.product.css/product-edit.css");
			IncludeJs.Add("/static/shopping.product.js/product-edit.min.js");
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<Product, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<ProductCrudController>();
				searchBar.StandardSetupFor<ProductCrudController>("Name/Remark");
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"ProductType", typeof(ListItemsWithOptional<ProductTypeListItemProvider>))));
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"ProductState", typeof(ListItemsWithOptional<ProductStateListItemProvider>))));
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"ProductClass", typeof(ListItemsWithOptional<GenericClassListItemProvider<ProductClassController>>))));
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"ProductTag", typeof(ListItemsWithOptional<GenericTagListItemProvider<ProductTagController>>))));
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<Product> query) {
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
				var productClass = request.Conditions.GetOrDefault<Guid?>("ProductClass");
				if (productClass != null) {
					query = query.Where(q => q.Classes.Any(c => c.Id == productClass));
				}
				// 按标签
				var productTag = request.Conditions.GetOrDefault<Guid?>("ProductTag");
				if (productTag != null) {
					query = query.Where(q => q.Tags.Any(t => t.Id == productTag));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public override void OnSort(
				AjaxTableSearchRequest request, ref IQueryable<Product> query) {
				query = query.OrderByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<Product>> pairs) {
				foreach (var pair in pairs) {
					var matchedDatas = pair.Entity.MatchedDatas.ToList();
					var seller = pair.Entity.Seller;
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.GetSummaryHtml().ToString();
					pair.Row["NameText"] = pair.Entity.Name;
					pair.Row["Price"] = matchedDatas.GetPriceString();
					pair.Row["Stock"] = matchedDatas.GetTotalStockString();
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["UpdateTime"] = pair.Entity.UpdateTime.ToClientTimeString();
					pair.Row["Type"] = new T(pair.Entity.Type);
					pair.Row["State"] = new T(pair.Entity.State);
					pair.Row["Seller"] = seller?.Username;
					pair.Row["SellerId"] = seller?.Id;
					pair.Row["DisplayOrder"] = pair.Entity.DisplayOrder;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<ProductCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddHtmlColumn("Name", "30%");
				response.Columns.AddMemberColumn("Price");
				response.Columns.AddMemberColumn("Stock");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("UpdateTime");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddMemberColumn("State");
				response.Columns.AddEditColumnFor<UserCrudController>("Seller", "SellerId");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn("150");
				var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
				if (!deleted) {
					actionColumn.AddButtonForOpenLink(new T("Preview"),
						"btn btn-xs btn-success", "fa fa-eye", "/product/view?id=<%-row.Id%>", "_blank");
				}
				actionColumn.StandardSetupFor<ProductCrudController>(request);
			}
		}

		/// <summary>
		/// 添加和编辑商品使用的表单
		/// </summary>
		public class Form : TabEntityFormBuilder<Product, Guid, Form> {
			/// <summary>
			/// 商品名称
			/// </summary>
			[Required]
			[StringLength(500, MinimumLength = 1)]
			[TextBoxField("Name", "Please enter name")]
			public string Name { get; set; }
			/// <summary>
			/// 商品类型
			/// </summary>
			[Required]
			[RadioButtonsField("ProductType", typeof(ProductTypeListItemProvider))]
			public string Type { get; set; }
			/// <summary>
			/// 商品状态
			/// </summary>
			[Required]
			[RadioButtonsField("ProductState", typeof(ProductStateListItemProvider))]
			public string State { get; set; }
			/// <summary>
			/// 显示顺序
			/// </summary>
			[Required]
			[TextBoxField("DisplayOrder", "Order from small to large")]
			public long DisplayOrder { get; set; }
			/// <summary>
			/// 商品分类
			/// </summary>
			[CheckBoxTreeField("ProductClass", typeof(GenericClassListItemTreeProvider<ProductClassController>))]
			public HashSet<Guid> ProductClass { get; set; }
			/// <summary>
			/// 商品标签
			/// </summary>
			[CheckBoxGroupField("ProductTag", typeof(GenericTagListItemProvider<ProductTagController>))]
			public HashSet<Guid> ProductTag { get; set; }
			/// <summary>
			/// 卖家
			/// </summary>
			[TextBoxField("Seller", "Seller")]
			public string Seller { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[RichTextEditor("Remark", ImageBrowserUrl = "/image_browser/product")]
			public string Remark { get; set; }
			/// <summary>
			/// 商品相册的提示信息
			/// </summary>
			[AlertHtmlField("ProductAlbumAlert", "info", Group = "ProductAlbum")]
			public string ProductAlbumAlert { get; set; }
			/// <summary>
			/// 商品相册
			/// </summary>
			[ProductAlbumUploader("ProductAlbum", Group = "ProductAlbum")]
			public ProductAlbumUploadData ProductAlbum { get; set; }
			/// <summary>
			/// 类目Id，没有时等于null
			/// </summary>
			[SearchableDropdownListField("Category",
				typeof(ListItemsWithOptional<ProductCategoryListItemProvider>),
				Group = "ProductProperties")]
			public Guid? Category { get; set; }
			/// <summary>
			/// 选中的属性值
			/// </summary>
			[ProductToProperyValuesEditor("PropertyValues", "Category", Group = "ProductProperties")]
			public IList<ProductToPropertyValueForEdit> PropertyValues { get; set; }
			/// <summary>
			/// 价格库存
			/// </summary>
			[ProductMatchedDatasEditor("MatchedDatas", "Category", Group = "ProductPriceAndStock")]
			public IList<ProductMatchedDataForEdit> MatchedDatas { get; set; }
			/// <summary>
			/// 商品介绍
			/// </summary>
			[RichTextEditor("ProductIntroduction",
				ImageBrowserUrl = "/image_browser/product", Group = "ProductIntroduction")]
			public string Introduction { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(Product bindFrom) {
				// 基本信息
				Name = bindFrom.Name;
				Type = bindFrom.Type ??
					new ProductTypeListItemProvider().GetItems().Select(i => i.Value).FirstOrDefault();
				State = bindFrom.State ??
					new ProductStateListItemProvider().GetItems().Select(i => i.Value).FirstOrDefault();
				DisplayOrder = bindFrom.DisplayOrder;
				ProductClass = new HashSet<Guid>(bindFrom.Classes.Select(c => c.Id));
				ProductTag = new HashSet<Guid>(bindFrom.Tags.Select(t => t.Id));
				Seller = bindFrom.Seller == null ? null : bindFrom.Seller.Username;
				Remark = bindFrom.Remark;
				// 商品相册
				var templateManager = Application.Ioc.Resolve<TemplateManager>();
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var albumSettings = configManager.GetData<ProductAlbumSettings>();
				ProductAlbumAlert = new T("Uploaded pictures will be scaled to {0}x{1}, " +
					"upload pictures of this size can achieve the best display effect",
					albumSettings.OriginalImageWidth, albumSettings.OriginalImageHeight);
				ProductAlbum = new ProductAlbumUploadData(bindFrom.Id);
				// 属性规格
				Category = bindFrom.Category?.Id;
				PropertyValues = bindFrom.PropertyValues.ToEditList();
				// 价格库存
				MatchedDatas = bindFrom.MatchedDatas.ToEditList();
				// 商品介绍
				Introduction = bindFrom.Introduction;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(Product saveTo) {
				// 基本信息
				var classManager = Application.Ioc.Resolve<GenericClassManager>();
				var tagManager = Application.Ioc.Resolve<GenericTagManager>();
				var userManager = Application.Ioc.Resolve<UserManager>();
				saveTo.Name = Name;
				saveTo.Type = Type;
				saveTo.State = State;
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Classes = new HashSet<GenericClass>(classManager.GetMany(c => ProductClass.Contains(c.Id)));
				saveTo.Tags = new HashSet<GenericTag>(tagManager.GetMany(t => ProductTag.Contains(t.Id)));
				saveTo.Seller = Seller == null ? null : userManager.Get(u => u.Username == Seller);
				saveTo.Remark = Remark;
				if (saveTo.Seller == null && !string.IsNullOrEmpty(Seller)) {
					throw new NotFoundException(new T("Seller username not exist"));
				}
				// 属性规格
				var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
				saveTo.Category = Category == null ? null : categoryManager.Get(Category.Value);
				saveTo.PropertyValues.Clear();
				saveTo.PropertyValues.AddRange(PropertyValues.ToDatabaseSet(saveTo));
				// 价格库存
				saveTo.MatchedDatas.Clear();
				saveTo.MatchedDatas.AddRange(MatchedDatas.ToDatabaseSet(saveTo));
				// 商品介绍
				saveTo.Introduction = Introduction;
				// 编辑后清除商品管理器的缓存
				Application.Ioc.Resolve<ProductManager>().ClearCache();
				return this.SaveSuccessAndCloseModal();
			}

			/// <summary>
			/// 保存后的处理
			/// </summary>
			protected override void OnSubmitSaved(Product saved) {
				// 商品相册
				ProductAlbum.SaveFiles(saved.Id);
			}
		}
	}
}
