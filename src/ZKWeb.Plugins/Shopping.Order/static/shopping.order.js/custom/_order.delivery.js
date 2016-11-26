/*
	订单发货功能
	页面结构
	<form name="OrderDeliveryForm">
		<input type='hidden' name='DeliveryCountsJson' />
		<table class='table'>
			<tbody>
				<tr>
					<dl class="order-product-summary" data-order-product-id="订单商品Id"></dl>
					<input class="delivery-count" value="本次发货数量" />
			</tr>
		</div>
	</form>
	保存内容
		DeliveryCountsJson { 订单商品Id: 发货数, ... }
	事件
		collectAll.orderDeliveryGoods
			在页面打开或发货数修改时触发
			收集值并保存到DeliveryCountsJson
*/

$(function () {
	// 获取元素并检查
	var $form = $("[name='OrderDeliveryForm']");
	var $table = $form.find(".table");
	var $deliveryCountsJson = $form.find("[name='DeliveryCountsJson']");
	if (!$form.length || !$table.length || !$deliveryCountsJson.length) {
		console.warn("init order delivery goods script failed:", $form, $table, $deliveryCountsJson);
		return;
	}
	// 绑定收集事件
	var collectEventName = "collectAll.orderDeliveryGoods";
	$form.bind(collectEventName, function () {
		var deliveryCountsMapping = {};
		var $rows = $table.find("> tbody > tr");
		console.log("rows", $rows);
		$rows.each(function () {
			var orderProductId = $(this).find("[data-order-product-id]").data("order-product-id");
			var count = $(this).find(".delivery-count").val();
			deliveryCountsMapping[orderProductId] = count;
		});
		console.log(deliveryCountsMapping);
		$deliveryCountsJson.val(JSON.stringify(deliveryCountsMapping));
	});
	// 发货数修改时触发收集事件
	$form.find(".delivery-count").on("change", function () {
		$form.trigger(collectEventName);
	});
	// 页面打开时触发收集事件
	$form.trigger(collectEventName);
});
