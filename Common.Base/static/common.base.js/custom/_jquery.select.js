/*
	下拉框相关的功能
	- 支持搜索
	- 可编辑文本
*/

$(function () {
	// 按关键词搜索
	var advanceSelectSelector = "[data-toggle='advance-select']";
	$(document).on("keyup", advanceSelectSelector + " .keyword", function () {
		var $this = $(this);
		var keyword = $this.val().trim();
		$this.closest(advanceSelectSelector).find("option").each(function () {
			var $option = $(this);
			($option.text().indexOf(keyword) >= 0) ? $option.show() : $option.hide();
		})
	});
	// 选择项改变后应用到按钮或文本框
	$(document).on("change", advanceSelectSelector + " select", function () {
		var $this = $(this);
		var selectedText = $this.find("option:selected").text().trim();
		var $advanceSelect = $this.closest(advanceSelectSelector);
		$advanceSelect.find(".option-text").text(selectedText);
		$advanceSelect.find(".option-text-editable").val(selectedText).change();
	});
});
