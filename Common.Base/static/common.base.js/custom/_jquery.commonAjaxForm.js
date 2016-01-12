/*
	通用的Ajax表单
	$.ajaxForm的包装函数
	在提交期间为对象设置loading类
	自动触发以下事件
		beforeSubmit 提交前的处理
		success 提交成功时的处理
		error 提交失败时的处理
*/

$.fn.commonAjaxForm = function (options) {
	var $form = $(this);
	// 重新启用表单验证
	$form.reactivateValidator();
	// 识别传入的参数类型
	options = options || {};
	if (typeof options == "function") {
		options = { success: options }
	}
	// 提交之前执行函数
	var beforeSubmitOrig = options.beforeSubmit;
	options.beforeSubmit = function () {
		$form.addClass("loading");
		beforeSubmitOrig && beforeSubmitOrig.apply(this, arguments);
		$form.trigger("beforeSubmit", arguments);
	};
	// 执行成功之后回调函数
	var successOrig = options.success;
	options.success = function () {
		successOrig && successOrig.apply(this, arguments);
		$form.trigger("success", arguments);
		$form.removeClass("loading");
	};
	// 执行失败之后回调函数
	var errorOrig = options.error;
	options.error = function () {
		errorOrig && errorOrig.apply(this, arguments);
		$form.trigger("error", arguments);
		$form.removeClass("loading");
	};
	return this.ajaxForm(options);
};

/* 处理提交结果 */
$.handleAjaxResult = function (data) {
	// 支持的属性
	//	message 显示消息
	//	allowHtmlText 允许显示html消息
	//	script 执行的脚本
	// 显示消息
	var text = data.allowHtmlText ? data.message : _.escape(data.message);
	text && $.toast({ icon: "success", text: text });
	// 执行脚本
	data.script && eval(data.script);
}

/* 页面加载时自动设置ajax表单 */
$(function () {
	var setup = function ($elem) {
		$elem.commonAjaxForm(function (data) { $.handleAjaxResult.call($elem, data); });
	};
	var rule = "form[ajax=true]";
	setup($(rule));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(rule));
	});
});
