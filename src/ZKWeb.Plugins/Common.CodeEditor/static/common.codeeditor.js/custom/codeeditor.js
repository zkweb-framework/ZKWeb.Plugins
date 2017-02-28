/*
	初始化代码编辑器

	例子
	<textarea data-trigger="code-editor" data-language="html" data-config="{}"></textarea>
*/

$(function () {
	// 语言别名的定义
	var languageAlias = {
		"html": "htmlmixed",
		"js": "javascript",
		"c": "text/x-csrc",
		"c++": "text/x-c++src",
		"cpp": "text/x-c++src",
		"cxx": "text/x-c++src",
		"java": "text/x-java",
		"c#": "text/x-csharp",
		"csharp": "text/x-csharp",
		"objc": "text/x-objectivec",
		"objectivec": "text/x-objectivec",
		"scala": "text/x-scala",
		"squirrel": "text/x-squirrel",
		"ceylon": "text/x-ceylon"
	};

	// 初始化代码编辑器
	var setupCodeEditor = function ($element) {
		var language = $element.attr("data-language");
		var customConfig = $element.data("config") || {};
		var codeEditor = $element.data("codeEditor");
		if (codeEditor) {
			return codeEditor;
		}
		// 处理语言别名
		language = languageAlias[language] || language;
		// 调用CodeMirror
		codeEditor = CodeMirror.fromTextArea($element[0], $.extend({
			mode: language,
			lineNumbers: true,
			matchTags: true,
			matchBrackets: true,
			autoCloseTags: true,
			autoCloseBrackets: true,
			showTrailingSpace: true,
			highlightSelectionMatches: true,
			styleActiveLine: true,
			gutters: ["CodeMirror-lint-markers"],
			lint: true
		}, customConfig));
		// 触发自动提示
		// http://stackoverflow.com/questions/13744176/codemirror-autocomplete-after-any-keyup
		codeEditor.on("keyup", function (cm, event) {
			console.log(event);
			if (!cm.state.completionActive && (event.key || "").match(/^[\d\w-_]$/)) {
				CodeMirror.commands.autocomplete(cm, null, { completeSingle: false });
			}
		});
		$element.data("codeEditor", codeEditor);
		return codeEditor;
	};

	// 自动初始化[data-trigger=code-editor]
	var rule = "[data-trigger=code-editor]";
	$(rule).each(function () { setupCodeEditor($(this)); });
	$(document).on("dynamicLoaded", function (e, contents) {
		$(contents).find(rule).each(function () { setupCodeEditor($(this)); });
	});
});
