/*
	商品关联的属性值的编辑器
	编辑商品时使用
*/

// 商品关联的属性值的编辑器
// 例 $("#Editor").productToPropertyValuesEditor();
// 元素需要有以下属性
//	data-category-id-name 储存类目Id的字段名称
//	data-property-values-name 储存属性值列表的Json的字段名称
$.fn.productToPropertyValuesEditor = function () {
	var $editor = $(this);
	// 避免重复初始化
	if ($editor.data("initialized")) {
		return true;
	}
	$editor.data("initialized", true);
	// 获取类目Id和属性值列表Json的元素，不存在时返回
	var $categoryId = $editor.closest("form").find("[name='" + $editor.attr("data-category-id-name") + "']");
	var $propertyValuesJson = $editor.closest("form").find("[name='" + $editor.attr("data-property-values-name") + "']");
	if (!$categoryId.length || !$propertyValuesJson.length) {
		console.warn("init product property editor failed", $categoryId, $propertyValuesJson);
		return;
	}
	// 绑定属性值的事件
	// 根据$propertyValuesJson的内容绑定属性值控件
	// 远程载入属性编辑器后需要触发这个事件
	var bindEventName = "bind.productToPropertyValuesEditor";
	var bindingLockName = "bindingLock";
	var propertyTypeAttribute = "data-property-type";
	var propertyTypeSelector = "[" + propertyTypeAttribute + "]";
	var setPropertyName = function ($name, text) {
		// 修改属性值名称的函数，有变化时同时改变字体颜色
		var changed = $name.text().trim() != text.trim();
		changed && $name.text(text).css("color", "#31708F");
	}
	$editor.on(bindEventName, function () {
		$editor.data(bindingLockName, true); // 设置绑定锁，绑定时不能触发收集事件
		var values = JSON.parse($propertyValuesJson.val() || "[]"); // 属性值列表
		var valueMapping = _.groupBy(values, "propertyId"); // { "属性Id": [ 属性值数据, ... ], ... }
		$editor.find(propertyTypeSelector).each(function () {
			var $property = $(this);
			var propertyType = $property.attr(propertyTypeAttribute);
			var propertyId = $property.data("property-id");
			if (propertyType == "textbox") {
				// 文本框，填写的内容等于名称
				var value = _.first(valueMapping[propertyId]);
				$property.find("input[type='text']").val(value ? value.name : "").change();
			} else if (propertyType == "checkbox") {
				// 多选框，选中项等于存在的属性值，显示文本等于名称
				var selectedMapping = _.indexBy(
					valueMapping[propertyId], "propertyValueId"); // { "属性值Id": 属性值数据, ... }
				$property.find("input[type='checkbox']").each(function () {
					var $checkbox = $(this);
					var value = selectedMapping[$checkbox.val()];
					$checkbox.prop("checked", value ? true : false).change();
					value && setPropertyName($checkbox.parent().find(".alias-text"), value.name);
				});
			} else if (propertyType == "dropdown-list") {
				// 下拉框，选中值等于属性值
				var value = _.first(valueMapping[propertyId]);
				$property.find("select").val(value ? value.propertyValueId : "").change();
			} else if (propertyType == "radio-button") {
				// 单选按钮，选中值等于属性值
				var value = _.first(valueMapping[propertyId]);
				value && $property.find("input[type='radio']").filter(function () {
					return $(this).val() == value.propertyValueId;
				}).prop("checked", true);
			} else if (propertyType == "editable-dropdown-list") {
				// 可编辑的下拉框，选中值等于属性值，文本等于名称
				var value = _.first(valueMapping[propertyId]);
				$property.find("select").val(value ? value.propertyValueId : "").change();
				$property.find("input[type='text']").val(value ? value.name : "").change();
			}
		});
		$editor.data(bindingLockName, false); // 解除绑定锁
	});
	// 收集属性值的事件
	// 收集到的属性值保存在$propertyValuesJson中
	// 属性值的输入框改变后需要触发这个事件
	var collectEventName = "collect.productToPropertyValuesEditor";
	$editor.on(collectEventName, function () {
		if ($editor.data(bindingLockName)) {
			return; // 绑定时跳过收集事件
		}
		var values = []; // 属性值列表
		$editor.find(propertyTypeSelector).each(function () {
			var $property = $(this);
			var propertyType = $property.attr(propertyTypeAttribute);
			var propertyId = $property.data("property-id");
			if (propertyType == "textbox") {
				// 文本框，无属性值，填写的内容作为名称
				var value = $property.find("input[type='text']").val(); // 属性值名称
				value && values.push({ propertyId: propertyId, name: value });
			} else if (propertyType == "checkbox") {
				// 多选框，属性值等于选中值，显示的文本作为名称
				$property.find("input[type='checkbox']:checked").each(function () {
					var $checkbox = $(this);
					var text = $checkbox.parent().find(".alias-text").text().trim(); // 属性值名称
					var value = $checkbox.val(); // 属性值Id
					value && values.push({ propertyId: propertyId, propertyValueId: value, name: text });
				});
			} else if (propertyType == "dropdown-list") {
				// 下拉框，属性值等于选中值，选中的文本作为名称
				var $select = $property.find("select");
				var text = $select.find("option:selected").text().trim(); // 属性值名称
				var value = $select.val(); // 属性值Id
				value && values.push({ propertyId: propertyId, propertyValueId: value, name: text });
			} else if (propertyType == "radio-button") {
				// 单选按钮，属性等于选中值，选中的文本作为名称
				var $checkedRadio = $property.find("input[type='radio']:checked");
				var text = $checkedRadio.parent().text().trim();
				var value = $checkedRadio.val(); // 属性值Id
				value && values.push({ propertyId: propertyId, propertyValueId: value, name: text });
			} else if (propertyType == "editable-dropdown-list") {
				// 可编辑的下拉框，文本没有编辑过时属性值等于选中值否则等于空，文本框中的文本作为名称
				var $select = $property.find("select");
				var text = $property.find("input[type='text']").val().trim(); // 属性值名称
				var textSelected = $select.find("option:selected").text().trim(); // 选中项名称
				var value = (text == textSelected) ? $select.val() : ""; // 属性值Id
				text && values.push({ propertyId: propertyId, propertyValueId: value, name: text });
			} else {
				console.warn("unknow property type", propertyType);
			}
		});
		$propertyValuesJson.val(JSON.stringify(values));
	});
	// 类目改变时的处理，初始化时也执行这里的处理
	var onCategoryIdChanged = function () {
		// 确认类目是否有改变，有改变时弹出确认框
		var categoryId = $categoryId.val();
		var previousCategoryId = $editor.data("categoryId");
		var confirmMessage = "Sure to change category? The properties you selected will lost!";
		if (previousCategoryId && previousCategoryId != categoryId &&
			!confirm($.translate(confirmMessage))) {
			$categoryId.val(previousCategoryId);
			return false;
		}
		$editor.data("categoryId", categoryId);
		// 远程载入类目对应的属性编辑器
		$editor.load("/api/product/property_editor?categoryId=" + categoryId, function () {
			// 编辑属性值名称的事件
			var aliasSelector = ".property-value-alias";
			$editor.find(aliasSelector + ' .alias-edit-btn').on("click", function () {
				var $alias = $(this).closest(aliasSelector);
				var $aliasEditor = $alias.find(".alias-editor");
				var $aliasText = $alias.find(".alias-text");
				if ($alias.toggleClass("editing").hasClass("editing")) {
					$aliasEditor.val($aliasText.text());
				}
				return false;
			});
			$editor.find(aliasSelector + ' .alias-editor').on("change", function () {
				var $aliasEditor = $(this);
				var $alias = $aliasEditor.closest(aliasSelector);
				setPropertyName($alias.find(".alias-text"), $aliasEditor.val());
			});
			$editor.find(aliasSelector).on("click", function () {
				// 防止编辑时，点击编辑框和按钮之间的空白区域切换多选框
				return $(this).hasClass("editing") ? false : null;
			});
			// 触发绑定属性值的事件
			// 类目改变时清空原有的属性值列表
			if (previousCategoryId && previousCategoryId != categoryId) {
				$propertyValuesJson.val("");
			}
			$editor.trigger(bindEventName);
			// 在属性值的输入框改变后，触发收集属性值的事件
			$editor.find(propertyTypeSelector).find("input, select").on("change", function () {
				$editor.trigger(collectEventName);
			});
		});
	};
	$categoryId.on("change", onCategoryIdChanged);
	onCategoryIdChanged.call($categoryId);
};

// 自动初始化带[data-toggle='product-to-property-values-editor']属性的编辑器
$(function () {
	var setup = function ($elements) {
		$elements.each(function () { $(this).productToPropertyValuesEditor(); });
	};
	var rule = "[data-toggle='product-to-property-values-editor']";
	setup($(rule));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(rule));
	});
});
