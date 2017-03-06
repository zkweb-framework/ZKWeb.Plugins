namespace ZKWeb.Plugins.Finance.Payment.Alipay.src.Components.PaymentApiHandlers {
	/// <summary>
	/// 支付宝的服务类型
	/// </summary>
	public enum AlipayServiceTypes {
		/// <summary>
		/// 即时到账
		/// </summary>
		ImmediateArrival = 0,
		/// <summary>
		/// 担保交易
		/// </summary>
		SecuredTransaction = 1,
		/// <summary>
		/// 双接口
		/// </summary>
		DualInterface = 2,
		/// <summary>
		/// 手机支付
		/// </summary>
		WapPay = 3
	}
}
