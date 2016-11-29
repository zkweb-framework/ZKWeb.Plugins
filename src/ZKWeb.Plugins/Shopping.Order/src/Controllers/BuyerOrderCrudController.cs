using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
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
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems;
using ZKWeb.Plugins.Common.UserPanel.src.Controllers.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.HtmlItems.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.UserPanelPages {
	/// <summary>
	/// 前台的订单管理的控制器
	/// </summary>
	[ExportMany]
	public class BuyerOrderCrudController : CrudUserPanelControllerBase<BuyerOrder, Guid> {
		public override string Group { get { return "OrderManage"; } }
		public override string GroupIconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string Name { get { return "OrderManage"; } }
		public override string Url { get { return "/user/orders"; } }
		public override string IconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string AddUrl { get { return null; } }
		public override bool AllowDeleteForever { get { return false; } }
		public override string EditTemplatePath { get { return "common.user_panel/generic_edit_standalone.html"; } }
		protected override IAjaxTableHandler<BuyerOrder, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public BuyerOrderCrudController() {
			IncludeCss.Add("/static/shopping.order.css/progress-wizard.css");
			IncludeCss.Add("/static/shopping.order.css/order-list.css");
			IncludeCss.Add("/static/shopping.order.css/order-view.css");
			IncludeJs.Add("/static/shopping.order.js/order-list.min.js");
			IncludeJs.Add("/static/shopping.order.js/order-view.min.js");
		}

		/// <summary>
		/// 调整订单管理和订单列表的位置
		/// </summary>
		/// <param name="groups">菜单项列表的分组</param>
		public override void Setup(IList<MenuItemGroup> groups) {
			base.Setup(groups);
			var groupIndex = groups.FindIndex(g => g.Name == Group);
			var group = groups[groupIndex];
			groups.RemoveAt(groupIndex);
			groups.Insert(1, group); // 订单管理排第二（仅次于首页）
			var itemIndex = group.Items.Count - 1;
			var item = group.Items[itemIndex];
			group.Items.RemoveAt(itemIndex);
			group.Items.Insert(0, item); // 订单列表在订单管理中排第一
		}

		/// <summary>
		/// 取消订单
		/// </summary>
		protected IActionResult CancelOrder() {
			var form = new OrderCancelForm();
			if (Request.Method == HttpMethods.GET) {
				form.Bind();
				return new TemplateResult("shopping.order/order_set_cancelled.html", new { form });
			} else {
				return new JsonResult(form.Submit());
			}
		}

		/// <summary>
		/// 查看发货单
		/// </summary>
		/// <returns></returns>
		protected IActionResult DeliveryView() {
			var form = new OrderDeliveryBuyerDisplayForm();
			form.Bind();
			return new TemplateResult("shopping.order/order_delivery_view.html", new { form });
		}

		/// <summary>
		/// 确认收货
		/// </summary>
		protected IActionResult ConfirmOrder() {
			var form = new OrderConfirmForm();
			if (Request.Method == HttpMethods.GET) {
				form.Bind();
				return new TemplateResult("shopping.order/order_confirm.html", new { form });
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
			// 取消订单
			var cancelOrderUrl = Url + "/cancel_order";
			controllerManager.RegisterAction(
				cancelOrderUrl, HttpMethods.GET, WrapAction(CancelOrder, EditPrivileges));
			controllerManager.RegisterAction(
				cancelOrderUrl, HttpMethods.POST, WrapAction(CancelOrder, EditPrivileges));
			// 查看发货单
			var deliveryViewUrl = Url + "/delivery_view";
			controllerManager.RegisterAction(
				deliveryViewUrl, HttpMethods.GET, WrapAction(DeliveryView, EditPrivileges));
			// 确认收货
			var confirmOrderUrl = Url + "/confirm_order";
			controllerManager.RegisterAction(
				confirmOrderUrl, HttpMethods.GET, WrapAction(ConfirmOrder, EditPrivileges));
			controllerManager.RegisterAction(
				confirmOrderUrl, HttpMethods.POST, WrapAction(ConfirmOrder, EditPrivileges));
		}

		/// <summary>
		/// 表格处理器
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<BuyerOrder, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<BuyerOrderCrudController>();
				table.Template = "/static/shopping.order.tmpl/orderTable.tmpl";
				searchBar.StandardSetupFor<BuyerOrderCrudController>("Serial/Remark");
				searchBar.BeforeItems.AddOrderFilterBar();
				searchBar.Conditions.Add(new FormField(new TextBoxFieldAttribute("Seller")));
				searchBar.Conditions.Add(new FormField(new DropdownListFieldAttribute(
					"RemarkFlags", typeof(ListItemsWithOptional<ListItemFromEnum<OrderRemarkFlags>>))));
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<BuyerOrder> query) {
				// 按状态
				var state = request.Conditions.GetOrDefault<OrderState?>("State");
				if (state != null) {
					query = query.Where(o => o.SellerOrder.State == state);
				}
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.SellerOrder.Serial.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
				// 按卖家
				var buyer = request.Conditions.GetOrDefault<string>("Seller");
				if (!string.IsNullOrEmpty(buyer)) {
					query = query.Where(q => q.SellerOrder.Owner.Username == buyer);
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
				AjaxTableSearchRequest request, IList<EntityToTableRow<BuyerOrder>> pairs) {
				foreach (var pair in pairs) {
					var displayInfo = pair.Entity.ToDisplayInfo();
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Serial"] = pair.Entity.SellerOrder.Serial;
					pair.Row["HeadingHtml"] = displayInfo.GetTableHeadingHtml().ToString();
					pair.Row["OrderProducts"] = displayInfo.OrderProducts.GetSummryListHtml().ToString();
					pair.Row["Price"] = displayInfo.OrderProducts.GetPriceListHtml().ToString();
					pair.Row["Quantity"] = displayInfo.OrderProducts.GetOrderCountListHtml().ToString();
					pair.Row["ShippedQuantity"] = displayInfo.OrderProducts.GetShippedCountListHtml().ToString();
					pair.Row["TotalCost"] = displayInfo.GetTotalCostWithPartsHtml().ToString();
					pair.Row["State"] = pair.Entity.SellerOrder.State;
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
				response.Columns.AddIdColumn("Id").StandardSetupFor<BuyerOrderCrudController>(request);
				response.Columns.AddHtmlColumn("OrderProducts", "30%");
				response.Columns.AddHtmlColumn("Price", "70");
				response.Columns.AddHtmlColumn("Quantity", "70");
				response.Columns.AddHtmlColumn("ShippedQuantity", "70");
				response.Columns.AddHtmlColumn("TotalCost", "70");
				response.Columns.AddEnumLabelColumn("State", typeof(OrderState), "50");
				response.Columns.AddMemberColumn("Seller", "70");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn("5%");
				var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
				if (!deleted) {
					actionColumn.AddButtonForOpenLink(
						new T("View"), "btn btn-xs btn-info", "fa fa-edit",
						"/user/orders/edit?serial=<%-row.Serial%>");
				}
				actionColumn.AddHtmlAction("OrderActions");
			}
		}

		/// <summary>
		/// 订单表单
		/// </summary>
		public class Form : TabEntityFormBuilder<BuyerOrder, Guid, Form> {
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
			/// 添加订单留言
			/// </summary>
			[TextAreaField("OrderComment", 5, "Add comment here then click submit", Group = "OrderComments")]
			public string OrderComment { get; set; }
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
			/// 获取请求的Id
			/// </summary>
			protected override Guid GetRequestId() {
				var serial = Request.Get<string>("serial");
				var orderManager = Application.Ioc.Resolve<BuyerOrderManager>();
				var orderId = orderManager.GetBuyerOrderIdFromSerial(serial);
				if (!orderId.HasValue) {
					throw new NotFoundException(new T("Order not exist"));
				}
				return orderId.Value;
			}

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(BuyerOrder bindFrom) {
				var orderManager = Application.Ioc.Resolve<BuyerOrderManager>();
				var displayInfo = bindFrom.ToDisplayInfo();
				BaseInformationHtml = displayInfo.GetBaseInformationHtml();
				DeliveryRecordsHtml = displayInfo.GetDeliveryRecordsHtml(bindFrom.SellerOrder.OrderDeliveries);
				OrderRecordsHtml = displayInfo.GetOrderRecordsHtml();
				ReleatedTransactionsHtml = displayInfo.GetReleatedTransactionsHtml();
				OrderComment = null;
				OrderCommentsHtml = displayInfo.GetOrderCommentsHtml(bindFrom.SellerOrder.OrderComments);
				RemarkFlags = bindFrom.RemarkFlags;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(BuyerOrder saveTo) {
				saveTo.RemarkFlags = RemarkFlags;
				saveTo.Remark = Remark;
				if (!string.IsNullOrEmpty(OrderComment)) {
					var sessionManager = Application.Ioc.Resolve<SessionManager>();
					var user = sessionManager.GetSession().GetUser();
					var orderCommentManager = Application.Ioc.Resolve<OrderCommentManager>();
					orderCommentManager.AddComment(saveTo.SellerOrder,
						user?.Id, OrderCommentSide.BuyerComment, OrderComment);
				}
				return this.SaveSuccessAndRefreshPage(1500);
			}
		}
	}
}
