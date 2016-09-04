/*
	支持延迟加载脚本
	例子
	<div require-script="/statis/demo/demo.js"></div>
	
	注意：
	每个路径最多只会加载一次
	会同时加载所有要求的脚本，但最终运行顺序不固定，请在脚本中做好处理
*/

$(function () {
	var loadedScripts = []; // 已加载的脚本
	var loadRequiredScripts = function (rootElement) {
		$(rootElement).find("[require-script]").each(function () {
			var script = $(this).attr("require-script");
			if (_.contains(loadedScripts, script)) {
				return; // 防止重复加载
			}
			loadedScripts.push(script);
			$.ajax({ dataType: "script", cache: true, url: script });
		});
	};
	loadRequiredScripts(document);
	$(document).on("dynamicLoaded", function (e, contents) { loadRequiredScripts(contents); });
});
