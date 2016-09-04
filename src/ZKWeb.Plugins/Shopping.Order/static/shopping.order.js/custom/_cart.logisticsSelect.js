/*
	购物车功能
	物流配送的处理
		物流改变后保存到SellerToLogistics控件
		选择默认的物流配送
		卖家包含实体商品时显示物流选择，否则隐藏
		订单计算价格后显示各个物流配送的费用
		订单计算价格后隐藏不可使用的物流配送
*/

/* 物流配送的处理 */
$(function () {
	// 物流改变后保存到SellerToLogistics控件
	var $cartContainer = $(".cart-container");
	var $logisticsSelects = $cartContainer.find(".logistics-select");
	var $sellerToLogistics = $cartContainer.find("[name='sellerToLogistics']");
	var emptySellerId = "00000000-0000-0000-0000-000000000000";
	$logisticsSelects.on("change", "input[type='radio']", function () {
		var sellerToLogistics = {};
		$logisticsSelects.each(function () {
			var $logisticsSelect = $(this);
			var sellerId = $logisticsSelect.data("seller-id") || emptySellerId;
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
	var $cartProductTable = $cartContainer.find(".cart-product-table");
	var $cartProducts = $cartContainer.find("[name='cartProducts']");
	var onCartProductsChange = function () {
		var cartProducts = JSON.parse($cartProducts.val() || "{}");
		// 获取包含实体商品的卖家列表
		var cartProductMapping = _.indexBy(
			$cartProductTable.find(".cart-product"),
			function (elem) { return $(elem).data("cart-product-id"); });
		var sellerIdsHasRealProducts = [];
		_.each(cartProducts, function (_, key) {
			var $cartProduct = $(cartProductMapping[key]);
			if ($cartProduct.data("is-real")) {
				sellerIdsHasRealProducts.push($cartProduct.data("seller-id") || emptySellerId);
			}
		});
		// 枚举收货地址选择器，判断卖家不包含实体商品时隐藏
		$logisticsSelects.each(function () {
			var $logisticsSelect = $(this);
			var sellerId = $logisticsSelect.data("seller-id") || emptySellerId;
			var hasRealProducts = _.contains(sellerIdsHasRealProducts, sellerId);
			if (hasRealProducts) {
				$logisticsSelect.removeClass("hide");
			} else {
				$logisticsSelect.addClass("hide");
			}
		});
	};
	$cartProducts.on("change", onCartProductsChange);
	onCartProductsChange();
});

/* 物流配送计算价格的处理 */
$(function () {
	// 订单计算价格后显示各个物流配送的费用
	// 订单计算价格后隐藏不可使用的物流配送
	var $cartContainer = $(".cart-container");
	var $logisticsSelects = $cartContainer.find(".logistics-select");
	var emptySellerId = "00000000-0000-0000-0000-000000000000";
	$cartContainer.on("calcPriceSuccess.cartView", function () {
		var priceInfo = $cartContainer.data("priceInfo");
		$logisticsSelects.each(function () {
			var $logisticsSelect = $(this);
			var sellerId = $logisticsSelect.data("seller-id") || emptySellerId;
			var availableList = priceInfo.availableLogistics[sellerId] || [];
			var availableMap = _.indexBy(availableList, "logisticsId");
			$logisticsSelect.find("input[type='radio']").each(function () {
				var $radio = $(this);
				var $parent = $radio.closest(".radio-inline");
				var info = availableMap[$radio.val()];
				if (!info) {
					// 物流不可用
					$parent.addClass("not-available");
				} else {
					// 物流可用
					$parent.removeClass("not-available");
					$parent.find("em").remove();
					$parent.append($("<em>").text("[" + info.costString + "]"));
				}
			});
		});
	});
});
