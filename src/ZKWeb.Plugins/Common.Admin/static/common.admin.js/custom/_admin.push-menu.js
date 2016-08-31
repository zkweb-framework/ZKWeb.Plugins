/* 点击展开和收缩侧边栏的功能 */

$(function () {
	$(document).on("click", "[data-toggle='offcanvas']", function (e) {
		e.preventDefault();
		var $window = $(window);
		var $body = $("body");
		if ($window.width() >= 768) {
			if ($body.hasClass("sidebar-collapse")) {
				// 显示侧边栏
				$body.removeClass("sidebar-collapse").trigger("expanded.pushMenu");
			} else {
				// 隐藏侧边栏
				$body.addClass("sidebar-collapse").trigger("collapsed.pushMenu");
			}
		} else {
			if ($body.hasClass("sidebar-open")) {
				$body.removeClass("sidebar-open").removeClass("sidebar-collapse").trigger("collapsed.pushMenu");
			} else {
				$body.addClass("sidebar-open").trigger("expanded.pushMenu");
			}
		}
	});
});
