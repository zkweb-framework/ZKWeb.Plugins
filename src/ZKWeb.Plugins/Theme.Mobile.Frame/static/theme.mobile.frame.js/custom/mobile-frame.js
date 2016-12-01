/*
	手机版页面的功能
*/

$(function () {
	// 自动设置标题栏
	$(".top-navbar .title").text($("title").text());

	// 侧边栏展开和收缩
	var bottomPadding = 20;
	$(document).on("click", ".top-navbar .sidebar-menu", function () {
		var $sidebar = $(".top-sidebar");
		$("html, body").animate({ scrollTop: 0 }, 100);
		if (!$sidebar.hasClass("control-sidebar-open")) {
			$sidebar.addClass("control-sidebar-open");
			$sidebar.css("min-height", $(".page-body").outerHeight() + bottomPadding + "px");
		} else {
			$sidebar.removeClass("control-sidebar-open");
		}
	});
});
