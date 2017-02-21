/*
	可视化编辑的事件处理
*/

$(function () {
	// 阻止原有事件发生
	var preventEvent = function (e) {
		e.preventDefault && e.preventDefault();
		return false;
	}

	// 添加元素弹出框中对指定元素点击"添加"时的事件
	var onAddElementBtnClicked = function () {
		// 获取模块信息
		var $this = $(this);
		var path = $this.closest(".widget-box").attr("data-path");
		var info = VisualEditor.getWidgetInfo(path);
		// 关闭模态框
		$this.closest(".bootstrap-dialog").modal("hide");
		// 让用户选择区域
		var $areas = $(".template_area");
		$areas.addClass("select-area").on("click", function () {
			var $area = $(this);
			// TODO: 弹出编辑框
			// 添加到区域中
			console.log("add widget");
			$("<div>", { "class": "template_widget", "data-widget": path })
				.text("TODO").appendTo($area);
			$areas.off("click").removeClass("select-area");
			// 通知布局更新
			VisualEditor.onLayoutChange();
		});
	};

	// 点击"添加元素"时的事件
	var onAddElement = function () {
		var $content = $("<div>", { "class": "visual-editor-add-element" });
		var $tabContainer = $("<div>", { "class": "tabbable-line" }).appendTo($content);
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
					.text($.translate("Add")).on("click", onAddElementBtnClicked).appendTo($right);
			});
		});
		BootstrapDialog.show({
			type: "type-primary",
			size: "size-wide",
			title: $.translate("AddElement"),
			message: $content
		});
	};

	// 点击"管理主题"时的事件
	var onManageTheme = function () {
		$.toast("TODO");
	};

	// 点击"切换页面"时的事件
	var onSwitchPage = function () {
		BootstrapDialog.showRemote(
			$(this).text().trim(),
			"/api/visual_editor/get_switch_pages",
			{ type: "type-warning", size: "size-normal" });
	};

	// 点击"保存修改"时的事件
	var onSaveChanges = function () {
		$.toast("TODO");
	};

	// 窗口关闭时的事件
	// 浏览器会提示是否确认离开, ie会显示返回的信息, ff和chrome不会显示
	var onWindowClose = function () {
		return $.translate("Make sure you have saved all the changes, otherwise they will be lost.");
	};

	// 点击模块标题栏中的"编辑"时的事件
	var onTemplateWidgetEditClicked = function () {
		$.toast("TODO");
	};

	// 点击模块标题栏中的"删除"时的事件
	var onTemplateWidgetRemoveClicked = function () {
		$.toast("TODO");
	};

	// 模板模块鼠标进入时的处理
	var onTemplateWidgetMouseEnter = function () {
		var $this = $(this);
		// 获取模块信息
		var data = VisualEditor.parseWidgetElement($this);
		var info = VisualEditor.getWidgetInfo(data.path);
		// 添加标题栏
		$this.find(".template-widget-title-bar").remove();
		var $titleBar = $("<div>").addClass("template-widget-title-bar");
		$titleBar.append($("<span>").addClass("name").text($.translate(info.Name)));
		$titleBar.append($("<span>").addClass("remove")
			.text("[" + $.translate("Remove") + "]")
			.on("click", onTemplateWidgetRemoveClicked));
		$titleBar.append($("<span>").addClass("edit")
			.text("[" + $.translate("Edit") + "]")
			.on("click", onTemplateWidgetEditClicked));
		$titleBar.prependTo($this);
	};

	// 模板模块鼠标离开时的处理
	var onTemplateWidgetMouseLeave = function () {
		var $this = $(this);
		// 移除标题栏
		$this.find(".template-widget-title-bar").remove();
	};

	// 拖动停止后的处理
	var onSortStopped = function (e, data) {
		// 移除标题栏
		onTemplateWidgetMouseLeave.call($(data.item));
	};

	// 拖动停止后且DOM更新后的处理
	var onSortUpdated = function () {
		// 通知布局更新
		VisualEditor.onLayoutChange();
	};

	$(document).on("visual-editor-loaded", function () {
		// 阻止点击原有页面的元素
		var $allElements = $("body *:not(.visual-editor-top-bar):not(.modal) *");
		$allElements.unbind("click").unbind("mouseenter").unbind("mouseleave");
		$allElements.bind("click", preventEvent);
		// 绑定顶部栏中的按钮点击事件
		var $topBar = $(".visual-editor-top-bar");
		$topBar.find(".add-element").on("click", onAddElement);
		$topBar.find(".manage-theme").on("click", onManageTheme);
		$topBar.find(".switch-page").on("click", onSwitchPage);
		$topBar.find(".save-changes").on("click", onSaveChanges);
		// 绑定窗口关闭时的事件, 开发时可注释以下行
		// $(window).on("beforeunload", onWindowClose);
		// 绑定模板模块的鼠标进入和离开事件
		$(document).on("mouseenter", ".template_widget", onTemplateWidgetMouseEnter);
		$(document).on("mouseleave", ".template_widget", onTemplateWidgetMouseLeave);
		// 支持拖动模块
		// TODO: 修复拖动后标题栏残留的问题
		$(".template_area").sortable({
			connectWith: ".template_area",
			cursor: "move",
			forceHelperSize: true,
			forcePlaceholderSize: true,
			handle: ".template-widget-title-bar",
			tolerance: "pointer",
			zIndex: 10050,
			stop: onSortStopped,
			update: onSortUpdated
		}).disableSelection();
	});
});
