/* 用于支持AdminLTE的treemenu菜单 */
$(function () {
	$(document).on("click", ".treeview-toggle", function (e) {
		// 获取下一个元素
		var $this = $(this);
		var $childMenu = $this.next();
		var animationSpeed = 500;
		// 如果它是菜单，显示或隐藏它
		if ($childMenu.is(".treeview-menu")) {
			// 阻止原有事件
			e.preventDefault();
			if ($childMenu.is(":visible")) {
				// 隐藏菜单
				$childMenu.slideUp(animationSpeed, function () {
					$childMenu.removeClass("menu-open");
				});
				$childMenu.parent("li").removeClass("active");
			} else {
				// 显示菜单
				var $parent = $this.parent("li");
				$childMenu.slideDown(animationSpeed, function () {
					$childMenu.addClass("menu-open");
					$parent.addClass("active");
				});
			}
		}
	});
});
