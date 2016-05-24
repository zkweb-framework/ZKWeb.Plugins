/*
	商品列表页使用的功能
	商品列表页已通过静态表格和set-url-param实现了过滤商品和分页功能
	
	这个脚本只需要实现以下功能
	- 标记已选择的条件，再次点击时可以取消这个条件
	- 遇到商品搜索框提交时，合并关键字到当前的条件中而不是直接提交
	- 支持按价格过滤
*/

$(function () {
	// 标记已选择的条件，再次点击时可以取消这个条件
	var $filters = $(".product-list-filter-row [data-trigger='set-url-param']");
	var uri = new Uri(location.href);
	$filters.each(function () {
		var $filter = $(this);
		var key = $filter.data("key");
		var value = $filter.data("value");
		if (value && uri.getQueryParamValue(key) == value) {
			$filter.data("value", "").addClass("selected").append("<i class='fa fa-remove'></i>");
		}
	});
});
