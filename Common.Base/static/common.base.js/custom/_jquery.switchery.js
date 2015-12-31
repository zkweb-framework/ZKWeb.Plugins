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

$(function () {
	// 加载页面时自动设置表单中，css类带switchery的勾选框
	$("form input.switchery[type=checkbox]").Switchery();
});
