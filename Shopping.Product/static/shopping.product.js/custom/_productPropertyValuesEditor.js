/*
	商品属性值的编辑器
	编辑商品属性时使用
*/

// 商品属性值的编辑器
// 例 $("#Editor").productPropertyValuesEditor();
// 元素需要有以下属性
//	data-property-values-name 储存属性值列表的Json的字段名称
// 元素可以有以下属性
//	data-translations 翻译文本
$.fn.productPropertyValuesEditor = function () {
	var $editor = $(this);
	// 避免重复初始化
	if ($editor.data("initialized")) {
		return;
	}
	$editor.data("initialized", true);

	console.log($editor);
};

// 自动初始化带[data-toggle='product-property-values-editor']属性的编辑器
$(function () {
	var setup = function ($elements) {
		$elements.each(function () { $(this).productPropertyValuesEditor(); });
	};
	var rule = "[data-toggle='product-property-values-editor']";
	setup($(rule));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(rule));
	});
});
