/*
	Ajax动态表格
*/

$.ajaxTableType = function() { };
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
	table = new $.ajaxTableType();
	table.options = options;
	table.searchRequest = {
		PageIndex: options.pageIndex,
		PageSize: options.pageSize,
		Keyword: options.keyword,
		Conditions: options.conditions
	};
	table.container = this;
	table.compiledTemplate = null;
	this.data("ajaxTable", table);
	return table;
};

(function() {
	// 定义ajax表格的函数
	// 刷新
	$.ajaxTableType.prototype.refresh = function () {
		// 添加loading类
		var self = this;
		var options = self.options;
		self.container.addClass(options.loadingClass);
		// 提交搜索
		$.post(options.target, { json: JSON.stringify(self.searchRequest) }).done(function (data) {
			// 载入搜索的结果
			// 模板已预编译时使用编译的内容，否则重新获取并编译
			var loadResult = function () {
				self.searchRequest.PageIndex = data.PageIndex;
				self.searchRequest.PageSize = data.PageSize;
				self.container.html(self.compiledTemplate({ result: data }));
				self.container.removeClass(options.loadingClass);
				// 绑定分页事件
				var $pagination = self.container.find(".pagination-panel");
				(data.PageSize > 0x7fffffff) && $pagination.addClass("hide"); // 只显示单页时，隐藏分页控件
				(data.PageIndex == 0) && $pagination.find(".to-first, .to-prev").addClass("disabled"); // 当前是首页
				(data.IsLastPage) && $pagination.find(".to-last, .to-next").addClass("disabled"); // 当前是末页
				$pagination.find(".to-first").on("click", function () { self.toPage(0); }); // 到首页
				$pagination.find(".to-prev").on("click", function () { self.toPrevPage(); }); // 到上一页
				$pagination.find(".to-next").on("click", function () { self.toNextPage(); }); // 到下一页
				$pagination.find(".to-last").on("click", function () { self.toPage(0x7fffffff); }); // 到末页
				$pagination.find(".pagination-panel-input").on("keydown", function (e) {
					(e.keyCode == 13) && self.toPage(parseInt($(this).val() - 1));
				}).val(data.PageIndex + 1); // 到指定页
				// 触发载入后事件
				self.container.trigger("loaded.ajaxTable");
			};
			if (self.compiledTemplate) {
				loadResult();
			} else {
				$.get(options.template).done(function (html) {
					self.compiledTemplate = _.template(html);
					loadResult();
				}).fail(function (jqXHR) {
					self.container.removeClass(options.loadingClass);
				});
			}
		}).fail(function (jqXHR) {
			self.container.removeClass(options.loadingClass);
		});
	};
	// 到指定页
	$.ajaxTableType.prototype.toPage = function (pageIndex) {
		this.searchRequest.PageIndex = parseInt(pageIndex) || 0;
		this.refresh();
	};
	// 到上一页
	$.ajaxTableType.prototype.toPrevPage = function () {
		this.toPage(this.searchRequest.PageIndex - 1);
	};
	// 到下一页
	$.ajaxTableType.prototype.toNextPage = function () {
		this.toPage(this.searchRequest.PageIndex + 1);
	};
	// 更新每页显示数量
	$.ajaxTableType.prototype.updatePageSize = function (size) {
		this.searchRequest.PageSize = parseInt(size);
		this.toPage(0);
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
};

$(function () {
	// 自动初始化ajax表格
	$(".ajax-table").each(function () {
		var $table = $(this);
		var options = {
			target: $table.attr("ajax-table-target"),
			template: $table.attr("ajax-table-template") || undefined
		};
		if (options.target) {
			$table.ajaxTable(options).refresh();
		}
	});
	// ajax表格菜单功能
	// 刷新
	$("body").on("click", ".ajax-table-menu .refresh", function (e) {
		$(this).closestAjaxTable().refresh();
	});
	// 设置每页显示数量
	$("body").on("click", ".ajax-table-menu .set-page-size", function (e) {
		var $item = $(this);
		$item.closestAjaxTable().updatePageSize($item.data("size"));
	});
});
