/*
	订单发货功能
	页面结构
	<form id='DeliveryGoodsForm'>
		<input type='hidden' name='DeliveryCountsJson' />
		<div class='send-goods-product-table'>
			<input type='text' data-order-product-id='订单商品Id' value='发货数' />
			...
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
	var $form = $("#OrderDeliveryGoodsForm");
	var $table = $form.find(".send-goods-product-table");
	var $deliveryCountsJson = $form.find("[name='DeliveryCountsJson']");
	if (!$form.length || !$table.length || !$deliveryCountsJson) {
		console.warn("init order send goods script failed:", $form, $table, $deliveryCountsJson);
		return;
	}
	// 绑定收集事件
	var collectEventName = "collectAll.orderDeliveryGoods";
	$form.bind(collectEventName, function () {
		var deliveryCountsMapping = {};
		var $counts = $form.find("[data-order-product-id]");
		$counts.each(function () {
			var $count = $(this);
			deliveryCountsMapping[$count.data("order-product-id")] = $count.val();
		});
		$deliveryCountsJson.val(JSON.stringify(deliveryCountsMapping));
	});
	// 发货数修改时触发收集事件
	$form.find("[data-order-product-id]").on("change", function () {
		$form.trigger(collectEventName);
	});
	// 页面打开时触发收集事件
	$form.trigger(collectEventName);
});
