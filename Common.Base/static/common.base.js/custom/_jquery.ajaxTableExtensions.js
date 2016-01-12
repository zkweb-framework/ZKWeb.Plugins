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
		/*var table = this;
		var token = $("input[name='__RequestVerificationToken']").val();
		$.post(target, { json: JSON.stringify(data), __RequestVerificationToken: token }, function (data) {
			if (data.success) {
				table.refresh();
			}
			$.handleAjaxResult(data);
		});*/
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
})();
