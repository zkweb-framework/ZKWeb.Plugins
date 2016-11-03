/*
	订单发货功能
	页面结构
	<form id='SendGoodsForm'>
		<input type='hidden' name='SendCountsJson' />
		<div class='send-goods-product-table'>
			<input type='text' data-order-product-id='订单商品Id' value='发货数' />
			...
		</div>
	</form>
	保存内容
		SendCountsJson { 订单商品Id: 发货数, ... }
	事件
		collectAll.orderSendGoods
			在页面打开或发货数修改时触发
			收集值并保存到SendCountsJson
*/

$(function () {
	// 获取元素并检查
	var $form = $("#OrderSendGoodsForm");
	var $table = $form.find(".send-goods-product-table");
	var $sendCountsJson = $form.find("[name='SendCountsJson']");
	if (!$form.length || !$table.length || !$sendCountsJson) {
		console.warn("init order send goods script failed:", $form, $table, $sendCountsJson);
		return;
	}
	// 绑定收集事件
	var collectEventName = "collectAll.orderSendGoods";
	$form.bind(collectEventName, function () {
		var sentCountMapping = {};
		var $counts = $form.find("[data-order-product-id]");
		$counts.each(function () {
			var $count = $(this);
			sentCountMapping[$count.data("order-product-id")] = $count.val();
		});
		$sendCountsJson.val(JSON.stringify(sentCountMapping));
	});
	// 发货数修改时触发收集事件
	$form.find("[data-order-product-id]").on("change", function () {
		$form.trigger(collectEventName);
	});
	// 页面打开时触发收集事件
	$form.trigger(collectEventName);
});
