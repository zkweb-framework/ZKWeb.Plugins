/*
	支付宝扫码支付功能

	定时查询支付是否已完成或失败
*/

$(function () {
	var $container = $(".qrcode-pay-container");
	var transactionId = $container.attr("data-transaction-id");
	var updateInterval = parseInt($container.attr("data-update-interval")) || 3000;
	var updateTransactionState = function () {
		$.post("/payment/alipay_qrcode_pay/update_transaction_state", { transactionId: transactionId })
			.done(function (data) { $.handleAjaxResult(data); })
			.always(function () { setTimeout(updateTransactionState, updateInterval); });
	};
	setTimeout(updateTransactionState, updateInterval);
});
