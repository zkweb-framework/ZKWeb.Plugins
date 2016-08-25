/* 全屏portlet的功能，已重写 */

$(function () {
	// 支持全屏设置
	var $body = $("body");
	$body.on("click", ".portlet a.fullscreen", function (e) {
		var $portlet = $(this).closest(".portlet");
		if ($portlet.hasClass("portlet-fullscreen")) {
			$portlet.removeClass("portlet-fullscreen");
			$body.removeClass("page-portlet-fullscreen");
		} else {
			$portlet.addClass("portlet-fullscreen");
			$body.addClass("page-portlet-fullscreen");
		}
	});
});
