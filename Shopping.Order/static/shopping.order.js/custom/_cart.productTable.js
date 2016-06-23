/*
	购物车功能
	购物车商品的处理
		包含实体商品时会显示收货地址表单
		所有商品都是虚拟商品时会删除收货地址表单
		包含实体商品时会显示物流选择
		所有商品都是虚拟商品时会删除物流选择
		
		支持删除购物车商品（立刻购买时不显示）
		支持保存购物车商品Id到订单创建参数
*/

/* 购物车商品的处理 */
$(function () {
	// 绑定购物车商品改变时的事件
	var $cartContainer = $(".cart-container");
	var $cartProductTable = $cartContainer.find(".cart-product-table");
	var cartProductChangeEventName = "cartProductChange.cartView";
	$cartContainer.on(cartProductChangeEventName, function () {
		var createOrderProductParameters = []; // 创建订单商品的参数列表
		var anyRealProduct = false; // 是否包含实体商品
		var totalCount = 0; // 购物车商品数量总计
		var cartProducts = {}; // 购物车商品Id和数量
		$cartProductTable.find(".cart-product").each(function () {
			var $cartProduct = $(this);
			// 添加购物车商品关联的商品Id和匹配参数到列表中
			var productId = $cartProduct.data("product-id");
			var matchedParameters = $cartProduct.data("matched-parameters") || "{}";
			matchedParameters.OrderCount = parseInt(
				$cartProduct.find(".order-count input").val()) || 0; // 更新订购数量
			createOrderProductParameters.push({
				ProductId: productId,
				MatchParameters: matchedParameters
			});
			// 检测是否包含实体商品
			if ($cartProduct.data("is-real")) {
				anyRealProduct = true;
			}
			// 统计购物车商品数量
			totalCount += matchedParameters.OrderCount;
			// 保存购物车商品Id和数量
			// 用于修改服务端数量和提交订单后删除购物车商品
			var cartProductId = $cartProduct.data("cart-product-id");
			cartProducts[cartProductId] = matchedParameters.OrderCount;
		});
		// 不包含实体商品时，删除收货地址表单和物流选择
		if (!anyRealProduct) {
			$cartContainer.find(".shipping-address").remove();
			$cartContainer.find(".logistics-select").remove();
		}
		// 更新商品数量总计
		$cartProductTable.find(".cart-product-total .total-count > em").text(totalCount);
		// 保存到控件
		var $parameters = $cartContainer.find("[name='createOrderProductParameters']");
		$parameters.val(JSON.stringify(createOrderProductParameters)).change();
		var $cartProducts = $cartContainer.find("[name='cartProducts']");
		$cartProducts.val(JSON.stringify(cartProducts)).change();
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
	// 在数量改变时触发改变事件
	var updateCountsHandler = null;
	$cartProductTable.on("change", ".order-count input", function () {
		// 触发改变事件
		$cartContainer.trigger(cartProductChangeEventName);
		// 一秒后把数量保存到服务端，防止频繁提交
		clearTimeout(updateCountsHandler);
		updateCountsHandler = setTimeout(function () {
			var $cartProducts = $cartContainer.find("[name='cartProducts']");
			$.post("/api/cart/update_counts", { counts: $cartProducts.val() }, function (data) {
				$.handleAjaxResult(data);
			});
		}, 1000);
	});
	// 页面载入时触发改变事件
	$cartContainer.trigger(cartProductChangeEventName);
});
