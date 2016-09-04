using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Model;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.GenericConfigs {
	/// <summary>
	/// 订单设置
	/// </summary>
	[GenericConfig("Shopping.Order.OrderSettings", CacheTime = 15)]
	public class OrderSettings {
		/// <summary>
		/// 立刻购买的购物车商品的过期天数，默认1天
		/// </summary>
		public int BuynowCartProductExpiresDays { get; set; }
		/// <summary>
		/// 一般的购物车商品的过期天数，默认90天
		/// </summary>
		public int NormalCartProductExpiresDays { get; set; }
		/// <summary>
		/// 自动确认收货天数，默认14天
		/// </summary>
		public int AutoConfirmOrderAfterDays { get; set; }
		/// <summary>
		/// 允许非会员下单，默认允许
		/// </summary>
		public bool AllowAnonymousVisitorCreateOrder { get; set; }
		/// <summary>
		/// 库存减少模式，默认不减少
		/// </summary>
		public StockReductionMode StockReductionMode { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public OrderSettings() {
			BuynowCartProductExpiresDays = 1;
			NormalCartProductExpiresDays = 90;
			AutoConfirmOrderAfterDays = 14;
			AllowAnonymousVisitorCreateOrder = true;
			StockReductionMode = StockReductionMode.NoReduction;
		}
	}
}
