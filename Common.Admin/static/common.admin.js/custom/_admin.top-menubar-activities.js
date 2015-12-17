/*
	后台顶部栏的活动列表使用的脚本
	显示活动列表的对象".page-header .page-top .nav"
		页面打开后30秒后开始定时刷新，每30秒刷新一次
		提供事件 reloadActivities.adminTopMenuBar 立刻刷新活动列表
		提供事件 reloadActivitiesNoCache.adminTopMenuBar 立刻刷新活动列表，指定忽略缓存
*/
$(function () {
	// 设置定时刷新
	var $nav = $(".admin-page-top .page-header .page-top .nav").first();
	var reloadActivitiesEventName = "reloadActivities.adminTopMenuBar";
	var reloadActivitiesNoCacheEventName = "reloadActivitiesNoCache.adminTopMenuBar";
	if ($nav.length) {
		var timeout = null;
		var reload = function (nocache) {
			var url = "/admin/top_menubar_activities?nocache=" + Boolean(nocache);
			var $container = $("<div>");
			$container.load(url, function () {
				$nav.children(".activities").remove();
				$nav.prepend($container.children());
				$nav.find("[data-hover='dropdown']").dropdownHover();
			});
			clearTimeout(timeout);
			timeout = setTimeout(reload, 30000);
		};
		$nav.on(reloadActivitiesEventName, function () { reload(); });
		$nav.on(reloadActivitiesNoCacheEventName, function () { reload(true); });
		timeout = setTimeout(reload, 0);
	}
	// 点击显示消息的函数
	// 关闭显示对话框时立刻刷新
	$.fn.adminTopMenuBarShowUserMessage = function (messageId) {
		var $this = $(this);
		BootstrapDialog.show({
			title: $this.attr("data-title"),
			message: $('<div>').load("/user_messages/view_message/" + messageId),
			onshow: function (dialog) {
				dialog.getModal().one("hide.bs.modal", function () {
					$this.closest(".nav").trigger(reloadActivitiesNoCacheEventName);
				});
			}
		});
	};
});
