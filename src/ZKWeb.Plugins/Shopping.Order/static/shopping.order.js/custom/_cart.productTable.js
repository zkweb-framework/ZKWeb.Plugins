/*
	购物车功能
	购物车商品的处理
		支持删除购物车商品（立刻购买时不显示）
		支持更新商品的购买数量到服务器
		支持保存购物车商品信息到订单创建参数
		计算价格完成后刷新商品总价和商品单价
*/

/* 购物车商品的处理 */
$(function () {
	// 绑定购物车商品改变时的事件
	var $cartContainer = $(".cart-container");
	var $cartProductTable = $cartContainer.find(".cart-product-table");
	var $parametersList = $cartContainer.find("[name='createOrderProductParametersList']");
	var $cartProducts = $cartContainer.find("[name='cartProducts']");
	var cartProductChangeEventName = "cartProductChange.cartView";
	var cartProductChangeEventMask = false;
	$cartContainer.on(cartProductChangeEventName, function () {
		if (cartProductChangeEventMask) {
			return;
		}
		cartProductChangeEventMask = true; // 设置事件处理中，防止重复处理
		var createOrderProductParametersList = []; // 创建订单商品的参数列表
		var totalCount = 0; // 购物车商品数量总计
		var cartProducts = {}; // 购物车商品Id和数量
		$cartProductTable.find(".cart-product").each(function () {
			var $cartProduct = $(this);
			// 没有勾选时跳过
			var $checkbox = $cartProduct.find(".selection input");
			if (!$checkbox.is(":checked")) {
				return;
			}
			// 数量等于0时取消勾选并跳过
			var $orderCount = $cartProduct.find(".order-count input");
			var orderCount = $orderCount.val() || 0;
			if (orderCount <= 0) {
				$checkbox.prop("checked", false).change();
				return;
			}
			// 添加购物车商品关联的商品Id和匹配参数到列表中
			var productId = $cartProduct.data("product-id");
			var matchedParameters = $cartProduct.data("matched-parameters") || "{}";
			var cartProductId = $cartProduct.data("cart-product-id");
			matchedParameters.OrderCount = parseInt(
				$cartProduct.find(".order-count input").val()) || 0; // 更新订购数量
			createOrderProductParametersList.push({
				ProductId: productId,
				MatchParameters: matchedParameters,
				Extra: { cartProductId: cartProductId }
			});
			// 统计购物车商品数量
			totalCount += matchedParameters.OrderCount;
			// 保存购物车商品Id和数量
			// 用于修改服务端数量和提交订单后删除购物车商品
			var cartProductId = $cartProduct.data("cart-product-id");
			cartProducts[cartProductId] = matchedParameters.OrderCount;
		});
		// 更新商品数量总计
		$cartProductTable.find(".cart-product-total .total-count > em").text(totalCount);
		// 保存到控件
		$parametersList.val(JSON.stringify(createOrderProductParametersList)).change();
		$cartProducts.val(JSON.stringify(cartProducts)).change();
		// 设置结束处理
		cartProductChangeEventMask = false;
	});
	// 绑定删除事件，删除后触发改变事件
	$cartContainer.on("click", ".delete", function () {
		var $delete = $(this);
		var id = $delete.data("id");
		// 提交到服务器删除
		$.post("/api/cart/delete", { id: id }, function (data) {
			// 触发迷你购物车的重新初始化事件
			$(".minicart-menu").trigger("reinitialize.miniCart");
			$.handleAjaxResult(data);
			// 删除当前行并触发改变事件
			$delete.closest("[role='row']").remove();
			$cartContainer.trigger(cartProductChangeEventName);
		});
	});
	// 在勾选改变时触发改变事件
	$cartProductTable.on("change", ".selection input", function () {
		$cartContainer.trigger(cartProductChangeEventName);
	});
	// 在数量改变时触发改变事件
	var updateCountsHandler = null;
	$cartProductTable.on("change", ".order-count input", function () {
		// 触发改变事件
		$cartContainer.trigger(cartProductChangeEventName);
		// 一秒后把数量保存到服务端，防止频繁提交
		clearTimeout(updateCountsHandler);
		updateCountsHandler = setTimeout(function () {
			$.post("/api/cart/update_counts", { counts: $cartProducts.val() }, function (data) {
				$.handleAjaxResult(data);
			});
		}, 1000);
	});
	// 支持保存购物车商品信息到订单创建参数
	$parametersList.attr("data-order-parameter", "CreateOrderProductParametersList");
	$cartProducts.attr("data-order-parameter", "CartProducts");
	// 页面载入时触发改变事件
	$cartContainer.trigger(cartProductChangeEventName);
});

/* 购物车商品计算价格的处理 */
$(function () {
	// 计算价格完成后刷新商品总价和商品单价
	var $cartContainer = $(".cart-container");
	var $cartProductTotalPrice = $cartContainer.find(".cart-product-total .total-price > em");
	$cartContainer.on("calcPrice.cartView", function () {
		// 显示计算中
		$cartProductTotalPrice.html($cartContainer.data("calculatingHtml"));
	});
	$cartContainer.on("calcPriceSuccess.cartView", function () {
		var priceInfo = $cartContainer.data("priceInfo");
		// 商品总价
		$cartProductTotalPrice.text(priceInfo.orderProductTotalPriceString);
		// 商品单价
		var cartProductMapping = _.indexBy(
			$cartContainer.find(".cart-product-table .cart-product"),
			function (elem) { return $(elem).data("cart-product-id"); });
		_.each(priceInfo.orderProductUnitPrices, function (info) {
			var $cartProduct = $(cartProductMapping[info.extra.cartProductId]);
			var $price = $cartProduct.find(".unit-price .price");
			$price.attr("title", info.description).text(info.priceString);
		});
	});
});
