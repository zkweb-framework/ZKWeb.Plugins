/*
	对switchery的jquery包装
*/

$.fn.Switchery = function (config) {
	$(this).each(function (_, elem) {
		// 已设置时跳过
		if (elem.attributes["data-switchery"]) {
			return;
		}
		// 设置ios样式的勾选框
		new Switchery(elem, config);
	});
};

/* 加载页面时自动设置表单中，css类带switchery的勾选框 */
$(function () {
	var rule = "input.switchery[type=checkbox]";
	$(rule).Switchery();
	$(document).on("dynamicLoaded", function (e, contents) {
		$(contents).find(rule).Switchery();
	});
});
