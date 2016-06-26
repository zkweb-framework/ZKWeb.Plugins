/*
	购物车功能
	支付接口的处理
		支付接口改变后保存到paymentApiId控件
		选择默认的支付接口
		订单计算价格后显示各个支付接口的手续费
		订单计算价格后隐藏不可使用的支付接口
*/

/* 支付接口的处理 */
$(function () {
	// 支付接口改变后保存到paymentApiId控件
	var $cartContainer = $(".cart-container");
	var $paymentApiSelect = $cartContainer.find(".payment-api-select");
	var $paymentApiId = $cartContainer.find("[name='paymentApiId']");
	$paymentApiSelect.on("change", "input[type='radio']", function () {
		$paymentApiId.val($paymentApiSelect.find("input[type='radio']:checked").val()).change();
	});
	// 选择默认的支付接口
	$paymentApiSelect.find("input[type='radio']").first().prop("checked", true).change();
});

/* 支付接口计算价格的处理 */
$(function () {
	// 订单计算价格后显示各个支付接口的手续费
	// 订单计算价格后隐藏不可使用的支付接口
	var $cartContainer = $(".cart-container");
	var $paymentApiSelect = $cartContainer.find(".payment-api-select");
	$cartContainer.on("calcPriceSuccess.cartView", function () {
		var priceInfo = $cartContainer.data("priceInfo");
		var availableMap = _.indexBy(priceInfo.availablePaymentApis, "apiId");
		$paymentApiSelect.find("input[type='radio']").each(function () {
			var $radio = $(this);
			var $parent = $radio.closest(".radio-inline");
			var info = availableMap[$radio.val()];
			if (!info) {
				// 支付接口不可用
				$parent.addClass("not-available");
			} else {
				// 支付接口可用
				$parent.removeClass("not-available");
				$parent.find("em").remove();
				$parent.append($("<em>").text("[" + info.feeString + "]"));
			}
		});
	});
});
