/*
	Ajax动态表格树帮助类
*/

$.fn.ajaxTableTreeHelper = function (options) {
	// 已生成过时返回之前生成的对象
	var helper = this.data("ajaxTableTreeHelper");
	if (helper) {
		return helper;
	}

	// 创建新的Ajax表格树帮助类
	var $ajaxTable = this.ajaxTable();
	var helper = {
		getParents: function (element, levelMember) {
			// 获取所有上级行，从下到上
			// 只能用于表格树中
			var $row = $(element).closest("[role='row']");
			var $parents = $();
			var level = $row.data("row")[levelMember];
			while ($row.length && level > 0) {
				$row = $row.prev();
				var row = $row.data("row");
				if (!row) {
					continue;
				}
				var thisLevel = row[levelMember];
				if (thisLevel < level) {
					level = thisLevel;
					$parents = $parents.add($row);
				}
			}
			return $parents;
		},
		toggleChilds: function (element, levelMember) {
			// 切换所有子行的显示状态
			// 只能用于表格树中，要求所有子行必须跟随传入的行
			var $row = $(element).closest("[role='row']");
			var level = $row.data("row")[levelMember];
			$row.toggleClass("collapsed");
			while (($row = $row.next()).length &&
				($row.data("row") || {})[levelMember] > level) {
				var $parents = helper.getParents($row, levelMember);
				$parents.is(".collapsed") ? $row.hide("fast") : $row.show("fast");
			}
		},
		toggleAll: function (levelMember) {
			// 展开/折叠全部，只能用于表格树中
			var $rows = $ajaxTable.container.find("[role='row']").filter(function () {
				return $(this).data("row");
			});
			$rows.toggleClass("collapsed", !$rows.hasClass("collapsed"));
			$rows.each(function () {
				var $this = $(this);
				var $parents = helper.getParents($this, levelMember);
				$parents.is(".collapsed") ? $this.hide("fast") : $this.show("fast");
			});
		}
	};
	this.data("ajaxTableTreeHelper", helper);
	return helper;
};

$.fn.closestAjaxTableTreeHelper = function () {
	// 获取离元素最近的ajaxTableTree帮助类
	return $(this).closestAjaxTable().container.ajaxTableTreeHelper();
};
