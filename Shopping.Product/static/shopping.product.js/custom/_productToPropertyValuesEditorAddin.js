/*
	商品关联的属性值的编辑器的扩展
	编辑商品时使用
	功能有
		设置商品匹配数据编辑器的规格下拉框样式，粗体显示选中的规格值，修改别名颜色
*/

// 设置商品匹配数据编辑器的规格下拉框样式
// 触发条件
// 已获取到商品属性编辑器和匹配数据编辑器
// 以下事件执行
//		bind.editableTable (table元素)
//		collect.editableTable (table元素)
//		bind.productToPropertyValuesEditor
//		collect.productToPropertyValuesEditor
// 注意
// 不适用于一个页面上有多个编辑器的情况
// 编辑器必须有product-matched-data-editor和product-to-property-values-editor的Css类，否则不会生效
$(function () {
	var $productMatchedDataEditor = null;
	var $productToPropertyValuesEditor = null;
	var updateOptionsStyle = function () {
		if (!$productMatchedDataEditor || !$productToPropertyValuesEditor) {
			return;
		}
		// 获取选中的多选项和别名
		// 数据格式 { 属性Id: [{ PropertyValueId: 属性值Id, name: 名称, color: 名称的颜色 }, ...] }
		var selectedMapping = {};
		$productToPropertyValuesEditor.find("[data-property-type]").each(function () {
			var $property = $(this);
			var propertyType = $property.attr("data-property-type");
			var propertyId = $property.data("property-id");
			if (propertyType == "checkbox") {
				var selectedList = [];
				$property.find("input[type='checkbox']:checked").each(function () {
					var $checkbox = $(this);
					var $aliasText = $checkbox.parent().find(".alias-text");
					var text = $aliasText.text().trim(); // 属性值名称
					var color = $aliasText.css("color"); // 名称的颜色
					var value = $checkbox.val(); // 属性值Id
					value && selectedList.push({ propertyValueId: value, name: text, color: color });
				});
				selectedMapping[propertyId] = selectedList;
			}
		});
		// 设置商品匹配数据编辑器的规格下拉框选项
		// 隐藏没有选中的规格值
		// console.log(selectedMapping);
		$productMatchedDataEditor.find(".condition-binder [data-property-id]").each(function () {
			var $property = $(this);
			var propertyId = $property.data("property-id");
			var selectedValueMapping = _.indexBy(selectedMapping[propertyId], "propertyValueId");
			$property.find("option").each(function () {
				var $option = $(this);
				if (!$option.val()) {
					return;
				}
				var selectValue = selectedValueMapping[$option.val()];
				selectValue ? $option.show() : $option.hide(); // 隐藏没有选中的规格值
				selectValue && $option.text(selectValue.name); // 修改名称，对应别名的修改
			});
		});
		// 更新条件文本，对应别名的修改
		$productMatchedDataEditor.find(".condition-cell").trigger("update.productMatchedDataEditor");
		// 更新完毕
		// console.log("options style updated");
	};
	$(document).on("bind.editableTable", ".product-matched-data-editor table", function () {
		$productMatchedDataEditor = $(this);
		updateOptionsStyle();
	});
	$(document).on("collect.editableTable", ".product-matched-data-editor table", function () {
		$productMatchedDataEditor = $(this);
		updateOptionsStyle();
	});
	$(document).on("bind.productToPropertyValuesEditor", ".product-to-property-values-editor", function () {
		$productToPropertyValuesEditor = $(this);
		updateOptionsStyle();
	});
	$(document).on("collect.productToPropertyValuesEditor", ".product-to-property-values-editor", function () {
		$productToPropertyValuesEditor = $(this);
		updateOptionsStyle();
	});
})
