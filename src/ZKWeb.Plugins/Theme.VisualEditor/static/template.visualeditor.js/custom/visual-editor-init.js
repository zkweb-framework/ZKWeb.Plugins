/*
	可视化编辑的初始化处理
*/

$(function () {
	// 显示载入中处理
	$("body").append($("<div>").addClass("visual-editor-loading"));
	// Ajax获取顶部栏的Html并添加到顶部
	$.ajax({
		url: "/api/visual_editor/get_top_bar",
		cache: false,
		success: function (html) {
			$("body").prepend(html);
			setTimeout(function () {
				$(document).trigger("visual-editor-loaded");
				setTimeout(function () {
					// 隐藏载入中处理
					$(".visual-editor-loading").remove();
				}, 250);
			}, 100);
		}
	});
});
