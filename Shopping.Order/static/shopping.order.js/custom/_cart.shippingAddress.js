/*
	购物车功能
	收货地址的处理
		包含实体商品时显示收货地址表单，否则隐藏
		包含实体商品时显示物流选择，否则隐藏
		支持保存收货地址到订单创建参数
*/

$(function () {
	// 选中的购物车商品改变时的事件
	var $cartContainer = $(".cart-container");
	var $cartProductTable = $cartContainer.find(".cart-product-table");
	var $cartProducts = $cartContainer.find("[name='cartProducts']");
	$cartProducts.on("change", function () {
		var cartProducts = JSON.parse($cartProducts.val() || "{}");
		// 检测是否包含实体商品
		var cartProductMapping = _.indexBy(
			$cartProductTable.find(".cart-product"),
			function (elem) { return $(elem).data("cart-product-id"); });
		var anyRealProduct = _.any(cartProducts,
			function (_, key) { return $(cartProductMapping[key]).data("is-real"); });
		// 包含实体商品时，显示收货地址表单和物流选择，否则隐藏
		var $shippingAddress = $cartContainer.find(".shipping-address");
		var $logisticsSelect = $cartContainer.find(".logistics-select");
		anyRealProduct ? $shippingAddress.removeClass("hide") : $shippingAddress.addClass("hide");
		anyRealProduct ? $logisticsSelect.removeClass("hide") : $logisticsSelect.addClass("hide");
	});
	// 支持保存收货地址到订单创建参数
	var $shippingAddressForm = $cartContainer.find(".user-shipping-address-form");
	var $shippingAddressJson = $shippingAddressForm.find("input[name='ShippingAddressJson']");
	$shippingAddressJson.attr("data-order-parameter", "ShippingAddress");
});
