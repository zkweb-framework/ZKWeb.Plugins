/*
	多选框相关的功能

	多选框组，支持全选/取消全选
		<div data-toggle="checkbox-group">
			<input type="checkbox" class="select-all">
			<input type="checkbox">
			<input type="checkbox">
		</div>
		点击全选多选框时选中或取消选中同一分组下的其他多选框
		同一分组的多选框除了全选外全部选中时，把全选也选中

	支持储存多个多选框的值到一个控件中
		<input type="checkbox" value="A" merge-to="input[name=ExampleList]">
		<input type="checkbox" value="B" merge-to="input[name=ExampleList]">
		<input type="hidden" name="ExampleList" value="[]" />
		当A和B选中或取消选中时，ExampleList中的值会随着改变
*/

$(function () {
	// 多选框组，支持全选/取消全选
	var groupSelector = "[data-toggle='checkbox-group']";
	var checkboxSelector = "input[type='checkbox']";
	$(document).on("change", groupSelector + " " + checkboxSelector, function () {
		var $this = $(this);
		var selectAllSelector = ".select-all";
		var changingMask = "changingBySelectAll";
		// 避免重复触发事件影响性能
		if ($this.data(changingMask)) {
			return;
		}
		// 点击全选多选框时选中或取消选中同一分组下的其他多选框
		if ($this.is(selectAllSelector)) {
			($this.closest(groupSelector).find(checkboxSelector).not($this)
				.data(changingMask, true)
				.prop("checked", $this.is(":checked"))
				.trigger("change")
				.removeData(changingMask));
		}
		// 同一分组的多选框除了全选外全部选中时，把全选也选中
		// 注意多级checkbox-group时不能删除上级的全选用的多选框，否则会导致误选$selectAll
		$this.parents(groupSelector).each(function () {
			var $checkboxes = $(this).find(checkboxSelector);
			var $selectAll = $checkboxes.filter(selectAllSelector).first();
			var $targets = $checkboxes.not($selectAll);
			($selectAll.data(changingMask, true)
				.prop("checked", ($targets.length == $targets.filter(":checked").length))
				.trigger("change")
				.removeData(changingMask));
		});
	});

	// 支持储存多个多选框的值到一个控件中
	var checkboxWithMergeToSelector = checkboxSelector + "[merge-to]";
	$(document).on("change", checkboxWithMergeToSelector, function () {
		var $checkbox = $(this);
		var $target = $($checkbox.attr("merge-to"));
		var value = $checkbox.val();
		var values = $target.data("values") || _.map(JSON.parse($target.val() || "[]"), String);
		values = $checkbox.is(":checked") ? _.union(values, [value]) : _.without(values, value);
		$target.data("values", values);
		$target.val(JSON.stringify(values));
	});

	// 页面载入时自动绑定值
	var setup = function (checkboxes, targetSource) {
		$(checkboxes).each(function () {
			var $checkbox = $(this);
			var $target = $(targetSource).find($checkbox.attr("merge-to"));
			var values = $target.data("values") || _.map(JSON.parse($target.val() || "[]"), String);
			$target.data("values", values);
			$checkbox.prop("checked", _.contains(values, $checkbox.val()));
		});
	};
	setup($(checkboxWithMergeToSelector), document);
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(checkboxWithMergeToSelector), contents);
	});
});
