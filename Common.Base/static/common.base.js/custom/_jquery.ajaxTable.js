/*
	Ajax动态表格
*/

$.fn.ajaxTable = function (options) {
	// 已生成过时返回之前生成的对象
	var table = this.data("ajaxTable");
	if (table) {
		return table;
	}

	// 元素不存在时返回undefined
	if (!this.length) {
		return;
	}

	// 设置默认选项
	options = $.extend({
		pageIndex: 0,
		pageSize: 50,
		keyword: null,
		conditions: {},
		target: null,
		loadingClass: "loading",
		template: "/static/common.base.tmpl/ajaxTable.tmpl"
	}, options || {});

	// 创建新的Ajax表格对象
	table = {
		searchRequest: {
			PageIndex: options.pageIndex,
			PageSize: options.pageSize,
			Keyword: options.keyword,
			Conditions: options.conditions
		},
		container: this,
		compiledTemplate: null,
		refresh: function () {
			// 添加loading类
			table.container.addClass(options.loadingClass);
			// 提交搜索
			$.post(options.target, { json: JSON.stringify(this.searchRequest) }).done(function (data) {
				// 载入搜索的结果
				// 模板已预编译时使用编译的内容，否则重新获取并编译
				var loadResult = function () {
					table.searchRequest.PageIndex = data.PageIndex;
					table.searchRequest.PageSize = data.PageSize;
					table.container.html(table.compiledTemplate({ result: data }));
					table.container.removeClass(options.loadingClass);
					// 绑定分页事件
					var $pagination = table.container.find(".pagination-panel");
					(data.PageSize > 0x7fffffff) && $pagination.addClass("hide"); // 只显示单页时，隐藏分页控件
					(data.PageIndex == 0) && $pagination.find(".to-first, .to-prev").addClass("disabled"); // 当前是首页
					(data.IsLastPage) && $pagination.find(".to-last, .to-next").addClass("disabled"); // 当前是末页
					$pagination.find(".to-first").on("click", function () { table.toPage(0); }); // 到首页
					$pagination.find(".to-prev").on("click", function () { table.toPrevPage(); }); // 到上一页
					$pagination.find(".to-next").on("click", function () { table.toNextPage(); }); // 到下一页
					$pagination.find(".to-last").on("click", function () { table.toPage(0x7fffffff); }); // 到末页
					$pagination.find(".pagination-panel-input").on("keydown", function (e) {
						(e.keyCode == 13) && table.toPage(parseInt($(this).val() - 1));
					}).val(data.PageIndex + 1); // 到指定页
					// 触发载入后事件
					table.container.trigger("loaded.ajaxTable");
				};
				if (table.compiledTemplate) {
					loadResult();
				} else {
					$.get(options.template).done(function (html) {
						table.compiledTemplate = _.template(html);
						loadResult();
					}).fail(function (jqXHR) {
						table.container.removeClass(options.loadingClass);
					});
				}
			}).fail(function (jqXHR) {
				table.container.removeClass(options.loadingClass);
			});
		},
		toPage: function (pageIndex) {
			// 到指定页
			table.searchRequest.PageIndex = parseInt(pageIndex) || 0;
			table.refresh();
		},
		toPrevPage: function () {
			// 到上一页
			table.toPage(table.searchRequest.PageIndex - 1);
		},
		toNextPage: function () {
			// 到下一页
			table.toPage(table.searchRequest.PageIndex + 1);
		}
	}
	this.data("ajaxTable", table);
	return table;
};

$.fn.closestAjaxTable = function () {
	// 获取离元素最近的ajaxTable对象
	var $element = $(this);
	while ($element.length) {
		var table = $element.data("ajaxTable");
		if (table) {
			return table;
		}
		$element = $element.parent();
	}
};
