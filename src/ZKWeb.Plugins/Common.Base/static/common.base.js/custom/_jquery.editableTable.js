/*
	可编辑的表格
	支持添加和删除行，定义收集和绑定事件等功能
	例子
	var $table = $editor.editableTable({
		columns: [ "A", "B", $("<div>") ],
		tableClass: "table table-bordered table-hover",
		tableHeaderClass: "heading"
	});
	$table.on("addRow.editableTable", function (e, data) { ... });
	$table.on("collect.editableTable", function () { ... });
	$table.on("bind.editableTable", function () { ... }); // 绑定事件需要自己来触发
*/

$.fn.editableTable = function (options) {
	// 已生成过时返回之前生成的对象
	var $table = this.data("editableTable");
	if ($table) {
		return $table;
	}
	// 元素不存在时返回undefined
	if (!this.length) {
		return;
	}
	// 设置默认选项
	options = $.extend({
		columns: [],
		tableClass: "table table-bordered table-hover table-editable",
		tableHeaderClass: "heading"
	}, options || {});
	// 添加表格元素
	$table = $("<table>").addClass(options.tableClass).appendTo(this);
	$tableHead = $("<tr>").appendTo($("<thead>").appendTo($table));
	$tableBody = $("<tbody>").appendTo($table);
	_.each(options.columns, function (column) {
		var $th = $("<th>").addClass(options.tableHeaderClass);
		(column instanceof jQuery) ? $th.append(column) : $th.text(column);
		$tableHead.append($th);
	});
	$table.data("actionsHead", $("<th width='100'>")
		.addClass(options.tableHeaderClass)
		.append($("<div class='actions'>")
		.append("<a class='btn-xs btn-primary add-data'><i class='fa fa-plus'></i></a>")));
	$tableHead.append($table.data("actionsHead"));
	// 设置默认的操作单元格元素
	$table.data("actionsCell", $("<td>").append($("<div class='actions'>")
		.append("<a class='btn-xs btn-primary up-data'><i class='fa fa-arrow-up'></i></a>")
		.append("<a class='btn-xs btn-primary down-data'><i class='fa fa-arrow-down'></i></a>")
		.append("<a class='btn-xs btn-primary remove-data'><i class='fa fa-remove'></i></a>")));
	// 绑定行操作事件，支持添加删除移动行等
	// 行改变后触发收集事件
	var addRowEventName = "addRow.editableTable";
	var collectEventName = "collect.editableTable";
	var bindEventName = "bind.editableTable";
	$table.on("click", ".actions .add-data", function () {
		$table.trigger(addRowEventName, [{}]); // 这里添加的是空数据
		$table.trigger(collectEventName);
	});
	$table.on("click", ".actions .up-data", function () {
		var $row = $(this).closest("tr");
		var $prev = $row.prev();
		$prev.length && $prev.before($row);
		$table.trigger(collectEventName);
	});
	$table.on("click", ".actions .down-data", function () {
		var $row = $(this).closest("tr");
		var $next = $row.next();
		$next.length && $next.after($row);
		$table.trigger(collectEventName);
	});
	$table.on("click", ".actions .remove-data", function () {
		var $row = $(this).closest("tr");
		$row.remove();
		$table.trigger(collectEventName);
	});
	// 在文本框或下拉框改变后触发收集事件
	$table.on("change", "input, select", function () {
		$table.trigger(collectEventName);
	});
	// 返回表格元素
	this.data("editableTable", $table);
	return $table;
};
