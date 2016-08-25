/*
	支持刷新图片，用在img对象中
*/

$.fn.refreshImage = function () {
	$(this).each(function () {
		var $img = $(this);
		var src = $img.attr("src");
		if (src) {
			var uri = new Uri(src);
			uri.replaceQueryParam("timestamp", new Date().getTime());
			$img.attr("src", uri);
		}
	});
};
