/*
	点击缩略图弹出大图
	例子
	<img src="a.thumb.jpg" alt="图片描述" data-trigger="popup-original" data-original-src="a.jpg" />
*/

$(function () {
	var setup = function ($elements) {
		$elements.each(function () {
			var $this = $(this);
			var alt = $this.attr("alt");
			var originalSrc = $this.attr("data-original-src");
			$this.click(function () {
				var $contents = $("<div>", { "class": "original-image" });
				$contents.append($("<img>", { src: originalSrc, alt: alt }));
				BootstrapDialog.show({
					type: "type-primary",
					size: "size-wide",
					title: alt,
					message: $contents,
					closeByBackdrop: true,
					closeByKeyboard: true
				});
			});
		});
	};
	var rule = "img[data-trigger='popup-original']";
	setup($(rule));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(rule));
	});
});
