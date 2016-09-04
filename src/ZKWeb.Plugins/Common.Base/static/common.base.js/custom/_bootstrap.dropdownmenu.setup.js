/*
	对bootstrap-dropdownmenu的修改和额外处理
*/

$(function () {
	// 对于下拉菜单中的表单，只有在点击提交按钮时才隐藏
	// 测试hide.bs.dropdown事件没有作用，只能通过绑定click实现
	$(document).on("click", ".dropdown-menu .form", function (e) {
		if (!$(e.target).closest("[type='submit']").length)
			e.stopPropagation();
	});
	// 对于下拉菜单中的高级多选框，只有在点击选项时才隐藏
	$(document).on("click", ".advance-select .dropdown-menu", function (e) {
		if (!$(e.target).closest("select").length)
			e.stopPropagation();
	});
	// 下拉菜单显示在屏幕外时，调整菜单的位置
	// bootstrap-dropdown使用shown事件，bootstrap-hover-dropdown使用show事件
	function onDropdownMenuShow() {
		// 获取菜单，找不到或未显示时返回
		// 由bootstrap-hover-dropdown触发时会触发链接而不是链接的上一级元素
		var $this = $(this);
		var $menu = $this.find(".dropdown-menu").first();
		if (!$menu.length && $this.attr("data-toggle") == "dropdown") {
			$menu = $this.parent().find(".dropdown-menu").first();
		}
		if (!$menu.length || !$menu.is(":visible")) {
			return;
		}
		// 超过屏幕左边时
		var menuLeft = $menu.offset().left;
		if (menuLeft < 0) {
			var marginRight = menuLeft - 15;
			$menu.css("margin-left", "0px").css("margin-right", marginRight + "px");
		}
		// 超过屏幕右边时
		var $document = $(document);
		var documentRight = $(document).width() + $(document).scrollLeft();
		var menuRight = $menu.offset().left + $menu.outerWidth();
		if (menuRight > documentRight) {
			var marginLeft = documentRight - menuRight - 15;
			$menu.css("margin-right", "0px").css("margin-left", marginLeft + "px");
		}
	}
	$(document).on("show.bs.dropdown", "*", function () { onDropdownMenuShow.call(this) });
	$(document).on("shown.bs.dropdown", "*", function () { onDropdownMenuShow.call(this) });
});