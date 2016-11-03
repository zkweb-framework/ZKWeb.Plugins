/*
	订单列表页面的功能，会同时用在详情页
*/

/* 订单操作按钮 - 已禁止 */
$(function () {
	$(document).on("click", ".btn-order-action.action-disabled", function () {
		var $this = $(this);
		var message = $this.attr("data-message");
		$.toast({ icon: "warning", text: message });
	});
});

/* 订单操作按钮 - 模态框 */
$(function () {
	$(document).on("click", ".btn-order-action.action-modal", function () {
		var $this = $(this);
		var title = $this.attr("data-title");
		var url = $this.attr("data-url");
		var dialogParameters = $this.data("dialog-parameters");
		BootstrapDialog.showRemote(title, url, $.extend({
			size: "size-wide",
			onshow: function (dialog) {
				var $modal = dialog.getModal();
				$modal.one("updated.ajaxTable", function () {
					$modal.one("hide.bs.modal", function () {
						// 包含订单列表时: 刷新ajax表格
						var table = $(".order-table-container").closestAjaxTable();
						table && table.refresh();
						// 模态框详情页: 刷新里面的内容
						// 单独页面详情页: 刷新整个页面
						var $prevModal = $modal.prevAll(".bootstrap-dialog");
						if ($prevModal.length) {
							$prevModal.find(".remote-contents").trigger("reload");
						} else if (!table) {
							location.href = location.href;
						}
					});
				});
			}
		}, dialogParameters || {}));
	});
});
