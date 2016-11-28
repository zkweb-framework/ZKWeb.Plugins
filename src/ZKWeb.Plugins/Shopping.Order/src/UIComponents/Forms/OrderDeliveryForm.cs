using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Extensions;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	using Logistics = Logistics.src.Domain.Entities.Logistics;

	/// <summary>
	/// 订单发货表单
	/// </summary>
	[Form("OrderDeliveryForm")]
	public class OrderDeliveryForm : ModelFormBuilder {
		/// <summary>
		/// 提示Html
		/// </summary>
		[HtmlField("AlertHtml")]
		public HtmlString AlertHtml { get; set; }
		/// <summary>
		/// 物流配送，虚拟发货时不使用
		/// </summary>
		[DropdownListField("Logistics", typeof(ListItemFromEntities<Logistics, Guid>))]
		public Guid Logistics { get; set; }
		/// <summary>
		/// 快递单编号，虚拟发货时不使用
		/// </summary>
		[TextBoxField("LogisticsSerial", PlaceHolder = "LogisticsSerial")]
		public string LogisticsSerial { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[TextBoxField("Remark", PlaceHolder = "Remark")]
		public string Remark { get; set; }
		/// <summary>
		/// 发货件数 { 订单商品Id, 发货数量 }
		/// </summary>
		[Required]
		[JsonField("DeliveryCountsJson", typeof(IDictionary<Guid, long>))]
		public IDictionary<Guid, long> DeliveryCountsJson { get; set; }
		/// <summary>
		/// 发货商品的表格
		/// </summary>
		[HtmlField("DeliveryProductTable")]
		public HtmlString DeliveryProductTable { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() {
			// 获取订单
			var id = Request.Get<Guid>("id");
			var orderManager = Application.Ioc.Resolve<SellerOrderManager>();
			var order = orderManager.Get(id);
			if (order == null) {
				throw new BadRequestException("Order not exist");
			}
			// 包含实际商品时，提示收货地址和物流名称，否则提示是虚拟发货并隐藏物流控件
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var containsRealProduct = order.ContainsRealProduct();
			if (containsRealProduct) {
				var logisticsManager = Application.Ioc.Resolve<LogisticsManager>();
				var shippingAddress = order.OrderParameters.GetShippingAddress();
				var logisticsId = order.OrderParameters.GetSellerToLogistics()
					.GetOrDefault(order.Owner?.Id ?? Guid.Empty);
				var logistics = logisticsManager.GetWithCache(logisticsId);
				var message = new T(
					"The shipping address is \"{0}\", and buyer want to use logistics \"{1}\"",
					shippingAddress?.GenerateSummary(), logistics?.Name);
				AlertHtml = new HtmlString(templateManager.RenderTemplate(
					"shopping.order/order_delivery.alert.html", new { message }));
				Logistics = logistics?.Id ?? Guid.Empty;
			} else {
				var message = new T(
					"Order only contains virtual products, " +
					"if you have something to buyer please use comment");
				AlertHtml = new HtmlString(templateManager.RenderTemplate(
					"shopping.order/order_delivery.alert.html", new { message }));
				Form.Fields.RemoveAll(a =>
					a.Attribute.Name == "Logistics" ||
					a.Attribute.Name == "LogisticsSerial");
			}
			// 构建发货商品的表格
			var tableBuilder = new StaticTableBuilder();
			tableBuilder.Columns.Add("Product");
			tableBuilder.Columns.Add("ShippedQuantity", "130");
			tableBuilder.Columns.Add("ThisDeliveryQuantity", "130");
			foreach (var product in order.OrderProducts) {
				var info = product.ToDisplayInfo();
				tableBuilder.Rows.Add(new {
					Product = info.GetSummaryHtml(),
					ShippedQuantity = info.GetShippedCountHtml(),
					ThisDeliveryQuantity = info.GetDeliveryCountEditor()
				});
			}
			DeliveryCountsJson = new Dictionary<Guid, long>();
			DeliveryProductTable = (HtmlString)tableBuilder.ToLiquid();
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit() {
			var id = Request.Get<Guid>("id");
			var deliveryManager = Application.Ioc.Resolve<OrderDeliveryManager>();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			deliveryManager.DeliveryGoods(
				id, user?.Id, Logistics, LogisticsSerial, Remark, DeliveryCountsJson);
			return this.SaveSuccessAndCloseModal();
		}
	}
}
