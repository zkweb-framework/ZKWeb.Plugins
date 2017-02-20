/*
	可视化编辑的事件处理
*/

$(function () {
	// 阻止原有事件发生
	var preventEvent = function (e) {
		e.preventDefault && e.preventDefault();
		return false;
	}

	// 点击"添加元素"时的事件
	var onAddElement = function () {
		$.toast("TODO");
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
		$titleBar.prependTo($this);
	};

	// 模板模块鼠标离开时的处理
	var onTemplateWidgetMouseLeave = function () {
		var $this = $(this);
		// 移除标题栏
		$this.find(".template-widget-title-bar").remove();
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
			zIndex: 10050
		}).disableSelection();
	});
});
