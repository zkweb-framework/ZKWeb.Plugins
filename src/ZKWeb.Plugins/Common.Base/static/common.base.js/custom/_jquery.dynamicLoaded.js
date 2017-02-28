/*
	触发动态加载内容的事件
	等待元素在DOM后再触发
	例子: $.dynamicLoaded(contents);
*/

$.dynamicLoaded = function (contents) {
	var $contents = $(contents);
	var triggerDynamicLoaded = function () {
		if ($.contains(document, $contents[0])) {
			$(document).trigger("dynamicLoaded", $contents);
		} else {
			setTimeout(triggerDynamicLoaded, 1);
		}
	};
	setTimeout(triggerDynamicLoaded, 1);
};
