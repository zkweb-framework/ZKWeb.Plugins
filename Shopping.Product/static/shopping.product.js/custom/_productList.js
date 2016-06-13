/*
	商品列表页使用的功能
	- 支持按价格过滤
*/

// 支持按价格过滤
$(function () {
	var $filterNav = $(".product-filter-by-price-and-order");
	var $priceLowerBound = $filterNav.find(".price-lower-bound");
	var $priceUpperBound = $filterNav.find(".price-upper-bound");
	var $priceFilter = $filterNav.find(".price-filter");
	var $cancelPriceFilter = $filterNav.find(".price-filter-cancel");
	// 设置价格范围的值
	var priceRangeParam = $priceFilter.attr("data-value");
	var priceRange = (priceRangeParam || "0~0").split('~');
	$priceLowerBound.val(priceRange[0] || 0);
	$priceUpperBound.val(priceRange[1] || 0);
	// 有指定价格过滤时，显示取消按钮
	if (priceRangeParam) {
		$priceFilter.addClass("selected");
		$cancelPriceFilter.removeClass("hide");
	}
	// 价格范围有修改时应用到过滤按钮
	var onPriceBoundChanged = function () {
		var lowerBound = parseFloat($priceLowerBound.val()) || 0;
		var upperBound = parseFloat($priceUpperBound.val()) || 0;
		$priceFilter.attr("data-value", lowerBound + "~" + upperBound);
	};
	$priceLowerBound.on("change", onPriceBoundChanged);
	$priceUpperBound.on("change", onPriceBoundChanged);
	onPriceBoundChanged();
	// 过滤按钮点击时适用条件
	$priceFilter.on("click", function () {
		var priceRange = $priceFilter.attr("data-value");
		var uri = new Uri($cancelPriceFilter.attr("href"));
		uri.replaceQueryParam("price_range", priceRange);
		location.href = uri.path() + uri.query();
	});
	// 在价格范围框中按下enter时触发过滤按钮的点击事件
	var onPriceBoundKeyDown = function (e) {
		if (e.keyCode == 13) {
			onPriceBoundChanged();
			$priceFilter.click();
		}
	};
	$priceLowerBound.on("keydown", onPriceBoundKeyDown);
	$priceUpperBound.on("keydown", onPriceBoundKeyDown);
});
