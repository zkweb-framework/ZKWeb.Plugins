/*
	可视化编辑的事件处理
	- 绑定顶部栏中的按钮点击事件
	- 绑定窗口关闭时的事件
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
		return $(".visual-editor-messages .confirm-close").text();
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
	});
});
