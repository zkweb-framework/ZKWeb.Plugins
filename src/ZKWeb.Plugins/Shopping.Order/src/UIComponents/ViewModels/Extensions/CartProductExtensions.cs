using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.UIComponents.ViewModels.Extensions {
	/// <summary>
	/// 购物车商品的扩展函数
	/// </summary>
	public static class CartProductExtensions {
		/// <summary>
		/// 从购物车商品生成显示信息
		/// 生成后会设置购物车商品Id到附加数据
		/// </summary>
		/// <param name="cartProduct">购物车商品</param>
		/// <returns></returns>
		public static OrderProductDisplayInfo ToOrderProductDisplayInfo(
			this CartProduct cartProduct) {
			var parameters = cartProduct.ToCreateOrderProductParameters();
			var userId = cartProduct.Owner?.Id;
			var displayInfo = parameters.ToOrderProductDisplayInfo(userId);
			displayInfo.Extra["cartProductId"] = cartProduct.Id;
			return displayInfo;
		}
	}
}
