using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Enums;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 买家订单的扩展函数
	/// </summary>
	public static class BuyerOrderExtensions {
		/// <summary>
		/// 转换订单到显示信息
		/// </summary>
		/// <param name="order">买家订单</param>
		/// <returns></returns>
		public static OrderDisplayInfo ToDisplayInfo(this BuyerOrder order) {
			var info = order.SellerOrder.ToDisplayInfo(OrderOperatorType.Buyer);
			info.RemarkFlags = order.RemarkFlags.ToString();
			return info;
		}
	}
}
