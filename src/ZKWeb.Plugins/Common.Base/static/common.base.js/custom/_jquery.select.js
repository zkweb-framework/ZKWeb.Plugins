/*
	下拉框相关的功能
	- 支持搜索
	- 可编辑文本
*/

$(function () {
	// 按关键词搜索
	var advanceSelectSelector = "[data-toggle='advance-select']";
	var advanceSelectKeywordSelector = advanceSelectSelector + " .keyword";
	$(document).on("keyup", advanceSelectKeywordSelector, function () {
		var $this = $(this);
		var keyword = $this.val().trim().toLowerCase();
		$this.closest(advanceSelectSelector).find("option").each(function () {
			var $option = $(this);
			($option.text().toLowerCase().indexOf(keyword) >= 0) ? $option.show() : $option.hide();
		})
	});

	// 选择项改变后应用到按钮或文本框
	var advanceSelectFieldSelector = advanceSelectSelector + " select";
	$(document).on("change", advanceSelectFieldSelector, function () {
		var $this = $(this);
		var selectedText = $this.find("option:selected").text().trim();
		var $advanceSelect = $this.closest(advanceSelectSelector);
		$advanceSelect.find(".option-text").text(selectedText);
		$advanceSelect.find(".option-text-editable").val(selectedText).change();
	});

	// 页面载入时初始化高级下拉框
	var setup = function ($elements) {
		$elements.each(function () {
			var $this = $(this);
			var size = $this.closest(advanceSelectSelector).data("select-size") || 5;
			$this.attr("size", "5");
			$this.change();
		});
	};
	setup($(advanceSelectFieldSelector));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(advanceSelectFieldSelector));
	});
});
