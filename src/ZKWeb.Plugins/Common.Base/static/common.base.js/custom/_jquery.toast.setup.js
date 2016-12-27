/*
	 对jquery.toast的修改和额外处理
*/

$(function () {
	// 修改默认选项
	$.toast.options.showHideTransition = "fade";
	$.toast.options.position = "bottom-center";
	$.toast.options.icon = "info";
	// 替换换行到html换行
	$.toastOrig = $.toast;
	$.toast = function (options) {
		if ((typeof options === "string") || (options instanceof Array)) {
			options = { text: options };
		}
		options.text = (options.text || "").replace(/\r?\n/g, "<br />");
		return $.toastOrig(options);
	}
	$.toast.options = $.toastOrig.options;
	// 设置Ajax出错时自动显示提示消息
	$.toast.extra = { showAjaxError: true };
	$(document).ajaxError(function (event, jqXHR) {
		if (!jqXHR.status) {
			// 页面跳转时如果仍有处理中的请求，会生成一个状态是0的错误，不应该提示
		} else if (jqXHR.status == 200 || jqXHR.status == 304) {
			// 如果碰到304（内容无修改）会触发这个错误，不应该提示
		} else if (jqXHR.responseText && $.toast.extra.showAjaxError) {
			var text = jqXHR.responseText.replace(/<[^>]+>/g, ""); // 过滤html标签
			text = _.escape(text.split("\n").slice(0, 7).join("\n")); // 只显示前面的行
			$.toast({ icon: "error", text: text });
		}
	});
});
