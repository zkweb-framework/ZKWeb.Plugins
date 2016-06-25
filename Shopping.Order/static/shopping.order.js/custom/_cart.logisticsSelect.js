/*
	购物车功能
	物流配送的处理
		物流改变后保存到logisticsWithSeller控件
		选择默认的物流配送
		订单计算价格后显示各个物流配送的费用
		订单计算价格后隐藏不可使用的物流配送
*/

$(function () {
	// 物流改变后保存到logisticsWithSeller控件
	var $cartContainer = $(".cart-container");
	var $logisticsSelects = $cartContainer.find(".logistics-select");
	var $logisticsWithSeller = $cartContainer.find("[name='logisticsWithSeller']");
	$logisticsSelects.on("change", "input[type='radio']", function () {
		var logisticsWithSeller = {};
		$logisticsSelects.each(function () {
			var $logisticsSelect = $(this);
			var sellerId = $logisticsSelect.data("seller-id");
			var logisticsId = $logisticsSelect.find("input[type='radio']:checked").val();
			logisticsWithSeller[sellerId] = logisticsId;
		});
		$logisticsWithSeller.val(JSON.stringify(logisticsWithSeller));
	});
	// 选择默认的物流配送
	$logisticsSelects.each(function () {
		$(this).find("input[type='radio']").first().prop("checked", true).change();
	});
	// 订单计算价格后显示各个物流配送的费用
	// 订单计算价格后隐藏不可使用的物流配送
	// TODO: ...
});
