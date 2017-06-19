/*
	额外的验证函数
	https://github.com/jzaefferer/jquery-validation
	因为全部引入体积比较大所以只复制需要的部分，这个文件需要在jquery.validate和jquery.validate.unobtrusive间引用
*/

/* 验证上传文件后缀 */
$.validator.addMethod("extension", function (value, element, param) {
	param = typeof param === "string" ? param.replace(/,/g, "|") : "png|jpe?g|gif";
	return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
}, $.validator.format("Please enter a value with a valid extension."));

// 验证隐藏字段
// https://stackoverflow.com/questions/9707973/validate-a-hidden-field
$.validator.setDefaults({
	ignore: ".data-ignore-val" // 默认是 :hidden
});

/* 重新启用表单验证的函数，在表单html改变后可以调用 */
$.fn.reactivateValidator = function () {
	var $form = $(this);
	$form.removeData("validator").removeData("unobtrusiveValidation");
	$.validator.unobtrusive.parse($form);
}
