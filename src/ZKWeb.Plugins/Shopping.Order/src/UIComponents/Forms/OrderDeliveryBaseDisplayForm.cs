using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 发货单详情页表单的基类
	/// </summary>
	public abstract class OrderDeliveryBaseDisplayForm<TForm> :
		TabEntityFormBuilder<OrderDelivery, Guid, TForm>
		where TForm : OrderDeliveryBaseDisplayForm<TForm> {
		/// <summary>
		/// 发货单编号
		/// </summary>
		[LabelField("DeliverySerial")]
		public string DeliverySerial { get; set; }
		/// <summary>
		/// 快递单编号
		/// </summary>
		[LabelField("OrderLogistics")]
		public string OrderLogistics { get; set; }
		/// <summary>
		/// 物流配送
		/// </summary>
		[LabelField("LogisticsSerial")]
		public string LogisticsSerial { get; set; }
		/// <summary>
		/// 发货人
		/// </summary>
		[LabelField("DeliveryOperator")]
		public string DeliveryOperator { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		[LabelField("CreateTime")]
		public string CreateTime { get; set; }
		/// <summary>
		/// 本次发货的订单商品表格
		/// </summary>
		[HtmlField("OrderProductsTable")]
		public HtmlString DeliveryProductTable { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind(OrderDelivery bindFrom) {
			DeliverySerial = bindFrom.Serial;
			OrderLogistics = new T(bindFrom.Logistics?.Name);
			LogisticsSerial = bindFrom.LogisticsSerial;
			DeliveryOperator = new T(bindFrom.Operator?.Username);
			CreateTime = bindFrom.CreateTime.ToClientTimeString();
			var tableBuilder = new StaticTableBuilder();
			tableBuilder.Columns.Add("Product");
			tableBuilder.Columns.Add("ThisDeliveryQuantity", "130");
			foreach (var product in bindFrom.OrderProducts) {
				var info = product.OrderProduct.ToDisplayInfo();
				tableBuilder.Rows.Add(new {
					Product = info.GetSummaryHtml(),
					ThisDeliveryQuantity = product.Count
				});
			}
			DeliveryProductTable = (HtmlString)tableBuilder.ToLiquid();
		}
	}
}
