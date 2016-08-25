/*
	Ajax动态表格的扩展函数
*/

(function () {
	// 全选/取消全选
	$.ajaxTableType.prototype.selectOrUnselectAll = function () {
		var $all = this.container.find(".checker input[type=checkbox]");
		$all.prop("checked", $all.length != $all.filter(":checked").length);
	};
	// 获取元素对应行的数据
	$.ajaxTableType.prototype.getBelongedRowData = function (element) {
		return $(element).closest("[role='row']").data("row");
	};
	// 当前选中单行的数据
	$.ajaxTableType.prototype.getSingleSelectedRowData = function () {
		return this.container.find("[role='row'].selected").data("row");
	};
	// 当前多选框选中所有行的数据
	$.ajaxTableType.prototype.getMultiCheckedRowsData = function () {
		return $.map(
			this.container.find(".checker :checked").closest("[role='row']"),
			function (element) { return $(element).data("row"); });
	};
	// 提交操作到指定的地址并显示返回的消息，成功时刷新表格
	$.ajaxTableType.prototype.postAction = function (data, target) {
		var table = this;
		$.post(target, { json: JSON.stringify(data) }, function (data) {
			table.refresh();
			$.handleAjaxResult(data);
		});
	};
	// 切换回收站状态
	$.ajaxTableType.prototype.toggleRecycleBin = function (toggleButton) {
		var conditions = this.searchRequest.Conditions;
		if (conditions.Deleted) {
			delete conditions.Deleted;
			$(toggleButton).removeClass("active")
		} else {
			conditions.Deleted = true;
			$(toggleButton).addClass("active")
		}
		this.toPage(0);
	};
	// 弹出使用远程内容显示或编辑表格中数据的模态框，支持在数据改动后刷新表格
	// 标题和远程地址根据模板和数据生成
	$.ajaxTableType.prototype.showRemoteModalForRow = function (
		row, titleTemplate, urlTemplate, extendParameters) {
		var table = this;
		var title = _.template(titleTemplate)({ row: row });
		var url = _.template(urlTemplate)({ row: row });
		BootstrapDialog.showRemote(title, url, $.extend({
			onshow: function (dialog) {
				var $modal = dialog.getModal();
				$modal.one("updated.ajaxTable", function () {
					$modal.one("hide.bs.modal", function () {
						table.refresh();
					});
				});
			}
		}, extendParameters || {}));
	};
	// 显示需要确认的批量操作模态框
	// 标题和显示信息根据模板和数据生成
	$.ajaxTableType.prototype.showConfirmActionForRows = function (
		rows, okLabel, cancelLabel,
		titleTemplate, messageTemplate, callback, extendParameters) {
		var parameters = $.extend({
			type: "type-primary",
			size: "size-normal",
			draggable: true,
			btnOKLabel: okLabel,
			btnCancelLabel: cancelLabel,
			title: _.template(titleTemplate)({ rows: rows }),
			message: _.template(messageTemplate)({ rows: rows }),
			callback: callback
		}, extendParameters || {});
		BootstrapDialog.confirm(parameters);
	};
	// 获取所有上级行，从下到上
	// 只能用于表格树中
	$.ajaxTableType.prototype.treeNodeGetParents = function (element, levelMember) {
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
	};
	// 切换所有子行的显示状态
	// 只能用于表格树中，要求所有子行必须跟随传入的行
	$.ajaxTableType.prototype.treeNodeToggleChilds = function (element, levelMember) {
		var table = this;
		var $row = $(element).closest("[role='row']");
		var level = $row.data("row")[levelMember];
		$row.toggleClass("collapsed");
		while (($row = $row.next()).length &&
			($row.data("row") || {})[levelMember] > level) {
			var $parents = table.treeNodeGetParents($row, levelMember);
			$parents.is(".collapsed") ? $row.hide("fast") : $row.show("fast");
		}
	};
	// 展开/折叠全部，只能用于表格树中
	$.ajaxTableType.prototype.treeNodeToggleAll = function (levelMember) {
		var table = this;
		var $rows = table.container.find("[role='row']").filter(function () {
			return $(this).data("row");
		});
		$rows.toggleClass("collapsed", !$rows.hasClass("collapsed"));
		$rows.each(function () {
			var $this = $(this);
			var $parents = table.treeNodeGetParents($this, levelMember);
			$parents.is(".collapsed") ? $this.hide("fast") : $this.show("fast");
		});
	};
})();
