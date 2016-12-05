/*
	手机版页面的功能
*/

$(function () {
	// 自动设置标题栏
	$(".top-navbar .title").text($("title").text());

	// 侧边栏展开和收缩
	var bottomPadding = 0;
	$(document).on("click", ".top-navbar .sidebar-menu", function () {
		var $sidebar = $(".top-sidebar");
		$("html, body").animate({ scrollTop: 0 }, 100);
		if (!$sidebar.hasClass("control-sidebar-open")) {
			// 删除hide会对css效果有干扰，所以需要延迟
			$sidebar.removeClass("hide");
			setTimeout(function () {
				$sidebar.addClass("control-sidebar-open");
				$sidebar.css("min-height", $(".page-body").outerHeight() + bottomPadding + "px");
			}, 50);
		} else {
			// css效果结束后再添加hide
			$sidebar.removeClass("control-sidebar-open");
			setTimeout(function () {
				$sidebar.addClass("hide");
			}, 300);
		}
	});
});
