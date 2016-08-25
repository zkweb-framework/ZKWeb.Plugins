/*
	对bootstrap-contextmenu的修改和额外处理
*/

$(function () {
	// 原来的函数无法获取点击的元素
	// 这里让传递到show.bs.context中的e.target等于点击的元素
	var typeinfo = $.fn.contextmenu.Constructor.prototype;
	typeinfo.showOrig = typeinfo.show;
	typeinfo.show = function (e) {
		e.currentTarget = e.target;
		typeinfo.showOrig.call(this, e);
	};
	// 查找所有的右键菜单区域
	var $context = $("[data-toggle='context']");
	$context.each(function (_, element) {
		var $element = $(element);
		var $menu = $($element.attr("data-target"));
		$menu.on('show.bs.context', function (e) {
			// 区域中有数据表格时，为点击的行添加selected类
			if ($element.find("table.dataTable").length) {
				var $rowElement = $(e.target).closest("[role='row']");
				$rowElement.parent("tbody").length && $rowElement.addClass("selected");
			}
		});
		$menu.on('hidden.bs.context', function () {
			// 区域中有表格时，把所有行的selected类删除
			$element.find("table.dataTable > tbody").children().removeClass("selected");
		});
	});
	// 捕捉长点击事件，长点击时显示右键菜单
	// 显示右键菜单后需要防止一次click事件避免松开时菜单自动消失（不一定会触发）
	$(document).on("taphold", "[data-toggle='context']", function (e) {
		var $target = $(e.target);
		var offset = $target.offset() || {};
		$target.trigger({
			type: "contextmenu",
			target: e.target,
			currentTarget: e.currentTarget,
			clientX: offset.left - $(document).scrollLeft(),
			clientY: offset.top - $(document).scrollTop()
		});
		$target.one("click", function () { return false; });
	});
});