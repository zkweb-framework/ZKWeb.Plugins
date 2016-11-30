/*
	Ajax动态表格的过滤栏功能
*/

$(function () {
	// ajax表格过滤按钮功能
	// 点击时设置条件 data-key的值 等于 data-value的值
	var filterBtnRule = ".ajax-table-search-bar .btn-filter";
	$body.on("click", filterBtnRule, function () {
		var $this = $(this);
		var table = $(this).closestAjaxTable();
		$this.closest(".btn-group").find(".btn-filter").removeClass("active");
		$this.addClass("active");
		table.searchRequest.Conditions[$this.data("key")] = $this.data("value");
		table.applySearch(this);
	});
	// ajax表格刷新后，自动获取过滤栏上的显示数量
	$body.on("loaded.ajaxTable", ".ajax-table", function () {
		var $this = $(this);
		var table = $(this).closestAjaxTable();
		$(filterBtnRule).each(function () {
			// 查找ajax表格对应的过滤按钮，要求按钮的extra中的displayCount是true
			var $btn = $(this);
			if (!($btn.data("extra") || {}).displayCount || $btn.closestAjaxTable() != table) {
				return;
			}
			// 自动添加数字显示
			var $count = $btn.find(".total-count");
			if (!$count.length) {
				$count = $("<span>").addClass("total-count").appendTo($btn);
			}
			// 从服务器获取该过滤按钮对应的总数量
			$count.hide();
			var sequenceName = "display-count-sequence";
			var sequence = ($btn.data(sequenceName) || 0) + 1;
			$btn.data(sequenceName, sequence);
			var searchRequest = JSON.parse(JSON.stringify(table.searchRequest));
			searchRequest.PageNo = 1;
			searchRequest.PageSize = 1;
			searchRequest.Conditions[$btn.data("key")] = $btn.data("value");
			searchRequest.Conditions["RequireTotalCount"] = true;
			$.post(table.options.target, { json: JSON.stringify(searchRequest) }).done(function (data) {
				// 避免先发起的请求后收到回应时，覆盖后请求的数量
				if (sequence != $btn.data(sequenceName)) {
					return;
				}
				// 显示返回的总数量
				var totalCount = data.Pagination.TotalCount;
				if (totalCount != null && totalCount > 0) {
					$count.addClass("more-than-zero");
				} else {
					$count.removeClass("more-than-zero");
				}
				$count.text(totalCount == null ? "-" : totalCount).show();
			});
		});
	});
});
