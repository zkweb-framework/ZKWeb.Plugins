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
		<input type="checkbox" value="A" data-merge-to="ExampleList">
		<input type="checkbox" value="B" data-merge-to="ExampleList">
		<input type="hidden" name="ExampleList" value="[]" />
		使用 $("[name='ExampleList']").checkboxMerging() 初始化
		之后当A和B选中或取消选中时，ExampleList中的值会随着改变
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
	$.fn.checkboxMerging = function () {
		var $this = $(this);
		var name = $this.attr("name");
		if (!name) {
			return; // name不能等于空，否则会绑定到所有多选框
		}
		var values = JSON.parse($this.val() || "[]");
		($(checkboxSelector)
			.filter(function () { return $(this).data("merge-to") == name; })
			.each(function () {
				var $checkbox = $(this);
				$checkbox.prop("checked", _.contains(values, $checkbox.val())).trigger("change");
			})
			.on("change", function () {
				var $checkbox = $(this);
				var value = $checkbox.val();
				values = ($checkbox.is(":checked") ?
					_.union(values, [value]) : _.without(values, value));
				$this.val(JSON.stringify(values));
			}));
	};
});
