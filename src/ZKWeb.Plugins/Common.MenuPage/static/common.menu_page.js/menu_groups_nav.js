/* 给当前路径对应的菜单项添加active类 */
$(function () {
	var path = location.pathname + location.search;
	var $link = $(".menu_groups_nav").find("a").filter(function () {
		return $(this).attr("href") == path;
	});
	$link.closest("li").addClass("active");
	$link.closest(".collapse").addClass("in");
});
