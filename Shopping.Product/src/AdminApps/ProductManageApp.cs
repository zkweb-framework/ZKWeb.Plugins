using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.ListItemProviders;
using ZKWebStandard.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.AdminApps;
using ZKWeb.Plugins.Shopping.Product.src.GenericClasses;
using ZKWeb.Plugins.Common.GenericTag.src.ListItemProvider;
using ZKWeb.Plugins.Shopping.Product.src.GenericTags;
using ZKWeb.Plugins.Common.GenericClass.src.ListItemProviders;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.CMS.CKEditor.src.FormFieldAttributes;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericClass.src.Database;
using ZKWeb.Plugins.Common.GenericTag.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.FormFieldAttributes;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Templating;
using ZKWeb.Plugins.Shopping.Product.src.Config;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWebStandard.Ioc;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Product.src.AdminApps {
	/// <summary>
	/// 商品管理
	/// </summary>
	[ExportMany]
	public class ProductManageApp : AdminAppBuilder<Database.Product> {
		public override string Name { get { return "ProductManage"; } }
		public override string Url { get { return "/admin/products"; } }
		public override string TileClass { get { return "tile bg-red"; } }
		public override string IconClass { get { return "fa fa-diamond"; } }
		protected override IAjaxTableCallback<Database.Product> GetTableCallback() { return new TableCallback(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductManageApp() {
			IncludeCss.Add("/static/shopping.product.css/product-list.css");
			IncludeCss.Add("/static/shopping.product.css/product-edit.css");
			IncludeJs.Add("/static/shopping.product.js/product-edit.min.js");
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<Database.Product> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupForCrudPage<ProductManageApp>();
				searchBar.StandardSetupForCrudPage<ProductManageApp>("Name/Remark");
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
				AjaxTableSearchRequest request, List<EntityToTableRow<Database.Product>> pairs) {
				foreach (var pair in pairs) {
					var matchedDatas = pair.Entity.MatchedDatas.ToList();
					var seller = pair.Entity.Seller;
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.GetSummaryHtml().ToString();
					pair.Row["NameText"] = pair.Entity.Name;
					pair.Row["Price"] = matchedDatas.GetPriceString();
					pair.Row["Stock"] = matchedDatas.GetTotalStockString();
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["LastUpdated"] = pair.Entity.LastUpdated.ToClientTimeString();
					pair.Row["Type"] = new T(pair.Entity.Type);
					pair.Row["State"] = new T(pair.Entity.State);
					pair.Row["Seller"] = seller == null ? "" : seller.Username;
					pair.Row["SellerId"] = seller == null ? null : (long?)seller.Id;
					pair.Row["DisplayOrder"] = pair.Entity.DisplayOrder;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupForCrudPage<ProductManageApp>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddHtmlColumn("Name", "30%");
				response.Columns.AddMemberColumn("Price");
				response.Columns.AddMemberColumn("Stock");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddMemberColumn("State");
				response.Columns.AddEditColumnForCrudPage<UserManageApp>("Seller", "SellerId");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn("150");
				actionColumn.AddButtonForOpenLink(new T("Preview"),
					"btn btn-xs btn-success", "fa fa-eye", "/product/view?id=<%-row.Id%>", "_blank");
				actionColumn.StandardSetupForCrudPage<ProductManageApp>(request);
			}
		}

		/// <summary>
		/// 添加和编辑商品使用的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<Database.Product, Form> {
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
			[CheckBoxTreeField("ProductClass", typeof(GenericClassListItemTreeProvider<ProductClass>))]
			public HashSet<long> ProductClass { get; set; }
			/// <summary>
			/// 商品标签
			/// </summary>
			[CheckBoxGroupField("ProductTag", typeof(GenericTagListItemProvider<ProductTag>))]
			public HashSet<long> ProductTag { get; set; }
			/// <summary>
			/// 卖家
			/// </summary>
			[TextBoxField("Seller", "Seller")]
			public string Seller { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[CKEditor("Remark", ImageBrowserUrl = "/image_browser/product")]
			public string Remark { get; set; }
			/// <summary>
			/// 商品相册的提示信息
			/// </summary>
			[HtmlField("ProductAlbumAlert", Group = "ProductAlbum")]
			public HtmlString ProductAlbumAlert { get; set; }
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
			public long? Category { get; set; }
			/// <summary>
			/// 选中的属性值
			/// </summary>
			[ProductToProperyValuesEditor("PropertyValues", "Category", Group = "ProductProperties")]
			public List<ProductToPropertyValueForEdit> PropertyValues { get; set; }
			/// <summary>
			/// 价格库存
			/// </summary>
			[ProductMatchedDatasEditor("MatchedDatas", "Category", Group = "ProductPriceAndStock")]
			public List<ProductMatchedDataForEdit> MatchedDatas { get; set; }
			/// <summary>
			/// 商品介绍
			/// </summary>
			[CKEditor("ProductIntroduction",
				ImageBrowserUrl = "/image_browser/product", Group = "ProductIntroduction")]
			public string Introduction { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, Database.Product bindFrom) {
				// 基本信息
				Name = bindFrom.Name;
				Type = bindFrom.Type ??
					new ProductTypeListItemProvider().GetItems().Select(i => i.Value).FirstOrDefault();
				State = bindFrom.State ??
					new ProductStateListItemProvider().GetItems().Select(i => i.Value).FirstOrDefault();
				DisplayOrder = bindFrom.DisplayOrder;
				ProductClass = new HashSet<long>(bindFrom.Classes.Select(c => c.Id));
				ProductTag = new HashSet<long>(bindFrom.Tags.Select(t => t.Id));
				Seller = bindFrom.Seller == null ? null : bindFrom.Seller.Username;
				Remark = bindFrom.Remark;
				// 商品相册
				var templateManager = Application.Ioc.Resolve<TemplateManager>();
				var configManager = Application.Ioc.Resolve<GenericConfigManager>();
				var albumSettings = configManager.GetData<ProductAlbumSettings>();
				ProductAlbumAlert = new HtmlString(templateManager.RenderTemplate(
					"shopping.product/tmpl.album_alert.html", new {
						width = albumSettings.OriginalImageWidth,
						height = albumSettings.OriginalImageHeight
					}));
				ProductAlbum = new ProductAlbumUploadData(bindFrom.Id);
				// 属性规格
				Category = bindFrom.Category == null ? null : (long?)bindFrom.Category.Id;
				PropertyValues = bindFrom.PropertyValues.ToEditList();
				// 价格库存
				MatchedDatas = bindFrom.MatchedDatas.ToEditList();
				// 商品介绍
				Introduction = bindFrom.Introduction;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, Database.Product saveTo) {
				// 基本信息
				if (saveTo.Id <= 0) {
					saveTo.CreateTime = DateTime.UtcNow;
				}
				saveTo.Name = Name;
				saveTo.Type = Type;
				saveTo.State = State;
				saveTo.DisplayOrder = DisplayOrder;
				var classRepository = RepositoryResolver.Resolve<GenericClass>(context);
				var tagRepository = RepositoryResolver.Resolve<GenericTag>(context);
				saveTo.Classes = new HashSet<GenericClass>(classRepository.GetMany(c => ProductClass.Contains(c.Id)));
				saveTo.Tags = new HashSet<GenericTag>(tagRepository.GetMany(t => ProductTag.Contains(t.Id)));
				saveTo.Seller = Seller == null ? null : context.Get<User>(u => u.Username == Seller);
				saveTo.Remark = Remark;
				saveTo.LastUpdated = DateTime.UtcNow;
				// 属性规格
				var categoryRepository = RepositoryResolver.Resolve<ProductCategory>(context);
				saveTo.Category = Category == null ? null : categoryRepository.GetById(Category);
				saveTo.PropertyValues.Clear();
				saveTo.PropertyValues.AddRange(PropertyValues.ToDatabaseSet(saveTo));
				// 价格库存
				saveTo.MatchedDatas.Clear();
				saveTo.MatchedDatas.AddRange(MatchedDatas.ToDatabaseSet(saveTo));
				// 商品介绍
				saveTo.Introduction = Introduction;
				// 编辑后清除商品管理器的缓存
				Application.Ioc.Resolve<ProductManager>().ClearCache();
				return new {
					message = new T("Saved Successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}

			/// <summary>
			/// 保存后的处理
			/// </summary>
			protected override void OnSubmitSaved(DatabaseContext context, Database.Product saved) {
				// 商品相册
				ProductAlbum.SaveFiles(saved.Id);
			}
		}
	}
}
