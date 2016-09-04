/*
	商品详情页的购买功能
	立刻购买
		请求 /api/cart/add
		成功时跳转到 /cart?type=buynow
	加入购物车
		请求 /api/cart/add
		成功时显示弹出框
	失败时显示错误信息
*/

// 在选中的规格或数量改变后把数据复制到表单中
// 判断无库存、购买数量为零时禁用按钮，否则启用按钮
$(function () {
	var onParametersChanged = function () {
		var $this = $(this);
		var $form = $this.closest(".product-summary-row").find(".product-purchase-form");
		var parameters = $this.data("parameters");
		$form.addClass("unbuyable");
		if (parameters) {
			var orderCount = parameters.OrderCount;
			var stock = parseInt($this.find(".stock").text());
			var unbuyable = (orderCount <= 0 || stock <= 0);
			$form.find("[name='matchParameters']").val(JSON.stringify(parameters));
			unbuyable ? $form.addClass("unbuyable") : $form.removeClass("unbuyable");
		}
	};
	var $salesInfo = $(".product-view .product-sales-info");
	$salesInfo.on("applyMatchParameters.productSalesInfo", onParametersChanged);
	$salesInfo.each(function () { onParametersChanged.call($(this)); });
});

// 点击立刻购买或加入购物车时提交表单
// 其他插件需要可以注册表单的submit事件实现功能，这里不需要提供另外的事件
$(function () {
	var $buttons = $(".product-view .product-purchase-buttons");
	var onCartAdd = function (isBuynow) {
		var $form = $(this).closest($buttons).find(".product-purchase-form");
		$form.find("[name='isBuyNow']").val(isBuynow ? "true" : "false");
		$form.submit();
	};
	$buttons.find(".btn-buynow").on("click", function () { onCartAdd.call(this, true) });
	$buttons.find(".btn-add-to-cart").on("click", function () { onCartAdd.call(this, false) });
	// 表单提交成功后跳转或弹出购物车弹出框
	var $form = $buttons.find(".product-purchase-form");
	var dialogTimer = null;
	$form.on("success", function (e, data) {
		// 需要跳转时跳转到指定的页面（可能是用户登录页或立刻购买购物车页等）
		if (data.redirectTo) {
			location.href = data.redirectTo;
			return;
		}
		// 需要显示弹出框时设置总商品件数和价格并显示购物车弹出框
		if (data.showDialog) {
			var $dialog = $(this).closest($buttons).find(".product-added-to-cart-dialog");
			$dialog.find(".total-count").text(data.showDialog.totalCount);
			$dialog.find(".total-price").text(data.showDialog.totalPriceString);
			$dialog.removeClass("hide");
			// 5秒后自动关闭
			clearTimeout(dialogTimer);
			dialogTimer = setTimeout(function () { $dialog.addClass("hide"); }, 5000);
		}
		// 触发迷你购物车的重新初始化事件
		$(".minicart-menu").trigger("reinitialize.miniCart");
	});
	// 绑定关闭弹出框的事件
	$buttons.find(".product-added-to-cart-dialog .dialog-close").on("click", function () {
		$(this).closest(".product-added-to-cart-dialog").addClass("hide");
	});
});
