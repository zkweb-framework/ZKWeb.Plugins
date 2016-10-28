/*
	订单详情页的功能
*/

/* 弹出框的工具按钮 */
$(function () {
	$(document).on("click", ".order-details .show-prompt", function () {
		var title = $(this).attr("data-title");
		var body = $(this).attr("data-body");
		prompt(title, body);
	});
});

/* 跳转到标签的链接 */
$(function () {
	$(document).on("click", ".order-details .show-tab", function () {
		var tab = $(this).attr("data-tab");
		$(".nav-tabs [key='" + tab + "']").tab("show");
	});
});
