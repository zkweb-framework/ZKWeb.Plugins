/*
	图片浏览器使用的功能
*/

$(function () {
	// 上传完成后自动刷新图片列表
	var $form = $(".image-upload-portlet form");
	$form.on("success", function () {
		$(".image-browse-portlet .ajax-table").ajaxTable().refresh();
	});

	// 使用图片的处理
	// 发送事件到上层窗口，并关闭自身
	var $container = $(".image-browser-container");
	$container.on("click", ".image-actions .use-this-image", function () {
		var $tile = $(this).closest(".image-tile");
		var originalPath = $tile.find("img").data("src-original");
		var opener = window.opener;
		opener && opener.jQuery(opener.document).trigger("selected.imageBrowser", originalPath);
		window.close();
	});

	// 删除图片的处理
	$container.on("click", ".image-actions .remove", function () {
		var confirmMessage = $container.find(".translations .sure-to-remove").text();
		if (confirm(confirmMessage)) {
			var removeUrl = $container.find(".variables").data("removeUrl");
			var $title = $(this).closest(".image-tile");
			var $img = $title.find("img");
			var name = $img.attr("title");
			var extension = $img.attr("extension");
			$.post(removeUrl, { name: name, extension: extension }, function (data) {
				$.handleAjaxResult(data);
				$(".image-browse-portlet .ajax-table").ajaxTable().refresh();
			});
		}
	});
});
