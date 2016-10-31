using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.Forms {
	/// <summary>
	/// 编辑订单价格的表单
	/// </summary>
	public class OrderEditCostForm : EntityFormBuilder<SellerOrder, Guid, OrderEditCostForm> {
		/// <summary>
		/// 订单操作人的类型
		/// </summary>
		public OrderOperatorType OperatorType { get; set; }
		/// <summary>
		/// 修改订单价格的参数
		/// </summary>
		[JsonField("OrderEditCostParametersJson", typeof(OrderEditCostParameters))]
		public OrderEditCostParameters OrderEditParametersJson { get; set; }
		/// <summary>
		/// 修改订单商品价格的表格
		/// </summary>
		[HtmlField("OrderProductPriceEditTable")]
		public HtmlString OrderProductPriceEditTable { get; set; }
		/// <summary>
		/// 修改订单各项价格的表格
		/// </summary>
		[HtmlField("OrderCostEditTable")]
		public HtmlString OrderCostEditTable { get; set; }
		/// <summary>
		/// 修改订单交易金额的表格
		/// </summary>
		[HtmlField("OrderTransactionAmountEditTable")]
		public HtmlString OrderTransactionAmountEditTable { get; set; }
		
		/// <summary>
		/// 初始化
		/// </summary>
		public OrderEditCostForm(OrderOperatorType operatorType) {
			OperatorType = operatorType;
		}

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind(SellerOrder bindFrom) {
			var displayInfo = bindFrom.ToDisplayInfo(OperatorType);
			OrderEditParametersJson = new OrderEditCostParameters();
			OrderProductPriceEditTable = displayInfo.GetOrderProductPriceEditHtml();
			OrderCostEditTable = displayInfo.GetOrderCostEditHtml();
			OrderTransactionAmountEditTable = displayInfo.GetOrderTransactionEditHtml();
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		protected override object OnSubmit(SellerOrder saveTo) {
			throw new NotImplementedException();
		}
	}
}
