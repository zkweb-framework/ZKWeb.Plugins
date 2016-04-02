/*
	商品匹配数据编辑器
*/

// 商品匹配数据编辑器
// 例 $("#Editor").productMatchedDataEditor();
// 元素需要有以下属性
//	data-category-id-selector 储存类目Id的元素的选择器
//	data-matched-data-json-selector 储存匹配数据列表的Json的元素的选择器
// 元素可以有以下属性
//	data-table-class 表格的Css类
//	data-table-header-class 表格头部的Css类
//	data-resources-condition "条件"的本地化显示名称
//	data-resources-default "默认"的本地化显示名称
$.fn.productMatchedDataEditor = function () {
	var $editor = $(this);
	// 避免重复初始化
	if ($editor.data("initialized")) {
		return true;
	}
	$editor.data("initialized", true);
	// 获取类目Id和匹配数据列表Json的元素，不存在时返回
	var $categoryId = $($editor.attr("data-category-id-selector"));
	var $matchedDataJson = $($editor.attr("data-matched-data-json-selector"));
	if (!$categoryId.length || !$matchedDataJson.length) {
		console.warn("init product matched data editor failed", $categoryId, $matchedDataJson);
		return;
	}
	// 绑定匹配数据表格的事件
	// 根据$matchedDataJson的内容绑定匹配数据表格
	// 远程载入绑定器后需要触发这个事件
	var bindEventName = "bind.productMatchedDataEditor"; // 属于$editor
	var bindingLockName = "bindingLock"; // 绑定锁的名称
	var binders = { ConditionBinders: null, AffectsBinders: null }; // 从远程载入的绑定器
	var collectEventName = "collect.productMatchedDataEditor"; // 属于$editor
	var conditionCellUpdateEventName = "update.productMatchedDataEditor"; // 属于".condition-cell"
	$editor.on(bindEventName, function () {
		// 设置绑定锁，绑定时不能触发收集事件
		$editor.data(bindingLockName, true);
		// 获取匹配数据列表，并添加默认条件
		// 添加后再设置回去（添加商品时不操作这个编辑器也可以传回默认条件）
		var matchedData = JSON.parse($matchedDataJson.val() || "[]");
		if (!matchedData.length || !$.isEmptyObject(_.last(matchedData).Conditions)) {
			matchedData.push({}); // 自动添加默认条件到最后
		}
		$matchedDataJson.val(JSON.stringify(matchedData));
		// 添加表格，原表格会被删除
		$editor.html("<table><thead><tr></tr></thead><tbody></tbody></table>");
		var $table = $editor.find("table");
		var $tableHeader = $table.find("thead > tr");
		var $tableBody = $table.find("tbody");
		$table.addClass($editor.attr("data-table-class"));
		$tableHeader.addClass($editor.attr("data-table-header-class"));
		// 添加表格列
		// 条件列, 根据影响数据绑定器生成的列..., 操作列
		$tableHeader.append($("<th width='30%'>").append(
			$editor.attr("data-resources-condition")));
		_.each(binders.AffectsBinders, function (affectsBinder) {
			$tableHeader.append($("<th>").append(affectsBinder.Header));
		});
		$tableHeader.append($("<th width='100'>").append($("<div class='actions'>").append(
			"<a class='btn-xs btn-primary add-data'><i class='fa fa-plus'></i></a>")));
		// 添加表格行
		// 一条匹配数据对应一行，每行有 条件单元格, 根据影响数据绑定器生成的单元格..., 操作单元格
		// addRow函数在点击添加按钮时也会触发，prepend等于true
		var addRow = function (data, prepend) {
			var $row = $("<tr>");
			// 添加条件单元格
			var $conditionCell = $("<td class='condition-cell'>" +
				"<span class='condition-string'></span>" +
				"<div class='btn-group'>" +
					"<a data-toggle='dropdown' class='btn-xs btn-primary'><i class='fa fa-pencil'></i></a>" +
					"<ul class='dropdown-menu'>" +
						"<div class='form form-horizontal'><div class='form-body'></div></div>" +
					"</ul>" +
				"</div>" +
			"</td>");
			// 添加条件的下拉编辑框，根据条件绑定器生成
			// 每行中一个条件绑定器对应一个".condition-binder"元素
			var $conditionForm = $conditionCell.find(".form");
			_.each(binders.ConditionBinders, function (conditionBinder) {
				var $binder = $("<div class='condition-binder'>").append(conditionBinder.Contents);
				conditionBinder.BindFunction($binder, data.Conditions || {}); // 调用绑定函数
				$binder.data("CollectFunction", conditionBinder.CollectFunction); // 指定收集函数
				$binder.data("DisplayFunction", conditionBinder.DisplayFunction); // 指定显示函数
				$conditionForm.append($binder);
			});
			// 绑定条件文本的重新生成事件
			// 例如下拉中颜色选黑色，尺码选M的时候，文本显示"颜色: 黑色 尺码: M"
			// 文本等于空时显示"默认"，表示无条件
			$conditionCell.on(conditionCellUpdateEventName, function () {
				var conditionString = "";
				$conditionCell.find(".condition-binder").each(function () {
					var $binder = $(this);
					conditionString += $binder.data("DisplayFunction")($binder, data.Conditions || {});
				});
				$conditionCell.find(".condition-string").text(
					conditionString.trim() || $editor.attr("data-resources-default") || "Default");
			});
			$conditionCell.trigger(conditionCellUpdateEventName);
			$row.append($conditionCell);
			// 添加影响数据的单元格，根据影响数据绑定器生成
			// 每行中一个影响数据绑定器对应一个".affects-binder"元素
			_.each(binders.AffectsBinders, function (affectsBinder) {
				var $binder = $("<td class='affects-binder'>").append(affectsBinder.Contents);
				affectsBinder.BindFunction($binder, data.Affects || {}); // 调用绑定函数
				$binder.data("CollectFunction", affectsBinder.CollectFunction); // 指定收集函数
				$row.append($binder);
			});
			// 添加操作按钮，包括上下删除
			$row.append($("<td>").append($("<div class='actions'>")
				.append("<a class='btn-xs btn-primary up-data'><i class='fa fa-arrow-up'></i></a>")
				.append("<a class='btn-xs btn-primary down-data'><i class='fa fa-arrow-down'></i></a>")
				.append("<a class='btn-xs btn-primary remove-data'><i class='fa fa-remove'></i></a>")));
			// 绑定所有文本框和下拉框的改变事件
			// 改变后需要触发收集事件
			$row.find("input, select").on("change", function () {
				$editor.trigger(collectEventName);
			});
			// 绑定上下删除按钮的事件
			// 执行后需要触发收集事件
			$row.find(".actions .up-data").on("click", function () {
				var $prev = $row.prev();
				$prev.length && $prev.before($row);
				$editor.trigger(collectEventName);
			});
			$row.find(".actions .down-data").on("click", function () {
				var $next = $row.next();
				$next.length && $next.after($row);
				$editor.trigger(collectEventName);
			});
			$row.find(".actions .remove-data").on("click", function () {
				$row.remove();
				$editor.trigger(collectEventName);
			});
			// 添加行到单元格中
			prepend ? $tableBody.prepend($row) : $tableBody.append($row);
		};
		_.each(matchedData, function (data) { addRow(data); });
		// 最后一行隐藏下拉编辑框
		$table.find("> tbody > tr").last().find(".condition-cell .btn-group").hide();
		// 绑定添加按钮的事件
		// 添加后需要触发收集事件
		$table.find(".add-data").off("click").on("click", function () {
			addRow({}, true);
			$editor.trigger(collectEventName);
		});
		// 解除绑定锁
		// console.log("bind done", binders, matchedData);
		$editor.data(bindingLockName, false);
	});
	// 收集匹配数据的事件
	// 收集到的匹配数据保存在$matchedDataJson中
	// 匹配数据表格中的输入框改变后需要触发这个事件
	$editor.on(collectEventName, function () {
		// 绑定时跳过收集事件
		if ($editor.data(bindingLockName)) {
			return;
		}
		// 枚举表格中的行
		var matchedData = [];
		var $rows = $editor.find("table > tbody > tr");
		$rows.each(function () {
			var $row = $(this);
			var data = { Conditions: {}, Affects: {} };
			// 更新条件文本
			$row.find(".condition-cell").trigger(conditionCellUpdateEventName);
			// 收集条件
			$row.find(".condition-binder").each(function () {
				var $binder = $(this);
				$binder.data("CollectFunction")($binder, data.Conditions);
			});
			// 收集影响数据
			$row.find(".affects-binder").each(function () {
				var $binder = $(this);
				$binder.data("CollectFunction")($binder, data.Affects);
			});
			matchedData.push(data);
		});
		// 设置收集到的数据到$matchedDataJson中
		// console.log(collectEventName, matchedData);
		$matchedDataJson.val(JSON.stringify(matchedData));
	});
	// 类目改变时的处理，初始化时也触发这里的处理
	var onCategoryIdChanged = function () {
		// 检测类目是否有改变，无改变时返回
		var categoryId = $categoryId.val();
		var previousCategoryId = $editor.data("categoryId");
		if (previousCategoryId == categoryId) {
			return;
		}
		$editor.data("categoryId", categoryId);
		// 远程载入类目对应的绑定器
		$.get("/admin_products/matched_data_binders?categoryId=" + categoryId, function (remoteBinders) {
			// 更新绑定器，绑定器中的函数需要预先编译
			var evalFunc = function (body) { return eval("(" + body + " || function() {})"); };
			_.each(remoteBinders.ConditionBinders, function (conditionBinder) {
				conditionBinder.BindFunction = evalFunc(conditionBinder.Bind);
				conditionBinder.CollectFunction = evalFunc(conditionBinder.Collect);
				conditionBinder.DisplayFunction = evalFunc(conditionBinder.Display);
			});
			_.each(remoteBinders.AffectsBinders, function (affectBinder) {
				affectBinder.BindFunction = evalFunc(affectBinder.Bind);
				affectBinder.CollectFunction = evalFunc(affectBinder.Collect);
			});
			binders = remoteBinders;
			// 触发绑定匹配数据表格的事件
			// 类目改变，且原有的类目不是无类目时，清空原有的匹配数据列表
			if (previousCategoryId && previousCategoryId != categoryId) {
				$matchedDataJson.val("");
			}
			$editor.trigger(bindEventName);
		});
	};
	$categoryId.on("change", onCategoryIdChanged);
	onCategoryIdChanged.call($categoryId);
};
