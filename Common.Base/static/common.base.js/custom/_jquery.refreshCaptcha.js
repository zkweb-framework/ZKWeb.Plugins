/*
	支持刷新验证码，用在form对象中
*/

$.fn.refreshCaptcha = function() {
	var $captcha = $(this).find(".captcha");
	$captcha.click();
	$captcha.closest(".input-captcha").find("input[type='text']").val("");
};