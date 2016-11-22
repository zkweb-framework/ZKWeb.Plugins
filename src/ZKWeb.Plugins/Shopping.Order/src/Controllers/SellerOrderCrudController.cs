using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Controllers;
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
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.HtmlItems.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 后台的订单管理的控制器
	/// 只在后台使用，需要卖家使用的订单管理请安装多商城插件
	/// </summary>
	[ExportMany]
	public class SellerOrderCrudController : CrudAdminAppControllerBase<SellerOrder, Guid> {
		public override string Group { get { return "Shop Manage"; } }
		public override string GroupIconClass { get { return "fa fa-building"; } }
		public override string Name { get { return "OrderManage"; } }
		public override string Url { get { return "/admin/orders"; } }
		public override string TileClass { get { return "tile bg-aqua"; } }
		public override string IconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string AddUrl { get { return null; } }
		public override bool AllowDeleteForever { get { return false; } }
		protected override bool UseTransaction { get { return true; } }
		protected override IAjaxTableHandler<SellerOrder, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public SellerOrderCrudController() {
			IncludeCss.Add("/static/shopping.order.css/progress-wizard.css");
			IncludeCss.Add("/static/shopping.order.css/order-list.css");
			IncludeCss.Add("/static/shopping.order.css/order-view.css");
			IncludeJs.Add("/static/shopping.order.js/order-list.min.js");
			IncludeJs.Add("/static/shopping.order.js/order-view.min.js");
		}

		/// <summary>
		/// 编辑价格
		/// </summary>
		public IActionResult EditCost() {
			var form = new OrderEditCostForm(OrderOperatorType.Admin);
			if (Request.Method == HttpMethods.GET) {
				form.Bind();
				return new TemplateResult("shopping.order/order_edit_cost.html", new { form });
			} else {
				return new JsonResult(form.Submit());
			}
		}

		/// <summary>
		/// 编辑地址
		/// </summary>
		public IActionResult EditShippingAddress() {
			var form = new OrderEditShippingAddressForm();
			if (Request.Method == HttpMethods.GET) {
				form.Bind();
				return new TemplateResult("shopping.order/order_edit_shipping_address.html", new { form });
			} else {
				return new JsonResult(form.Submit());
			}
		}

		/// <summary>
		/// 网站启动时添加处理函数
		/// </summary>
		public override void OnWebsiteStart() {
			base.OnWebsiteStart();
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			// 编辑价格
			var editCostUrl = Url + "/edit_cost";
			controllerManager.RegisterAction(
				editCostUrl, HttpMethods.GET, WrapAction(EditCost, EditPrivileges));
			controllerManager.RegisterAction(
				editCostUrl, HttpMethods.POST, WrapAction(EditCost, EditPrivileges));
			// 编辑地址
			var editShippingAddressUrl = Url + "/edit_shipping_address";
			controllerManager.RegisterAction(
				editShippingAddressUrl, HttpMethods.GET, WrapAction(EditShippingAddress, EditPrivileges));
			controllerManager.RegisterAction(
				editShippingAddressUrl, HttpMethods.POST, WrapAction(EditShippingAddress, EditPrivileges));
		}

		/// <summary>
		/// 表格处理器
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<SellerOrder, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<SellerOrderCrudController>();
				table.Template = "/static/shopping.order.tmpl/orderTable.tmpl";
				searchBar.StandardSetupFor<SellerOrderCrudController>("Serial/Remark");
				searchBar.BeforeItems.AddOrderFilterBar();
				searchBar.Conditions.Add(new FormField(new TextBoxFieldAttribute("Buyer")));
				searchBar.Conditions.Add(new FormField(new TextBoxFieldAttribute("Seller")));
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"RemarkFlags", typeof(ListItemsWithOptional<ListItemFromEnum<OrderRemarkFlags>>))));
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<SellerOrder> query) {
				// 按状态
				var state = request.Conditions.GetOrDefault<OrderState?>("State");
				if (state != null) {
					query = query.Where(o => o.State == state);
				}
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Serial.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
				// 按买家
				var buyer = request.Conditions.GetOrDefault<string>("Buyer");
				if (!string.IsNullOrEmpty(buyer)) {
					query = query.Where(q => q.Buyer.Username == buyer);
				}
				// 按卖家
				var seller = request.Conditions.GetOrDefault<string>("Seller");
				if (!string.IsNullOrEmpty(seller)) {
					query = query.Where(q => q.Owner.Username == seller);
				}
				// 按备注旗帜
				var flags = request.Conditions.GetOrDefault<OrderRemarkFlags?>("RemarkFlags");
				if (flags != null) {
					query = query.Where(q => q.RemarkFlags == flags);
				}
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<SellerOrder>> pairs) {
				foreach (var pair in pairs) {
					var displayInfo = pair.Entity.ToDisplayInfo(OrderOperatorType.Admin);
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Serial"] = pair.Entity.Serial;
					pair.Row["HeadingHtml"] = displayInfo.GetTableHeadingHtml().ToString();
					pair.Row["OrderProducts"] = displayInfo.OrderProducts.GetSummryListHtml().ToString();
					pair.Row["Price"] = displayInfo.OrderProducts.GetPriceListHtml().ToString();
					pair.Row["Quantity"] = displayInfo.OrderProducts.GetOrderCountListHtml().ToString();
					pair.Row["ShippedQuantity"] = displayInfo.OrderProducts.GetShippedCountListHtml().ToString();
					pair.Row["TotalCost"] = displayInfo.GetTotalCostWithPartsHtml().ToString();
					pair.Row["State"] = pair.Entity.State;
					pair.Row["Buyer"] = displayInfo.Buyer;
					pair.Row["BuyerId"] = displayInfo.BuyerId;
					pair.Row["Seller"] = displayInfo.Seller;
					pair.Row["SellerId"] = displayInfo.SellerId;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
					pair.Row["OrderActions"] = displayInfo.GetOrderActionsTableCellHtml().ToString();
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<SellerOrderCrudController>(request);
				response.Columns.AddHtmlColumn("OrderProducts", "30%");
				response.Columns.AddHtmlColumn("Price", "70");
				response.Columns.AddHtmlColumn("Quantity", "70");
				response.Columns.AddHtmlColumn("ShippedQuantity", "70");
				response.Columns.AddHtmlColumn("TotalCost", "70");
				response.Columns.AddEnumLabelColumn("State", typeof(OrderState), "50");
				response.Columns.AddEditColumnFor<UserCrudController>("Buyer", "BuyerId", width: "70");
				response.Columns.AddEditColumnFor<UserCrudController>("Seller", "SellerId", width: "70");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn("5%");
				var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
				if (!deleted) {
					actionColumn.AddEditActionFor<SellerOrderCrudController>();
				}
				actionColumn.AddHtmlAction("OrderActions");
			}
		}

		/// <summary>
		/// 订单表单
		/// </summary>
		public class Form : TabEntityFormBuilder<SellerOrder, Guid, Form> {
			/// <summary>
			/// 基本信息的Html
			/// </summary>
			[HtmlField("BaseInformationHtml")]
			public HtmlString BaseInformationHtml { get; set; }
			/// <summary>
			/// 发货记录的Html
			/// </summary>
			[HtmlField("DeliveryRecordsHtml", Group = "DeliveryRecords")]
			public HtmlString DeliveryRecordsHtml { get; set; }
			/// <summary>
			/// 订单记录的Html
			/// </summary>
			[HtmlField("OrderRecordsHtml", Group = "OrderRecords")]
			public HtmlString OrderRecordsHtml { get; set; }
			/// <summary>
			/// 关联交易的Html
			/// </summary>
			[HtmlField("ReleatedTransactionsHtml", Group = "ReleatedTransactions")]
			public HtmlString ReleatedTransactionsHtml { get; set; }
			/// <summary>
			/// 订单留言的Html
			/// </summary>
			[HtmlField("OrderCommentsHtml", Group = "OrderComments")]
			public HtmlString OrderCommentsHtml { get; set; }
			/// <summary>
			/// 备注旗帜
			/// </summary>
			[RadioButtonsField("RemarkFlags", typeof(ListItemFromEnum<OrderRemarkFlags>), Group = "Remark")]
			public OrderRemarkFlags RemarkFlags { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[RichTextEditor("Remark", Group = "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(SellerOrder bindFrom) {
				var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
				var displayInfo = bindFrom.ToDisplayInfo(OrderOperatorType.Admin);
				BaseInformationHtml = displayInfo.GetBaseInformationHtml();
				DeliveryRecordsHtml = displayInfo.GetDeliveryRecordsHtml(bindFrom.OrderDeliveries);
				OrderRecordsHtml = displayInfo.GetOrderRecordsHtml();
				ReleatedTransactionsHtml = displayInfo.GetReleatedTransactionsHtml();
				OrderCommentsHtml = displayInfo.GetOrderCommentsHtml();
				RemarkFlags = bindFrom.RemarkFlags;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(SellerOrder saveTo) {
				saveTo.RemarkFlags = RemarkFlags;
				saveTo.Remark = Remark;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
