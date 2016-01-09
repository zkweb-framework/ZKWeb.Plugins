/*
	Ajax动态表格帮助类
*/

$.fn.ajaxTableHelper = function (options) {
	// 已生成过时返回之前生成的对象
	var helper = this.data("ajaxTableHelper");
	if (helper) {
		return helper;
	}

	// 创建新的Ajax表格帮助类
	var $ajaxTable = this.ajaxTable();
	var helper = {
		selectOrUnselectAll: function () {
			// 全选/取消全选
			var $all = $ajaxTable.container.find(".checker input[type=checkbox]");
			$all.prop("checked", $all.length != $all.filter(":checked").length);
		},
		getBelongedRowData: function (element) {
			// 获取元素对应行的数据
			return $(element).closest("[role='row']").data("row");
		},
		getSingleSelectedRowData: function () {
			// 当前选中单行的数据
			return $ajaxTable.container.find("[role='row'].selected").data("row");
		},
		getMultiCheckedRowsData: function () {
			// 当前多选框选中所有行的数据
			return $.map(
				$ajaxTable.container.find(".checker :checked").closest("[role='row']"),
				function (element) { return $(element).data("row"); });
		},
		postAction: function (data, target) {
			// 提交操作到指定的地址并显示返回的消息，成功时刷新表格
			var token = $("input[name='__RequestVerificationToken']").val();
			$.post(target, { json: JSON.stringify(data), __RequestVerificationToken: token }, function (data) {
				if (data.success) {
					$ajaxTable.refresh();
				}
				$.toastAjaxResult(data);
			});
		},
		toggleRecycleBin: function (toggleButton) {
			// 切换回收站状态
			var conditions = $ajaxTable.searchRequest.Conditions;
			if (conditions.DeleteState) {
				delete conditions.DeleteState;
				$(toggleButton).removeClass("active")
			} else {
				conditions.DeleteState = 1;
				$(toggleButton).addClass("active")
			}
			$ajaxTable.toPage(0);
		},
		showRemoteModalForRow: function (row, titleTemplate, urlTemplate, extendParameters) {
			// 弹出使用远程内容显示或编辑表格中数据的模态框，支持在数据改动后刷新表格
			// 标题和远程地址根据模板和数据生成
			var title = _.template(titleTemplate)({ row: row });
			var url = _.template(urlTemplate)({ row: row });
			BootstrapDialog.showRemote(title, url, $.extend({
				onshow: function (dialog) {
					var $modal = dialog.getModal();
					$modal.one("updated.ajaxTable", function () {
						$modal.one("hide.bs.modal", function () {
							$ajaxTable.refresh();
						});
					});
				}
			}, extendParameters || {}));
		},
		showConfirmActionForRows: function (rows, okLabel, cancelLabel,
			titleTemplate, messageTemplate, callback, extendParameters) {
			// 显示需要确认的批量操作模态框
			// 标题和显示信息根据模板和数据生成
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
		}
	};
	this.data("ajaxTableHelper", helper);
	return helper;
};

$.fn.closestAjaxTableHelper = function () {
	// 获取离元素最近的ajaxTable帮助类
	return $(this).closestAjaxTable().container.ajaxTableHelper();
};
