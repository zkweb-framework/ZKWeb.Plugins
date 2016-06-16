namespace ZKWeb.Plugins.Finance.Payment.src.Model {
	/// <summary>
	/// 担保交易付款后，自动发货使用的参数
	/// </summary>
	public class AutoSendGoodsParameters {
		/// <summary>
		/// 快递或物流名称
		/// </summary>
		public string LogisticsName { get; set; }
		/// <summary>
		/// 发货单号
		/// </summary>
		public string InvoiceNo { get; set; }
	}
}
