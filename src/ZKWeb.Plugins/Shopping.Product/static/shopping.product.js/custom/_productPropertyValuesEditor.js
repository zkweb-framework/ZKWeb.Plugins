/*
	商品属性值的编辑器
	编辑商品属性时使用
*/

// 商品属性值的编辑器
// 例 $("#Editor").productPropertyValuesEditor();
// 元素需要有以下属性
//	data-property-values-name 储存属性值列表的Json的字段名称
// 元素可以有以下属性
//	data-table-class 表格使用的css类
//	data-table-header-class 表格头使用的css类
//	data-translations 翻译文本
$.fn.productPropertyValuesEditor = function () {
	var $editor = $(this);
	// 避免重复初始化
	if ($editor.data("initialized")) {
		return;
	}
	$editor.data("initialized", true);
	// 获取属性和对应的字段
	var propertyValuesName = $editor.data("property-values-name");
	var tableClass = $editor.data("table-class");
	var tableHeaderClass = $editor.data("data-table-header-class");
	var translations = $editor.data("translations") || {};
	var $propertyValuesJson = $editor.closest("form").find("[name='" + propertyValuesName + "']");
	if ($propertyValuesJson.length <= 0) {
		console.warn("init product property values editor failed", $propertyValuesJson);
		return;
	}
	// 获取翻译
	var T = function (text) { return translations[text] || text; };
	var T_Name = T("Name");
	var T_Remark = T("Remark");
	// 添加表格元素
	var $table = $editor.editableTable({
		tableClass: tableClass,
		tableHeaderClass: tableHeaderClass,
		columns: [T_Name, T_Remark]
	});
	// 绑定添加行的事件
	// 名称，备注，操作
	var $tableBody = $table.find("tbody");
	var addRowEventName = "addRow.editableTable";
	$table.on(addRowEventName, function (e, data) {
		var $row = $("<tr>");
		// 添加单元格
		_.each([["name", T_Name], ["remark", T_Remark]], function (pair) {
			$row.append(
				$("<td>").addClass(pair[0]).append(
				$("<input>").addClass("form-control").attr("type", "text").attr("placeholder", pair[1])));
		});
		$row.append($table.data("actionsCell").clone());
		// 绑定单元格
		$row.data("property-value-id", data.Id);
		$row.find(".name input").val(data.Name);
		$row.find(".remark input").val(data.Remark);
		// 添加行到表格
		$tableBody.append($row);
	});
	// 绑定属性值的事件
	var bindEventName = "bind.editableTable";
	var collectEventName = "collect.editableTable";
	var bindLockName = "bindLock.editableTable";
	$table.on(bindEventName, function () {
		// 设置绑定锁，绑定时不能触发收集事件
		$editor.data(bindLockName, true);
		// 清空表格
		$tableBody.empty();
		// 按属性值添加行
		var propertyValues = JSON.parse($propertyValuesJson.val() || "[]") || [];
		_.each(propertyValues, function (value) { $table.trigger(addRowEventName, value); });
		// 解除绑定锁
		$editor.data(bindLockName, false);
	});
	// 收集属性值的事件
	$table.on(collectEventName, function () {
		// 绑定时跳过收集事件
		if ($editor.data(bindLockName)) {
			return;
		}
		// 枚举行
		var propertyValues = [];
		var $rows = $tableBody.find("> tr");
		$rows.each(function () {
			var $row = $(this);
			propertyValues.push({
				Id: $row.data("property-value-id") || null,
				Name: $row.find(".name input").val(),
				Remark: $row.find(".remark input").val()
			});
		});
		// 设置收集到的数据到$propertyValuesJson
		$propertyValuesJson.val(JSON.stringify(propertyValues));
	});
	// 初始化时手动触发绑定属性值的事件
	$table.trigger(bindEventName);
};

// 自动初始化带[data-toggle='product-property-values-editor']属性的编辑器
$(function () {
	var setup = function ($elements) {
		$elements.each(function () { $(this).productPropertyValuesEditor(); });
	};
	var rule = "[data-toggle='product-property-values-editor']";
	setup($(rule));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(rule));
	});
});
