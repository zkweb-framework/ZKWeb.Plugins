/*
	手机版页面的功能 (菜单页)
*/

$(function () {
	// 如果当前的url的menu-opened参数等于true则只显示菜单
	var uri = new Uri(location.href);
	if (uri.getQueryParamValue("menu-opened") == "true") {
		$("body").addClass("menu-page-menu-opened");
	}

	// 为标记了menupage-index的链接自动添加menu-opened
	$("a.menupage-index").each(function () {
		var $a = $(this);
		var uri = new Uri($a.attr("href"));
		$a.attr("href", uri.replaceQueryParam("menu-opened", "true").toString());
	});
});
