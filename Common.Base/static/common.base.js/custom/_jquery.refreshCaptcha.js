/*
	支持刷新验证码，用在form对象中
*/

$(function () {
	// 点击时自动刷新验证码
	$(document).on("click", "form .captcha", function () {
		var $captcha = $(this);
		$captcha.refreshImage();
		$captcha.closest(".input-group").find("input[type='text']").val("");
	});
	// 表单提交后自动刷新验证码
	var refresh = function () { $(this).find(".captcha").click(); };
	$(document).on("success", "form[ajax=true]", refresh);
	$(document).on("error", "form[ajax=true]", refresh);
});
