/*
	迷你购物车功能
	鼠标悬浮时从服务器获取购物车商品和价格信息
*/

$(function () {
	// 鼠标悬浮时从服务器获取购物车商品和价格信息
	var initialize = function ($menu) {
		var $dropdownMenu = $menu.find(".dropdown-menu");
		$dropdownMenu.html("<div class='loading'></div>");
		$dropdownMenu.load("/cart/minicart_contents", function () {
			// 载入后更新商品总件数
			var totalCount = $dropdownMenu.find(".summary .total-count > em").text();
			if (totalCount != "") {
				$menu.find("> a > em").text(totalCount);
			}
		});
	};
	var $minicartMenu = $(".minicart-menu");
	$minicartMenu.on("show.bs.dropdown", function () {
		var $menu = $(this);
		if ($menu.data("initialized")) {
			return;
		}
		$menu.data("initialized", true);
		initialize($menu);
	});
	// 绑定重新初始化的事件，在购物车商品发生变化后可以触发这个事件
	var reinitializeEventName = "reinitialize.miniCart";
	$minicartMenu.on(reinitializeEventName, function () {
		initialize($(this));
	});
	// 绑定删除购物车商品的事件
	$minicartMenu.on("click", ".delete", function () {
		var id = $(this).data("id");
		$.post("/api/cart/delete", { id: id }, function (data) {
			$minicartMenu.trigger(reinitializeEventName);
			$.handleAjaxResult(data);
		});
		return false;
	});
});
