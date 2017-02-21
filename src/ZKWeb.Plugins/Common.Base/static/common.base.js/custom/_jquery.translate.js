/*
	支持客户端翻译文本

	使用
	var title = $.translate("Title");

	提供翻译来源
	<span class="translation hide" data-original="Title" data-translated="标题"></span>
	<span class="translation hide" data-translations="{ \"Title\": \"标题\" }"></span>
*/

$.translations = {};
$.translate = function (text) {
	text = text || "";
	var translated = $.translations[text];
	if (translated == undefined || translated == null) {
		return text; // 允许翻译到""
	}
	return translated;
};

$(function () {
	var setup = function (element) {
		$(element).find(".translation").each(function () {
			var $translation = $(this);
			// 添加data-original和data-translated
			// 没有时不添加
			var original = $translation.attr("data-original");
			if (original) {
				$.translations[original] = $translation.attr("data-translated");
			}
			// 添加data-translations
			// 没有时不添加
			var translations = $translation.data("translations");
			if (translations) {
				$.each(translations, function (key, value) {
					$.translations[key] = value;
				});
			}
		});
	};
	setup(document);
	$(document).on("dynamicLoaded", function (e, contents) { setup(contents); });
});
