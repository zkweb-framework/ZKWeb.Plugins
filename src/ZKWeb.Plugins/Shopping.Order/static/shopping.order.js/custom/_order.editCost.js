/*
	订单价格修改功能
	页面结构
	<div class="order-edit-cost-form">
		<form>
			<input type='hidden' name='OrderEditCostParametersJson' />
			<table class='order-product-price-edit-table'>
				<tr>
					<td><dl data-order-product-id='订单商品Id'></dl></td>
					<td><input class='price' value='单价' /></td>
					<td><input class='order-count' value='数量' /></td>
				</tr>
			</table>
			<table class='order-cost-edit-table'>
				<tr data-row='{ OrderPricePartType: 费用类型 }'>
					<td>费用类型的名称</td>
					<td><input class='part-delta' value='金额' /></td>
				</tr>
			</table>
			<table class='transaction-amount-edit-table'>
				<tr data-row='{ PaymentTransactionId: 交易Id }'>
					<td>交易序列号</td>
					<td><input class='transaction-amount' value='金额' /></td>
				</tr>
			</table>
		</form>
	</div>
	保存内容
		OrderEditCostParametersJson {
			OrderProductCountMapping: { 订单商品Id: 数量，删除时为0, ... },
			OrderProductUnitPriceMapping: { 订单商品Id: 单价, ... },
			OrderTotalCostCalcResult: [{ Delta: 100, Type: "ProductTotalPrice" }, ...],
			TransactionAmountMapping: { 交易Id: 金额, ...}
		}
	事件
		collectAll.orderEditCost
			跟随下面的事件触发
			收集完整的参数并保存到OrderEditCostParametersJson
		orderProductPriceChange.orderEditCost
			订单商品单价或数量改变时触发
			修改订单价格中的"商品总价"
		orderCostChange.orderEditCost
			订单费用项改变时触发
			修改"订单总价"为所有费用项的合计
			修改订单交易中第一个交易的金额到（订单总价 - 其他交易的金额）
		transactionAmountChange.orderEditCost
			订单交易金额改变时触发
*/

