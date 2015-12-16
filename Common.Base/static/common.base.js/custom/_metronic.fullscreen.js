/*
	metronic框架带的全屏功能
*/

$(function () {
	// 获取屏幕大小，可能需要移动到其他文件
	// http://andylangton.co.uk/blog/development/get-viewport-size-width-and-height-javascript
	var getViewPort = function () {
		var e = window, a = 'inner';
		if (!('innerWidth' in window)) {
			a = 'client';
			e = document.documentElement || document.body;
		}
		return { width: e[a + 'Width'], height: e[a + 'Height'] };
	};

	// 支持全屏设置
	$('body').on('click', '.portlet a.fullscreen', function (e) {
		e.preventDefault();
		var portlet = $(this).closest(".portlet");
		if (portlet.hasClass('portlet-fullscreen')) {
			$(this).removeClass('on');
			portlet.removeClass('portlet-fullscreen');
			$('body').removeClass('page-portlet-fullscreen');
			portlet.children('.portlet-body').css('height', 'auto');
		} else {
			var height = getViewPort().height -
				portlet.children('.portlet-title').outerHeight() -
				parseInt(portlet.children('.portlet-body').css('padding-top')) -
				parseInt(portlet.children('.portlet-body').css('padding-bottom'));

			$(this).addClass('on');
			portlet.addClass('portlet-fullscreen');
			$('body').addClass('page-portlet-fullscreen');
			portlet.children('.portlet-body').css('height', height);
		}
	});
});
