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
		$form.trigger("beforeSubmit");
		beforeSubmitOrig && beforeSubmitOrig.apply(this, arguments);
	};
	// 执行成功之后回调函数
	var successOrig = options.success;
	options.success = function () {
		successOrig && successOrig.apply(this, arguments);
		$form.removeClass("loading");
		$form.trigger("success");
	};
	// 执行失败之后回调函数
	var errorOrig = options.error;
	options.error = function () {
		errorOrig && errorOrig.apply(this, arguments);
		$form.removeClass("loading");
		$form.trigger("error");
	};
	return this.ajaxForm(options);
};

/* 页面加载时自动设置ajax表单 */
$(function () {
	$("form[ajax=true]").commonAjaxForm(function (data) {
		$.toastAjaxResult(data);
	});
});