$(function () {
	// 获取元素并检查
	var $form = $(".order-edit-cost-form form");
	var $parametersJson = $form.find("[name='OrderEditCostParametersJson']");
	var $orderProductPriceEditTable = $form.find(".order-product-price-edit-table");
	var $orderCostEditTable = $form.find(".order-cost-edit-table");
	var $transactionAmountEditTable = $form.find(".transaction-amount-edit-table");
	var $totalCost = $(".order-edit-cost-form .total-cost");
	if (!$form.length || !$parametersJson.length || !$orderProductPriceEditTable.length ||
		!$orderCostEditTable.length || !$transactionAmountEditTable.length) {
		console.warn("init order cost editor failed:", $form, $parametersJson,
			$orderProductPriceEditTable, $orderCostEditTable, $transactionAmountEditTable);
		return;
	}

	// 事件名称
	var collectAllEventName = "collectAll.orderEditCost";
	var orderProductPriceChangeEventName = "orderProductPriceChange.orderEditCost";
	var orderCostChangeEventName = "orderCostChange.orderEditCost";
	var transactionAmountChangeEventName = "transactionAmountChange.orderEditCost";

	// 绑定收集完整参数的事件
	$form.bind(collectAllEventName, function () {
		var parameters = {
			OrderProductCountMapping: {},
			OrderProductUnitPriceMapping: {},
			OrderTotalCostCalcResult: [],
			TransactionAmountMapping: {}
		};
		// 获取订单商品数量和单价的修改
		// 商品单价使用字符串，避免parseFloat导致精度损失，服务端会反序列化到decimal
		var $orderProductRows = $orderProductPriceEditTable.find("[data-order-product-id]").closest("tr");
		$orderProductRows.each(function () {
			var $row = $(this);
			var productId = $row.find("[data-order-product-id]").data("order-product-id");
			var price = $row.find(".price").val();
			var orderCount = $row.find(".order-count").val();
			parameters.OrderProductUnitPriceMapping[productId] = price;
			parameters.OrderProductCountMapping[productId] = orderCount;
		});
		// 获取订单总金额的修改
		// 金额使用字符串，原因同上
		var $orderCostParts = $orderCostEditTable.find("[data-row]");
		$orderCostParts.each(function () {
			var $part = $(this);
			parameters.OrderTotalCostCalcResult.push({
				Delta: $part.find(".part-delta").val(),
				Type: $part.data("row").OrderPricePartType
			});
		});
		// 获取订单交易金额的修改
		// 金额使用字符串，原因同上
		var $transactionAmounts = $transactionAmountEditTable.find("[data-row]");
		$transactionAmounts.each(function () {
			var $amount = $(this);
			var transactionId = $amount.data("row").PaymentTransactionId;
			var transactionAmount = $amount.find(".transaction-amount").val();
			parameters.TransactionAmountMapping[transactionId] = transactionAmount;
		});
		$parametersJson.val(JSON.stringify(parameters));
		console.log(parameters);
	});

	// 绑定订单商品单价或数量改变时的事件
	$form.bind(orderProductPriceChangeEventName, function () {
		// 计算所有订单商品的单价*数量合计
		var $orderProductRows = $orderProductPriceEditTable.find("[data-order-product-id]").closest("tr");
		var productTotalPrice = 0;
		$orderProductRows.each(function () {
			var $row = $(this);
			var price = parseFloat($row.find(".price").val()) || 0;
			var orderCount = parseInt($row.find(".order-count").val()) || 0;
			productTotalPrice += price * orderCount;
		});
		// 修改订单价格中的"商品总价"
		var $price = $orderCostEditTable.find("[data-row]")
			.filter(function () { return $(this).data("row").OrderPricePartType == "ProductTotalPrice" })
			.find(".part-delta");
		$price.val(productTotalPrice.toFixed(2)).change();
		// 触发收集完整参数的事件
		$form.trigger(collectAllEventName);
	});
	$orderProductPriceEditTable.find(".price, .order-count").on("change", function () {
		$form.trigger(orderProductPriceChangeEventName);
	});

	// 更新订单总价，并返回订单总价的值
	var updateTotalCost = function () {
		// 计算所有费用项的合计
		var $orderCostParts = $orderCostEditTable.find("[data-row] .part-delta");
		var totalCost = 0;
		$orderCostParts.each(function () {
			totalCost += parseFloat($(this).val()) || 0;
		});
		// 修改"订单总价"的文本
		$totalCost.text(totalCost.toFixed(2));
		return totalCost;
	}
	updateTotalCost();

	// 绑定订单费用项改变时的事件
	$form.bind(orderCostChangeEventName, function () {
		// 修改订单交易中第一个交易的金额到（订单总价 - 其他交易的金额）
		var totalCost = updateTotalCost();
		var $transactionAmounts = $transactionAmountEditTable.find("[data-row] .transaction-amount");
		var restAmount = 0;
		_.each(_.tail($transactionAmounts), function () {
			restAmount += parseFloat($(this).val());
		});
		$transactionAmounts.first().val((totalCost - restAmount).toFixed(2)).change();
		// 触发收集完整参数的事件
		$form.trigger(collectAllEventName);
	});
	$orderCostEditTable.find("[data-row] .part-delta").on("change", function () {
		$form.trigger(orderCostChangeEventName);
	});

	// 绑定订单交易金额改变时的事件
	$form.bind(transactionAmountChangeEventName, function () {
		// 触发收集完整参数的事件
		$form.trigger(collectAllEventName);
	});
	$transactionAmountEditTable.find("[data-row] .transaction-amount").on("change", function () {
		$form.trigger(transactionAmountChangeEventName);
	});

	// 在页面打开时触发收集完整参数的事件
	$form.trigger(collectAllEventName);
});
