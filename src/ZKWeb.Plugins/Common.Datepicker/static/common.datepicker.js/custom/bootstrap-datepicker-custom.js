/* 支持选择时间和选择时间范围的表单控件 */

$(function () {
	// 初始化时间选择控件
	var setupDatePicker = function ($element) {
		var language = $("html").attr("lang");
		var format = $element.data("date-format").toLowerCase();
		var $input = $element.find("input[type=text]");
		$input.datepicker({ language: language, format: format });
	};

	// 自动初始化[data-trigger=date-picker]
	var rule = "[data-trigger=date-picker]";
	$(rule).each(function () { setupDatePicker($(this)); });
	$(document).on("dynamicLoaded", function (e, contents) {
		$(contents).find(rule).each(function () { setupDatePicker($(this)); });
	});
});

$(function () {
	// 初始化时间范围选择控件
	var setupDateRangePicker = function ($element) {
		var language = $("html").attr("lang");
		var format = $element.data("date-format").toLowerCase();
		var $json = $element.find("input[type=hidden]");
		var $begin = $element.find("input.begin");
		var $finish = $element.find("input.finish");
		// 绑定值
		var dateRange = JSON.parse($json.val() || {});
		$begin.val(dateRange.Begin);
		$finish.val(dateRange.Finish);
		// 初始化datepicker
		$element.datepicker({
			language: language,
			format: format,
			inputs: [$begin, $finish]
		});
		// 绑定改变时间
		var onDateRangeChange = function () { $json.val(JSON.stringify(dateRange)).trigger("change"); };
		$begin.on("change", function () { dateRange.Begin = $begin.val(); onDateRangeChange(); });
		$finish.on("change", function () { dateRange.Finish = $finish.val(); onDateRangeChange(); });
	};

	// 自动初始化[data-trigger=date-range-picker]
	var rule = "[data-trigger=date-range-picker]";
	$(rule).each(function () { setupDateRangePicker($(this)); });
	$(document).on("dynamicLoaded", function (e, contents) {
		$(contents).find(rule).each(function () { setupDateRangePicker($(this)); });
	});
});
