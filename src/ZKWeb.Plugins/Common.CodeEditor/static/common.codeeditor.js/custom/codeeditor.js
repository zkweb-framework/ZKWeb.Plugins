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

	// 一行的高度
	var heightPerRow = 20;

	// 初始化代码编辑器
	var setupCodeEditor = function ($element) {
		var language = $element.attr("data-language");
		var rows = parseInt($element.attr("rows")) || 8;
		var customConfig = $element.data("config") || {};
		var codeEditor = $element.data("codeEditor");
		if (codeEditor) {
			return codeEditor;
		}
		// 处理语言别名
		language = languageAlias[language] || language;
		// 调用CodeMirror
		var mergedConfig = $.extend({
			mode: language,
			lineNumbers: true,
			matchTags: true,
			matchBrackets: true,
			autoCloseTags: true,
			autoCloseBrackets: true,
			showTrailingSpace: true,
			highlightSelectionMatches: true,
			styleActiveLine: true,
			gutters: [],
			lint: true
		}, customConfig);
		if (mergedConfig.lint &&
			mergedConfig.gutters.indexOf("CodeMirror-lint-markers") < 0) {
			mergedConfig.gutters.push("CodeMirror-lint-markers");
		}
		codeEditor = CodeMirror.fromTextArea($element[0], mergedConfig);
		// 触发自动提示
		// http://stackoverflow.com/questions/13744176/codemirror-autocomplete-after-any-keyup
		var autoCompleteRegex = null;
		if (customConfig.autoCompleteKeys) {
			autoCompleteRegex = new RegExp(autoCompleteKeys);
		} else if (language == "htmlmixed" || language == "htmlembedded" || language == "xml") {
			autoCompleteRegex = /^[\d\w-_<]$/;
		} else {
			autoCompleteRegex = /^[\d\w-_]$/;
		}
		codeEditor.on("keyup", function (cm, event) {
			if (!cm.state.completionActive && (event.key || "").match(autoCompleteRegex)) {
				CodeMirror.commands.autocomplete(cm, null, { completeSingle: false });
			}
		});
		// 自动保存
		var saveHandler = null;
		codeEditor.on("change", function () {
			clearTimeout(saveHandler);
			setTimeout(function () { codeEditor.save(); }, 1);
		});
		// 设置高度
		$(codeEditor.display.wrapper).css("height", (rows * heightPerRow) + "px")
		// 设置到data并返回
		$element.data("codeEditor", codeEditor);
		return codeEditor;
	};

	// 自动初始化[data-trigger=code-editor]
	var rule = "[data-trigger=code-editor]";
	$(rule).each(function () { setupCodeEditor($(this)); });
	$(document).on("dynamicLoaded", function (e, contents) {
		$(contents).find(rule).each(function () { setupCodeEditor($(this)); });
	});

	// 定时刷新所有编辑器
	// 有时编辑器会从隐藏状态显示，这个时候就需要进行刷新
	// 否则会出现样式错位等问题
	setInterval(function () {
		$(rule).each(function () {
			var codeEditor = $(this).data("codeEditor");
			codeEditor && codeEditor.refresh();
		});
	}, 500);
});
