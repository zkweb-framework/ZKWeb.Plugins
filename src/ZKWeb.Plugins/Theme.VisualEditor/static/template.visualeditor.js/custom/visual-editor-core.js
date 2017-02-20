/*
	可视化编辑的核心功能
*/

$(function () {
	// 可视化编辑数据
	window.VisualEditorData = {
		// 模块分组列表
		widgetGroups: [],
		// { 模块路径: 模块信息 }
		widgetPathToWidgetInfo: {}
	};

	// 可视化编辑核心功能
	window.VisualEditor = {
		// 初始化核心功能
		init: function () {
			// 获取模块分组列表
			VisualEditorData.widgetGroups = (
				$(".visual-editor-top-bar .metadata .widget-groups").data("widget-groups") || []);
			// 生成模块路径到模块信息的索引
			$.each(VisualEditorData.widgetGroups, function (index, group) {
				$.each(group.Widgets, function (index, widget) {
					VisualEditorData.widgetPathToWidgetInfo[widget.WidgetInfo.WidgetPath] = widget.WidgetInfo;
				});
			});
		},

		// 解析模板模块的Html元素
		// 返回 { path: 路径, args: 参数 }
		parseWidgetElement: function ($element) {
			var data = $element.attr("data-widget") || "";
			var index = data.indexOf("{");
			if (index >= 0) {
				return { path: data.substr(0, index), args: data.substr(index) };
			}
			return { path: data, args: null };
		},

		// 根据模块路径获取模块信息
		getWidgetInfo: function (widgetPath) {
			return VisualEditorData.widgetPathToWidgetInfo[widgetPath] || {};
		}
	};

	$(document).on("visual-editor-loaded", function () {
		// 初始化核心功能
		VisualEditor.init();
	});
});
