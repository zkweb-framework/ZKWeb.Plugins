/*
	用于加载CKEditor和初始化CKEditor控件

	例子
	<textarea class="ckeditor" data-ckeditor-config="null"></textarea>
*/

$(function () {
	// 加载ckeditor的脚本
	window.CKEDITOR_BASEPATH = "/static/ckeditor/";
	$.getScript("/static/ckeditor/ckeditor.js", function () {
		// 初始化ckeditor控件
		var initCKEditor = function (element) {
			var $element = $(element);
			// 防止重复初始化
			var instanceKey = "ckeditorInstance";
			var instance = $element.data(instanceKey);
			if (instance) {
				return;
			}
			// 生成ckeditor实例
			instance = CKEDITOR.replace($element[0], $element.data("ckeditor-config"));
			$element.data(instanceKey, instance);
			// 表单提交前更新控件
			// form-pre-serialize是ajaxform用的事件
			var $form = $element.closest("form");
			$form.on("submit", function () { instance.updateElement(); });
			$form.on("form-pre-serialize", function () { instance.updateElement(); });
			// 防止ckeditor焦点无法捕捉的BUG
			$("[tabindex]").removeAttr("tabindex");
		};
		var rule = "[data-trigger=ckeditor]";
		$(rule).each(function () { initCKEditor(this); });
		$(document).on("dynamicLoaded", function (e, contents) {
			$(contents).find(rule).each(function () { initCKEditor(this); });
		});
	});
});
