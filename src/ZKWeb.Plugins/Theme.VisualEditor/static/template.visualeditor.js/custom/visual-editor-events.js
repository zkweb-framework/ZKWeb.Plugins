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
		// 关闭模态框
		$this.closest(".bootstrap-dialog").modal("hide");
		// 禁用"添加元素"按钮
		VisualEditor.switchAddElementBtn(false);
		// 让用户选择区域
		var $areas = $(".template_area");
		$areas.addClass("select-area").on("click", function () {
			var $area = $(this);
			// 完成后清除选择样式并恢复"添加元素"按钮
			var cleanup = function () {
				$areas.off("click").removeClass("select-area");
				VisualEditor.switchAddElementBtn(true);
			};
			// 弹出编辑窗口
			VisualEditor.showEditWidgetWindow(path, {}, function (html) {
				// 添加到区域中并触发动态加载事件
				var $widget = $(html).appendTo($area);
				$.dynamicLoaded($widget);
				$.toast({ icon: "success", text: $.translate("Add Element Success") });
				// 执行清理工作
				cleanup();
				// 通知布局更新
				VisualEditor.layoutChanged();
			}, function () {
				// 执行清理工作
				cleanup();
			});
		});
	};

	// 点击"添加元素"时的事件
	var onAddElement = function () {
		VisualEditor.showAddElementWindow(onAddElementBtnClicked);
	};

	// 点击"管理主题"时的事件
	var onManageTheme = function () {
		VisualEditor.showManageThemeWindow(function () {
			var $btn = $(this);
			var $theme = $btn.closest(".theme");
			var isBackupTheme = $theme.is(".backup-theme");
			var name = $theme.find(".name").text().trim();
			var filename = $theme.data("filename");
			if ($btn.is(".download-theme")) {
				// 下载主题
				VisualEditor.downloadTheme(filename, isBackupTheme);
			} else if ($btn.is(".apply-theme")) {
				// 应用主题
				VisualEditor.applyTheme.call($btn, name, filename, isBackupTheme);
			} else if ($btn.is(".delete-theme")) {
				// 删除主题
				VisualEditor.deleteTheme.call($btn, name, filename, isBackupTheme);
			}
		});
	};

	// 点击"切换页面"时的事件
	var onSwitchPage = function () {
		VisualEditor.showSwitchPageWindow();
	};

	// 点击"保存修改"时的事件
	var onSaveChanges = function () {
		VisualEditor.saveChanges();
	};

	// 窗口关闭时的事件
	// 浏览器会提示是否确认离开, ie会显示返回的信息, ff和chrome不会显示
	var onWindowClose = function () {
		return $.translate("Make sure you have saved all the changes, otherwise they will be lost.");
	};

	// 点击模块标题栏中的"编辑"时的事件
	var onTemplateWidgetEditClicked = function () {
		// 获取模块信息
		var $widget = $(this).closest(".template_widget");
		var data = VisualEditor.parseWidgetElement($widget);
		// 弹出编辑窗口
		VisualEditor.showEditWidgetWindow(data.path, data.args, function (html) {
			// 替换原有的模块并触发动态加载事件
			var $newWidget = $(html);
			$widget.replaceWith($newWidget);
			$.dynamicLoaded($newWidget);
			$.toast({ icon: "success", text: $.translate("Edit Element Success") });
			// 通知布局更新
			VisualEditor.layoutChanged();
		});
	};

	// 点击模块标题栏中的"删除"时的事件
	var onTemplateWidgetRemoveClicked = function () {
		// 获取模块信息
		var $widget = $(this).closest(".template_widget");
		var data = VisualEditor.parseWidgetElement($widget);
		// 确认是否删除
		VisualEditor.showRemoveWidgetWindow(data.path, function () {
			// 从DOM删除
			$widget.remove();
			$.toast({ icon: "success", text: $.translate("Remove Element Success") });
			// 通知布局更新
			VisualEditor.layoutChanged();
		});
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

	// 拖动开始时的处理
	var onSortStarted = function (e, data) {
		var $item = $(data.item);
		var width = $item.width();
		var height = $item.height();
		$(".ui-sortable-placeholder").css({
			width: width,
			height: height,
		});
	}

	// 拖动停止后的处理
	var onSortStopped = function (e, data) {
		// 移除标题栏
		onTemplateWidgetMouseLeave.call($(data.item));
	};

	// 拖动停止后且DOM更新后的处理
	var onSortUpdated = function () {
		// 通知布局更新
		VisualEditor.layoutChanged();
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
		// 设置转到页面的Url
		$topBar.find(".goto-page")
			.attr("href", location.href.replace("/visual_editor/", "/"))
			.attr("target", "_blank")
			.unbind("click");
		// 绑定窗口关闭时的事件, 开发时可注释以下行
		$(window).on("beforeunload", onWindowClose);
		// 绑定模板模块的鼠标进入和离开事件
		$(document).on("mouseenter", ".template_widget", onTemplateWidgetMouseEnter);
		$(document).on("mouseleave", ".template_widget", onTemplateWidgetMouseLeave);
		// 支持拖动模块
		$(".template_area").sortable({
			connectWith: ".template_area",
			cursor: "move",
			forceHelperSinameze: true,
			forcePlaceholderSize: true,
			handle: ".template-widget-title-bar",
			tolerance: "pointer",
			zIndex: 10050,
			start: onSortStarted,
			stop: onSortStopped,
			update: onSortUpdated
		}).disableSelection();
	});
});
