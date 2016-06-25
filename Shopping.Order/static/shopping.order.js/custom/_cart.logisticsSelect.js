/*
	购物车功能
	物流配送的处理
		物流改变后保存到SellerToLogistics控件
		选择默认的物流配送
		卖家包含实体商品时显示物流选择，否则隐藏
		订单计算价格后显示各个物流配送的费用
		订单计算价格后隐藏不可使用的物流配送
*/

$(function () {
	// 物流改变后保存到SellerToLogistics控件
	var $cartContainer = $(".cart-container");
	var $logisticsSelects = $cartContainer.find(".logistics-select");
	var $sellerToLogistics = $cartContainer.find("[name='sellerToLogistics']");
	$logisticsSelects.on("change", "input[type='radio']", function () {
		var sellerToLogistics = {};
		$logisticsSelects.each(function () {
			var $logisticsSelect = $(this);
			var sellerId = $logisticsSelect.data("seller-id") || 0;
			var logisticsId = $logisticsSelect.find("input[type='radio']:checked").val();
			sellerToLogistics[sellerId] = logisticsId;
		});
		$sellerToLogistics.val(JSON.stringify(sellerToLogistics)).change();
	});
	// 选择默认的物流配送
	$logisticsSelects.each(function () {
		$(this).find("input[type='radio']").first().prop("checked", true).change();
	});
	// 卖家包含实体商品时显示物流选择，否则隐藏
	// TODO: ...
	// 订单计算价格后显示各个物流配送的费用
	// 订单计算价格后隐藏不可使用的物流配送
	// TODO: ...
});
