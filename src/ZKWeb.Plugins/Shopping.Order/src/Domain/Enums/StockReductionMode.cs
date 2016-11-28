namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Enums {
	/// <summary>
	/// 库存减少模式
	/// </summary>
	public enum StockReductionMode {
		/// <summary>
		/// 不减少
		/// </summary>
		NoReduction = 0,
		/// <summary>
		/// 创建订单后减少
		/// </summary>
		AfterCreateOrder = 1,
		/// <summary>
		/// 支付成功后减少
		/// </summary>
		AfterOrderPaid = 2
	}
}
