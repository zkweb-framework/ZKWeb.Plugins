/*
	前台商品相册使用的功能
		相册图片切换和放大镜
	使用时要求页面有ProductGallery元素
	脚本只在页面打开时执行，对于打开后动态添加的内容不起效果
*/

$(function () {
	var $productGallery = $(".product-gallery"); // 商品相册的元素，注意一个页面可能有多个
	var mainImageChangedEventName = "mainImageChanged.productGallery"; // 大图改变后的事件，可以给其他插件绑定
	// 鼠标点击相册图片时，切换大图到指向的图片
	$productGallery.on("click", ".images img", function () {
		var $image = $(this);
		var $mainImage = $image.closest($productGallery).find(".main-image img");
		var targetUrl = $image.attr("data-src-original").trim();
		if ($mainImage.attr("src").trim() != targetUrl) {
			$mainImage.fadeOut(150, function () {
				$mainImage.removeAttr("src"); // 清空原图片，防止载入延迟
				$mainImage.attr("src", targetUrl); // 切换图片
				var elevate = $mainImage.data("elevateZoom"); // 获取放大镜
				elevate && elevate.swaptheimage(targetUrl, targetUrl); // 切换放大镜图片
				$productGallery.trigger(mainImageChangedEventName); // 触发大图改变后的事件
			}).fadeIn(150);
		}
	});
	// 启用放大镜效果
	$productGallery.find(".main-image img").each(function () {
		$(this).elevateZoom({
			lensFadeOut: 250,
			borderSize: 1,
			showLens: true,
			lensOpacity: 0.3,
			zoomWindowFadeOut: 250,
			lensBorderColour: "#888",
			zoomType: "inner",
			zoomWindowHeight: 300,
			borderColour: "#000",
			lensColour: "#fff",
			lensShape: "square",
			lensBorderSize: 1,
			zoomWindowWidth: 300,
			zoomWindowFadeIn: 250,
			lensFadeIn: 250,
			easing: true
		});
	});
});
