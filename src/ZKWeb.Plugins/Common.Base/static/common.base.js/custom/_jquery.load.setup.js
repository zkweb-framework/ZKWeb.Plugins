/*
	对Jquery.load的修改和额外处理
*/

$(function () {
	// 使用Jquery.load时，删除远程内容中的重复的script和link标签
	// 如果不删除重复的script标签，
	// 从DOM删除载入的内容时会同时删除脚本中定义的对象，导致主页面的原有的脚本不能使用
	var removeDuplicateTag = function (name) {
		$(this).find(name).filter(function () {
			var html = $(this).prop("outerHTML");
			var result = $(name).filter(function () { return $(this).prop("outerHTML") == html; });
			return result.length;
		}).remove();
	};
	$.fn.loadOrig = $.fn.load;
	$.fn.load = function (url, params, callback) {
		if (typeof url !== "string" && _load) {
			return jQuery.fn.loadOrig.apply(this, arguments);
		}
		var selector, type, response, self = this, off = url.indexOf(" ");
		if (off > -1) {
			selector = jQuery.trim(url.slice(off));
			url = url.slice(0, off);
		}
		if ($.isFunction(params)) {
			callback = params;
			params = undefined;
		} else if (params && typeof params === "object") {
			type = "POST";
		}
		if (self.length > 0) {
			$.ajax({
				url: url,
				type: type || "GET",
				dataType: "html",
				data: params,
				cache: false // 防止ie下缓存
			}).done(function (responseText) {
				$response = $("<div>").append($.parseHTML(responseText, null, true));
				removeDuplicateTag.call($response, "script");
				removeDuplicateTag.call($response, "link");
				self.html(selector ? $response.find(selector) : $response.children());
			}).always(callback && function (jqXHR, status) {
				self.each(function () {
					callback.apply(self, response || [jqXHR.responseText, status, jqXHR]);
				});
			});
		}
		return this;
	};
});
