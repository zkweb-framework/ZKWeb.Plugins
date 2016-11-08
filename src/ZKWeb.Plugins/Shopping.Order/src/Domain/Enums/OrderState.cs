using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums.Attributes;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Enums {
	/// <summary>
	/// 订单状态
	/// 因为使用了固定的逻辑处理，这里不开放接口
	/// </summary>
	public enum OrderState {
		/// <summary>
		/// 等待付款
		/// </summary>
		[LabelCssClass("label label-default")]
		WaitingBuyerPay = 0,
		/// <summary>
		/// 等待发货
		/// </summary>
		[LabelCssClass("label label-warning")]
		WaitingSellerDeliveryGoods = 1,
		/// <summary>
		/// 已发货
		/// </summary>
		[LabelCssClass("label label-primary")]
		WaitingBuyerConfirm = 2,
		/// <summary>
		/// 交易成功
		/// </summary>
		[LabelCssClass("label label-success")]
		OrderSuccess = 3,
		/// <summary>
		/// 已取消
		/// 指买家操作取消
		/// </summary>
		[LabelCssClass("label label-danger")]
		OrderCancelled = 4,
		/// <summary>
		/// 已作废
		/// 指卖家操作作废
		/// </summary>
		[LabelCssClass("label label-danger")]
		OrderInvalid = 5,
	}
}
