/*
	对jquery-validation-unobtrusive的额外设置
	https://github.com/aspnet/jquery-validation-unobtrusive/blob/master/src/jquery.validate.unobtrusive.js
	需要在jquery.validate.unobtrusive后引用
*/

// 验证失败时跳转到字段所在标签, 并提示信息
// https://jqueryvalidation.org/validate/
$.validator.unobtrusive.options = {
	invalidHandler: function (event, validator) {
		var errorList = validator.errorList || [];
		if (!errorList.length) {
			return;
		}
		// 显示提示信息
		var messages = [];
		$.each(errorList, function (_, v) {
			messages.push($("<div>").text(v.message).html());
		});
		$.toast({ icon: "error", text: messages.join("<br/>") });
		// 跳到第一个错误所在的标签
		var $element = $(errorList[0].element);
		var tabId = $element.closest(".tab-pane").attr("id");
		if (tabId) {
			var $tab = $("a[href='#" + tabId + "']");
			$tab.length && $tab.tab("show");
		}
	}
};
