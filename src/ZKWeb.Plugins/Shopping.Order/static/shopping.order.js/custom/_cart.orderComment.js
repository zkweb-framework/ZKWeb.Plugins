/*
	购物车功能
	订单留言的处理
		支持保存订单留言到订单创建参数
*/

$(function () {
	// 支持保存订单留言到订单创建参数
	var $cartContainer = $(".cart-container");
	$cartContainer.find("[name='OrderComment']").attr("data-order-parameter", "OrderComment");
});
