/*
	额外的显示模态框函数
*/

/* 显示包含远程页面的模态框 */
BootstrapDialog.showRemote = function (title, url, extendParameters) {
	// 创建页面内容的容器
	var $contents = $("<div class='remote-contents'>").attr("href", url);
	$contents.on("reload", function () {
		var url = $contents.attr("href");
		$contents.empty().addClass("loading").load(url, function () {
			$(document).trigger("dynamicLoaded", $contents); // 触发动态加载事件
			$contents.removeClass("loading");
		});
	});
	// 弹出模态框
	BootstrapDialog.show($.extend({
		type: "type-primary",
		size: "size-fullscreen",
		title: title,
		message: $contents
	}, extendParameters || {}));
	// 等待模态框加到DOM后再读取远程数据
	setTimeout(function () { $contents.trigger("reload"); }, 1);
};
