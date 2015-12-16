/*
	 对jquery.toast的修改和额外处理
*/

$(function () {
	// 修改默认选项
	$.toast.options.showHideTransition = "slide";
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
		if (jqXHR.responseText && $.toast.extra.showAjaxError) {
			var text = jqXHR.responseText.replace(/<[^>]+>/g, ""); // 过滤html标签
			text = _.escape(text.split("\n").slice(0, 7).join("\n")); // 只显示前面的行
			$.toast({ icon: "error", text: text, hideAfter: false });
		}
	});
});

// 显示Ajax操作结果的消息
// 允许以下类型的参数
// { success: true, message: "成功时的消息" }
// { success: false, reason: "失败时的原因" }
// { success: true, message: "允许使用Html文本时", allowHtmlText: true }
$.toastAjaxResult = function (data) {
	if (data.success) {
		var text = data.allowHtmlText ? data.message : _.escape(data.message);
		text && $.toast({ icon: "success", text: text });
	} else {
		var text = data.allowHtmlText ? data.reason : _.escape(data.reason);
		$.toast({ icon: "error", text: text });
	}
};
