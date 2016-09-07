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
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 后台的订单管理的控制器
	/// 只在后台使用，需要卖家使用的订单管理请安装多商城插件
	/// </summary>
	[ExportMany]
	public class OrderCrudController : CrudAdminAppControllerBase<SellerOrder, Guid> {
		public override string Group { get { return "Shop Manage"; } }
		public override string GroupIconClass { get { return "fa fa-building"; } }
		public override string Name { get { return "OrderManage"; } }
		public override string Url { get { return "/admin/orders"; } }
		public override string TileClass { get { return "tile bg-aqua"; } }
		public override string IconClass { get { return "fa fa-cart-arrow-down"; } }
		public override string AddUrl { get { return null; } }
		public override string EditUrl { get { return null; } }
		protected override IAjaxTableHandler<SellerOrder, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderCrudController() {
			IncludeCss.Add("/static/shopping.order.css/order-list.css");
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
				table.StandardSetupFor<OrderCrudController>();
				table.Template = "/static/shopping.order.tmpl/orderTable.tmpl";
				searchBar.StandardSetupFor<OrderCrudController>("Serial/Remark");
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
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Serial"] = pair.Entity.Serial;
					pair.Row["HeadingHtml"] = null;
					pair.Row["OrderProducts"] = null;
					pair.Row["Price"] = null;
					pair.Row["Quantity"] = null;
					pair.Row["ShippedQuantity"] = null;
					pair.Row["TotalCost"] = null;
					pair.Row["State"] = null;
					pair.Row["Buyer"] = null;
					pair.Row["BuyerId"] = null;
					pair.Row["Seller"] = null;
					pair.Row["SellerId"] = null;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<OrderCrudController>(request);
				response.Columns.AddHtmlColumn("OrderProducts", "30%");
				response.Columns.AddHtmlColumn("Price", "70");
				response.Columns.AddHtmlColumn("Quantity", "70");
				response.Columns.AddHtmlColumn("ShippedQuantity", "70");
				response.Columns.AddHtmlColumn("TotalCost", "70");
				response.Columns.AddEnumLabelColumn("State", typeof(OrderState), "50");
				response.Columns.AddEditColumnFor<UserCrudController>("Buyer", "BuyerId", width: "70");
				response.Columns.AddEditColumnFor<UserCrudController>("Seller", "SellerId", width: "70");
				var actionColumn = response.Columns.AddActionColumn();
			}
		}
	}
}
