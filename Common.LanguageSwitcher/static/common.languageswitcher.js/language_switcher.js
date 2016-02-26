$(function () {
	// 切换语言的菜单项点击后，提交切换请求然后刷新当前页面
	$(".language-switcher").on("click", function () {
		var language = $(this).attr("language");
		$.post("/api/switch_to_language", { language: language }).success(function () {
			location.href = location.href;
		});
	});
});
