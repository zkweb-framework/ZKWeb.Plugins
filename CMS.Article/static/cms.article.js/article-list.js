/*
	文章列表页使用的脚本
	提供以下的功能
		支持搜索框
*/

/* 支持搜索框 */
$(function () {
	var $searchBar = $(".article-search-bar");
	var $keyword = $searchBar.find(".keyword");
	var uri = new Uri(location.href);
	$keyword.val(uri.getQueryParamValue("keyword"));
	var submit = function () {
		uri.replaceQueryParam("keyword", $keyword.val());
		uri.deleteQueryParam("page");
		location.href = uri.path() + uri.query();
	};
	$searchBar.find(".search-button").on("click", submit);
	$keyword.on("keydown", function (e) { e.keyCode == 13 && submit(); });
});
