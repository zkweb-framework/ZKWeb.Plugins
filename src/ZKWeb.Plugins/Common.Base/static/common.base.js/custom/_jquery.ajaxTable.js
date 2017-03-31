/*
	Ajax动态表格
*/

$.ajaxTableType = function () { };
$.ajaxTableOptions = {
	pageNo: 1, // 页号, 0开始
	pageSize: 50, // 一页的数量
	keyword: null, // 抓取数据使用的关键字
	conditions: {}, // 抓取数据使用的参数
	target: null, // 抓取数据的url
	loadingClass: "loading", // 载入中的css类
	hidePagination: false, // 隐藏分页栏
	template: "/static/common.base.tmpl/ajaxTable.tmpl", // 绑定内容的模板
	delayRefresh: false // 是否延迟刷新, 仅适用于自动初始化
};
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
	options = $.extend({}, $.ajaxTableOptions, options || {});
	// 创建新的Ajax表格对象
	table = new $.ajaxTableType();
	table.options = options;
	table.searchRequest = {
		PageNo: options.pageNo,
		PageSize: options.pageSize,
		Keyword: options.keyword,
		Conditions: options.conditions
	};
	table.container = this;
	table.compiledTemplate = null;
	this.data("ajaxTable", table);
	return table;
};

(function () {
	// 定义ajax表格的函数
	// 刷新
	$.ajaxTableType.prototype.refresh = function () {
		// 添加loading类
		var table = this;
		var options = table.options;
		table.container.addClass(options.loadingClass);
		// 提交搜索
		$.post(options.target, { json: JSON.stringify(table.searchRequest) }).done(function (data) {
			// 载入搜索的结果
			// 模板已预编译时使用编译的内容，否则重新获取并编译
			var loadResult = function () {
				table.searchRequest.PageNo = data.PageNo;
				table.searchRequest.PageSize = data.PageSize;
				table.container.html(table.compiledTemplate({ result: data }));
				$.dynamicLoaded(table.container);
				// 绑定分页事件
				var $pagination = table.container.find(".pagination");
				$pagination.on("click", ".enabled", function () {
					table.toPage($(this).data("page"));
				});
				$pagination.on("keydown", ".pagination-input", function (e) {
					(e.keyCode == 13) && table.toPage(parseInt($(this).val() - 1)); // 到指定页
				});
				// 需要时隐藏分页
				if (table.options.hidePagination) {
					$pagination.hide();
				}
				// 触发载入后事件
				table.container.removeClass(options.loadingClass);
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
	};
	// 到指定页
	$.ajaxTableType.prototype.toPage = function (pageNo) {
		this.searchRequest.PageNo = parseInt(pageNo) || 1;
		this.refresh();
	};
	// 到上一页
	$.ajaxTableType.prototype.toPrevPage = function () {
		this.toPage(this.searchRequest.PageNo - 1);
	};
	// 到下一页
	$.ajaxTableType.prototype.toNextPage = function () {
		this.toPage(this.searchRequest.PageNo + 1);
	};
	// 更新每页显示数量
	$.ajaxTableType.prototype.updatePageSize = function (size) {
		this.searchRequest.PageSize = parseInt(size);
		this.toPage(1);
	};
	// 应用搜索条件并提交搜索
	$.ajaxTableType.prototype.applySearch = function (searchButton) {
		var table = this;
		var $searchButton = $(searchButton);
		var $searchBar = $searchButton.closest(".ajax-table-search-bar");
		var isAdvanceSearch = $searchButton.hasClass("advance-search-button");
		var conditions = table.searchRequest.Conditions;
		table.searchRequest.Keyword = $searchBar.find(".keyword").val();
		$searchBar.find(".advanced-search").find("input, select").each(function () {
			// 高级搜索时设置找到的搜索条件，否则删除这些搜索条件 (不影响其他条件)
			var $this = $(this);
			var key = $this.attr("name");
			if (isAdvanceSearch) {
				if ($this.attr("type") == "checkbox") {
					conditions[key] = $this.is(":checked") ? $this.val() : null;
				} else {
					conditions[key] = $this.val();
				}
			} else {
				delete conditions[key];
			}
		});
		table.toPage(0);
	};
})();

$.fn.closestAjaxTable = function () {
	// 获取离元素最近的ajaxTable对象
	var $this = $(this);
	var $element = $this;
	while ($element.length) {
		var table = $element.data("ajaxTable");
		if (table) {
			return table;
		}
		$element = $element.parent();
	}
	// 获取离元素最近的.ajax-table-menu并获取菜单指向的表格
	var $menu = $this.closest(".ajax-table-menu");
	if ($menu.length > 0) {
		return $($menu.attr("ajax-table")).data("ajaxTable");
	}
	// 获取离元素最近的.ajax-table-search-bar并获取搜索栏指向的表格
	var $searchBar = $this.closest(".ajax-table-search-bar");
	if ($searchBar.length > 0) {
		return $($searchBar.attr("ajax-table")).data("ajaxTable");
	}
	// 找不到，返回undefined
};

$(function () {
	// 自动初始化ajax表格
	var setup = function ($elements) {
		$elements.each(function () {
			var $table = $(this);
			var options = {
				target: $table.attr("ajax-table-target"),
				template: $table.attr("ajax-table-template") || undefined
			};
			options = $.extend(options,
				JSON.parse($table.attr("ajax-table-extra-options") || "{}") || {});
			var table = $table.ajaxTable(options);
			if (options.target && !options.delayRefresh) {
				table.refresh();
			}
		});
	};
	var rule = ".ajax-table";
	setup($(rule));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(rule));
	});
	// ajax表格菜单功能
	// 刷新
	$body = $("body");
	$body.on("click", ".ajax-table-menu .refresh", function (e) {
		$(this).closestAjaxTable().refresh();
	});
	// 设置每页显示数量
	$body.on("click", ".ajax-table-menu .set-page-size", function (e) {
		var $item = $(this);
		$item.closestAjaxTable().updatePageSize($item.data("size"));
	});
	// 导出到表格
	$body.on("click", ".ajax-table-menu .export-to-excel", function (e) {
		var table = $(this).closestAjaxTable();
		var $form = $("<form>", { action: "/api/base/export_ajax_table", target: "_top", method: "POST" });
		$form.append($("<input>", { name: "json", value: JSON.stringify(table.searchRequest), type: "hidden" }));
		$form.append($("<input>", { name: "target", value: table.options.target, type: "hidden" }));
		$form.appendTo("body");
		$form.submit();
	});
	// ajax表格搜索栏功能
	$body.on("keydown", ".ajax-table-search-bar .keyword", function (e) {
		e.keyCode == 13 && $(this).parent().find('.search-button').click();
	});
	$body.on("click", ".ajax-table-search-bar .search-button", function (e) {
		$(this).closestAjaxTable().applySearch(this);
	});
	$body.on("click", ".ajax-table-search-bar .advance-search-button", function (e) {
		$(this).closestAjaxTable().applySearch(this);
	});
});
