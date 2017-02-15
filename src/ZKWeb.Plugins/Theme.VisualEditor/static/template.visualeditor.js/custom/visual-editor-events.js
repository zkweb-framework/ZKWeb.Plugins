/*
	可视化编辑的事件处理
	- 绑定顶部栏中的按钮点击事件
	- TODO
*/

$(function () {
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

	$(document).on("visual-editor-loaded", function () {
		// 绑定顶部栏中的按钮点击事件
		var $topBar = $(".visual-editor-top-bar");
		$topBar.find(".add-element").on("click", onAddElement);
		$topBar.find(".manage-theme").on("click", onManageTheme);
		$topBar.find(".switch-page").on("click", onSwitchPage);
		$topBar.find(".save-changes").on("click", onSaveChanges);
	});
});
