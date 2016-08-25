/*
	支持延迟加载样式
	例子
	<div require-style="/statis/demo/demo.css"></div>
	
	注意：
	每个路径最多只会加载一次
*/

$(function () {
	var loadedStyles = []; // 已加载的样式
	var loadRequiredStyles = function (rootElement) {
		$(rootElement).find("[require-style]").each(function () {
			var style = $(this).attr("require-style");
			if (_.contains(loadedStyles, style)) {
				return; // 防止重复加载
			}
			loadedStyles.push(style);
			$("<link/>", { rel: "stylesheet", type: "text/css", href: style }).appendTo("head");
		});
	};
	loadRequiredStyles(document);
	$(document).on("dynamicLoaded", function (e, contents) { loadRequiredStyles(contents); });
});
