/*
	可视化编辑的初始化处理
*/

$(function () {
	// 显示载入中处理
	VisualEditor.showLoadingLayer();
	// Ajax获取顶部栏的Html并添加到顶部
	$.ajax({
		url: "/api/visual_editor/get_top_bar",
		cache: false,
		success: function (html) {
			var $topBar = $(html).prependTo("body");
			setTimeout(function () {
				$(document).trigger("visual-editor-loaded");
				$(document).trigger("dynamicLoaded", $topBar);
				setTimeout(function () {
					// 隐藏载入中处理
					VisualEditor.hideLoadingLayer();
				}, 250);
			}, 100);
		}
	});
});
