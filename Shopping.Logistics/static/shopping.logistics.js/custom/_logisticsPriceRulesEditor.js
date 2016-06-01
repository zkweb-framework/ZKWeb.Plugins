/*
	物流运费规则编辑器
	格式
	<div data-trigger='logistics-price-rules-editor'
		data-price-rules-name='储存规则的字段名称'
		data-table-class='表格使用的css类，可选'
		data-table-header-class='表格头使用的css类，可选'
		data-display-country-dropdown='是否显示国家下拉框'
		data-currency-list-items='货币的选项列表'
		data-default-price-rule='默认运费规则'
		data-translations='翻译内容'>
	</div>
*/

$.fn.logisticsPriceRulesEditor = function () {
	var $editor = $(this);
	// 避免重复初始化
	if ($editor.data("initialized")) {
		return;
	}
	$editor.data("initialized", true);
	// 获取属性和对应的字段
	var priceRulesName = $editor.data("price-rules-name");
	var tableClass = $editor.data("table-class");
	var tableHeaderClass = $editor.data("table-header-class");
	var displayCountryDropdown = $editor.data("display-country-dropdown");
	var currencyListItems = $editor.data("currency-list-items");
	var defaultPriceRule = $editor.data("default-price-rule");
	var translations = $editor.data("translations") || {};
	var $priceRulesJson = $editor.closest("form").find("[name='" + priceRulesName + "']");
	if ($priceRulesJson.length <= 0) {
		console.warn("init logistics price rule editor failed", $priceRulesJson);
		return;
	}
	// 获取翻译
	var T = function (text) { return translations[text] || text };
	var T_Default = T("Default");
	var T_Region = T("Region");
	var T_FirstHeavyUnit = T("FirstHeavyUnit(g)");
	var T_FirstHeavyCost = T("FirstHeavyCost");
	var T_ContinuedHeavyUnit = T("ContinuedHeavyUnit(g)");
	var T_ContinuedHeavyCost = T("ContinuedHeavyCost");
	var T_Currency = T("Currency");
	var T_Disabled = T("Disabled");
	// 添加表格元素
	var $table = $editor.editableTable({
		tableClass: tableClass,
		tableHeaderClass: tableHeaderClass,
		columns: [
			T_Region, T_FirstHeavyUnit, T_FirstHeavyCost,
			T_ContinuedHeavyUnit, T_ContinuedHeavyCost, T_Currency, T_Disabled]
	});
	// 绑定添加行的事件
	// 地区, 首重, 首重费用, 续重, 续重费用, 货币, 已禁用, 操作
	var $tableBody = $table.find("tbody");
	var addRowEventName = "addRow.editableTable";
	$table.on(addRowEventName, function (e, data) {
		var $row = $("<tr>");
		// 添加单元格
		$row.append(
			$("<td>").addClass("region").append(
			$("<div>").addClass("region-editor")
				.attr("data-trigger", "region-editor")
				.attr("data-display-country-dropdown", "" + displayCountryDropdown).append(
			$("<input>").attr("type", "hidden"))));
		_.each([
			["first-heavy-unit", T_FirstHeavyUnit],
			["first-heavy-cost", T_FirstHeavyCost],
			["continued-heavy-unit", T_ContinuedHeavyUnit],
			["continued-heavy-cost", T_ContinuedHeavyCost]], function (pair) {
				$row.append(
					$("<td>").addClass(pair[0]).append(
					$("<input>").addClass("form-control").attr("type", "text").attr("placeholder", pair[1])));
			});
		$row.append($("<td>").addClass("currency").append(
			$("<select>").addClass("form-control").append(_.map(currencyListItems, function (item) {
				return $("<option>").attr("value", item.Value).text(item.Name);
			}))));
		$row.append($("<td>").addClass("disabled").append($("<input>").attr("type", "checkbox")));
		$row.append($table.data("actionsCell").clone());
		// 绑定单元格
		var rule = $.isEmptyObject(data) ? defaultPriceRule : data;
		var $regionEditor = $row.find(".region .region-editor");
		var $regionField = $regionEditor.find("input[type=hidden]");
		var regionValue = JSON.parse($regionField.val() || "{}");
		regionValue.Country = rule.Country; // 国家 null或数值
		regionValue.RegionId = rule.RegionId; // 地区Id null或数值
		$regionField.val(JSON.stringify(regionValue));
		$row.find(".first-heavy-unit input").val(rule.FirstHeavyUnit); // 首重单位
		$row.find(".first-heavy-cost input").val(rule.FirstHeavyCost); // 首重费用
		$row.find(".continued-heavy-unit input").val(rule.ContinuedHeavyUnit); // 续重单位
		$row.find(".continued-heavy-cost input").val(rule.ContinuedHeavyCost); // 续重费用
		$row.find(".currency select").val(rule.Currency); // 货币
		$row.find(".disabled input").prop("checked", rule.Disabled); // 已禁用
		// 国家等于null时显示文本"默认"，否则初始化地区编辑器
		if (!(regionValue.Country || false)) {
			$row.find(".region").empty().text(T_Default);
		} else {
			$regionEditor.regionEditor();
		}
		// 添加行到表格
		$.isEmptyObject(data) ? $tableBody.prepend($row) : $tableBody.append($row);
	});
	// 绑定运费规则的事件
	var bindEventName = "bind.editableTable";
	var collectEventName = "collect.editableTable";
	var bindLockName = "bindLock.editableTable";
	$table.on(bindEventName, function () {
		// 设置绑定锁，绑定时不能触发收集事件
		$editor.data(bindLockName, true);
		// 获取当前的运费规则，并添加默认条件
		// 添加后再设置回去（添加物流时不操作这个编辑器也可以传回默认条件）
		var priceRules = JSON.parse($priceRulesJson.val() || "[]") || [];
		if (!priceRules.length || !$.isEmptyObject(_.last(priceRules).Conditions)) {
			priceRules.push($.extend({}, defaultPriceRule, { Country: null }));
		}
		$priceRulesJson.val(JSON.stringify(priceRules));
		// 清空表格
		$tableBody.empty();
		// 按规则添加行
		_.each(priceRules, function (rule) { $table.trigger(addRowEventName, [rule]); });
		// 解除绑定锁
		$editor.data(bindLockName, false);
	});
	// 收集运费规则的事件
	$editor.on(collectEventName, function () {
		// 绑定时跳过收集事件
		if ($editor.data(bindLockName)) {
			return;
		}
		// 枚举行
		var priceRules = [];
		var $rows = $tableBody.find("> tr");
		$rows.each(function () {
			var $row = $(this);
			var $regionEditor = $row.find(".region .region-editor");
			var $regionField = $regionEditor.find("input[type=hidden]");
			var regionValue = JSON.parse($regionField.val() || "{}");
			priceRules.push({
				Country: regionValue.Country,
				RegionId: regionValue.RegionId,
				FirstHeavyUnit: $row.find(".first-heavy-unit input").val(),
				FirstHeavyCost: $row.find(".first-heavy-cost input").val(),
				ContinuedHeavyUnit: $row.find(".continued-heavy-unit input").val(),
				ContinuedHeavyCost: $row.find(".continued-heavy-cost input").val(),
				Currency: $row.find(".currency select").val(),
				Disabled: $row.find(".disabled input").is(":checked")
			});
		});
		// 设置收集到的数据到$priceRulesJson中
		$priceRulesJson.val(JSON.stringify(priceRules));
	});
	// 初始化时手动触发绑定运费规则的事件
	$table.trigger(bindEventName);
};

// 自动初始化带[data-toggle='logistics-price-rules-editor']属性的编辑器
$(function () {
	var setup = function ($elements) {
		$elements.each(function () { $(this).logisticsPriceRulesEditor(); });
	};
	var logisticsPriceRulesEditorSelector = "[data-toggle='logistics-price-rules-editor']";
	setup($(logisticsPriceRulesEditorSelector));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(logisticsPriceRulesEditorSelector));
	});
});
