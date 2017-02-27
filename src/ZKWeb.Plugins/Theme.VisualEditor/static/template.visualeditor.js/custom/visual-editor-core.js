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
				return { path: data.substr(0, index), args: JSON.parse(data.substr(index)) };
			}
			return { path: data, args: null };
		},

		// 根据模块路径获取模块信息
		getWidgetInfo: function (widgetPath) {
			return VisualEditorData.widgetPathToWidgetInfo[widgetPath] || {};
		},

		// 显示载入中
		showLoadingLayer: function () {
			$("body").append($("<div>").addClass("visual-editor-loading"));
		},

		// 隐藏载入中
		hideLoadingLayer: function () {
			$(".visual-editor-loading").remove();
		},

		// 显示添加元素的窗口
		showAddElementWindow: function (callback) {
			var $contents = $("<div>", { "class": "visual-editor-add-element" });
			var $tabContainer = $("<div>", { "class": "tabbable-line" }).appendTo($contents);
			var $tabs = $("<ul>", { "class": "nav nav-stacked" }).appendTo($tabContainer);
			var $tabContents = $("<div>", { "class": "tab-content" }).appendTo($tabContainer);
			var $clearfix = $("<div>", { "class": "clearfix" }).appendTo($tabContainer);
			$.each(VisualEditorData.widgetGroups, function (index, group) {
				var tabId = "tabWidgetGroup-" + group.Group.replace(/\s/g, "");
				var $tab = $("<li>").appendTo($tabs);
				var $tabLink = $("<a>", { "href": "#" + tabId, "data-toggle": "tab" })
					.text($.translate(group.Group)).appendTo($tab);
				var $tabPane = $("<div>", { "id": tabId, "class": "tab-pane" }).appendTo($tabContents);
				if (index == 0) {
					$tab.addClass("active");
					$tabPane.addClass("active");
				}
				$.each(group.Widgets, function (index, widget) {
					var info = widget.WidgetInfo;
					var preview = widget.WidgetInfo.Extra.Preview ||
						"/static/template.visualeditor.images/default-preview.jpg";
					var $box = $("<div>", { "class": "widget-box", "data-path": info.WidgetPath }).appendTo($tabPane);
					var $image = $("<img>", { "src": preview, "alt": "preview" }).appendTo($box);
					var $left = $("<div>", { "class": "pull-left" }).appendTo($box);
					var $right = $("<div>", { "class": "pull-right" }).appendTo($box);
					var nameText = $.translate(info.Name);
					var descriptionText = $.translate(info.Description || "NoDescription");
					var $name = $("<div>", { "class": "name", "title": nameText })
						.text(nameText).appendTo($left);
					var $description = $("<div>", { "class": "description", "title": descriptionText })
						.text(descriptionText).appendTo($left);
					var $btn = $("<div>", { "class": "btn btn-xs btn-primary" })
						.text($.translate("Add")).on("click", callback).appendTo($right);
				});
			});
			BootstrapDialog.show({
				type: "type-primary",
				size: "size-wide",
				title: $.translate("AddElement"),
				message: $contents
			});
		},

		// 显示切换页面的窗口
		showSwitchPageWindow: function () {
			BootstrapDialog.showRemote(
				$(this).text().trim(),
				"/api/visual_editor/get_switch_pages",
				{ type: "type-warning", size: "size-normal" });
		},

		// 显示编辑模块的窗口
		showEditWidgetWindow: function (path, args, done, fail) {
			// 显示载入中
			VisualEditor.showLoadingLayer();
			// 获取编辑表单, 这里需要使用post因为参数有可能很长
			fail = fail || function () { };
			$.post("/api/visual_editor/get_widget_edit_form",
				{ path: path, args: JSON.stringify(args) })
				.always(VisualEditor.hideLoadingLayer)
				.fail(fail)
				.done(function (html) {
					// 弹出编辑窗口
					var info = VisualEditor.getWidgetInfo(path);
					var $contents = $(html);
					var formSubmitted = false;
					BootstrapDialog.show({
						type: "type-primary",
						size: "size-wide",
						title: $.translate(info.Name),
						message: $contents,
						onshow: function (dialog) {
							// 触发动态加载事件
							setTimeout(function () {
								$(document).trigger("dynamicLoaded", $contents);
							}, 100);
							// 绑定表单提交后的事件
							var $form = $contents.find("form").first();
							$form.on("success", function (e, data) {
								formSubmitted = true;
								dialog.close();
								VisualEditor.showLoadingLayer();
								// 获取模块的Html
								var editedArgs = data.result;
								$.post("/api/visual_editor/get_widget_html",
									{ url: location.href, path: path, args: JSON.stringify(editedArgs) })
									.always(VisualEditor.hideLoadingLayer)
									.fail(fail)
									.done(done);
							});
						},
						onhide: function (dialog) {
							// 不提交表单就关闭时
							!formSubmitted && fail();
						}
					});
				});
		},

		// 显示删除模块的窗口
		showRemoveWidgetWindow: function (path, callback) {
			var info = VisualEditor.getWidgetInfo(path);
			BootstrapDialog.confirm({
				title: $.translate("RemoveElement"),
				message: $.translate("Are you sure to remove $element?").replace("$element", $.translate(info.name)),
				type: BootstrapDialog.TYPE_WARNING,
				btnCancelLabel: $.translate("Cancel"),
				btnOKLabel: $.translate("Ok"),
				callback: function (result) {
					result && callback();
				}
			});
		},

		// 布局更新后的处理
		layoutChanged: function () {
			// TODO: 更新数据
			// TODO: 启用保存按钮
			$.toast("layout change");
		}
	};

	$(document).on("visual-editor-loaded", function () {
		// 初始化核心功能
		VisualEditor.init();
	});
});
