/*
	数字文本框的支持
	使用需要定义以下结构的Html
	<div data-toggle='number-input'
		data-allow-decimal='false' data-allow-negative='false' data-delta='1'>
		<input type='text' class='number' />
		<span class='up'></span>
		<span class='down'></span>
	</div>
*/

$(function () {
	// 点击上下按钮时影响数量
	var numberInputSelector = "[data-toggle='number-input']";
	var onUpDownClick = function () {
		var $this = $(this);
		var $numberInput = $this.closest(numberInputSelector);
		var $number = $numberInput.find(".number");
		var delta = (parseFloat($numberInput.data('delta')) || 1) * ($this.is(".down") ? -1 : 1);
		var newValue = (parseFloat($number.val()) || 0) + delta;
		if (!$numberInput.data("allow-negative") && newValue < 0) {
			newValue = 0; // 不允许负数时最小只能等于0
		}
		$number.val(newValue).change().blur(); // blur用于触发表单验证
	};
	$(document).on("click", numberInputSelector + " .up", onUpDownClick);
	$(document).on("click", numberInputSelector + " .down", onUpDownClick);
	// 阻止填写非数字内容
	var onNumberChange = function (inputting) {
		var $this = $(this);
		var $numberInput = $this.closest(numberInputSelector);
		var allowDecimal = $numberInput.data("allow-decimal");
		var allowNegative = $numberInput.data("allow-negative");
		var value = $(this).val() + "";
		var regex = inputting ? /^\-?\d*\.?\d*$/ : /^\-?\d+(\.?\d+)?$/;
		if (!value.match(regex) ||
			(!allowNegative && _.contains(value, '-')) ||
			(!allowDecimal && _.contains(value, '.'))) {
			var number = (allowDecimal ? parseFloat(value) : parseInt(value)) || 0;
			$this.val(allowNegative ? number : Math.abs(number)); // 数值不符合要求时重置
		}
	}
	$(document).on("keyup", numberInputSelector + " .number", function () {
		onNumberChange.call(this, true);
	});
	$(document).on("change", numberInputSelector + " .number", function () {
		onNumberChange.call(this);
	});
	$(document).on("blur", numberInputSelector + " .number", function () {
		onNumberChange.call(this);
	});
});
