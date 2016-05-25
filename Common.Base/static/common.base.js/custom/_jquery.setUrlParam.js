/*
	支持点击时设置Url参数
	例子
	<a data-trigger="set-url-param" data-key="page" data-value="0"></a>
*/

$(function () {
	$(document).on("click", "[data-trigger='set-url-param']", function () {
		var $this = $(this);
		var key = $this.attr("data-key");
		var value = $this.attr("data-value");
		var uri = new Uri(location.href);
		value ? uri.replaceQueryParam(key, value) : uri.deleteQueryParam(key);
		location.href = uri.path() + uri.query();
	});
});
