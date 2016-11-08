using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Collections;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderCheckers.Interfaces {
	/// <summary>
	/// 订单检查器，用于检查订单是否可以进行各操作
	/// </summary>
	public interface IOrderChecker {
		/// <summary>
		/// 判断订单是否可以付款
		/// 前台使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanPay(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 判断订单是否可以取消
		/// 前台使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanSetCancelled(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 判断订单是否可以确认收货
		/// 前台使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanConfirm(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 判断订单是否可以修改价格
		/// 后台使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanEditCost(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 判断订单是否可以编辑收货地址
		/// 后台使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanEditShippingAddress(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 判断订单是否可以发货
		/// 后台使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanDeliveryGoods(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 判断订单是否可以作废
		/// 后台使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanSetInvalid(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 是否可处理订单已付款
		/// 修改订单状态时使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanProcessOrderPaid(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 是否可处理订单所有商品已发货
		/// 修改订单状态时使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanProcessAllGoodsShipped(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 是否可处理交易成功
		/// 修改订单状态时使用
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		void CanProcessSuccess(SellerOrder order, ref Pair<bool, string> result);
	}
}
