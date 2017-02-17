/*
	可视化编辑的核心功能
*/

$(function () {
	window.VisualEditor = {
		// 解析模板模块的Html元素
		// 返回 { path: 路径, args: 参数 }
		parseWidgetElement: function ($element) {
			var data = $element.attr("data-widget") || "";
			var index = data.indexOf("{");
			if (index >= 0) {
				return { path: data.substr(0, index), args: data.substr(index) };
			}
			return { path: data, args: null };
		}
	};
});
