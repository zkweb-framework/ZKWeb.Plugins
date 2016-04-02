/*
	前台商品销售信息使用的功能
		按选择的规格和数量匹配查找对应的价格和库存
	使用时要求页面有ProductSalesInfo元素
	脚本只在页面打开时执行，对于打开后动态添加的内容不起效果
*/

// 选择的规格或数量等改变后，匹配查找对应的价格和库存并显示
// 事件
// applyMatchParameters.productSalesInfo
//		应用选择的规格和数量等条件
//		匹配查找商品匹配数据中的价格和库存并显示
$(function () {
	// 绑定应用选择的规格和数量等条件的事件
	var applyMatchParametersEventName = "applyMatchParameters.productSalesInfo";
	var $productSalesInfo = $(".product-sales-info"); // 销售信息的元素，注意一个页面可能有多个
	var parameterSelector = "[data-match-parameter]"; // 提供匹配条件的元素选择器
	$productSalesInfo.on(applyMatchParametersEventName, function () {
		var $this = $(this);
		// 获取匹配数据列表和货币信息索引，只获取一次
		var matchedDataDataKey = "matchedData";
		var matchedData = $this.data(matchedDataDataKey);
		if (!matchedData) {
			matchedData = JSON.parse($this.find("[name='matchedDataJson']").val() || "[]");
			$this.data(matchedDataDataKey, matchedData);
			// console.log(matchedData);
		}
		var currencyInfoMappingDataKey = "currencyInfoMapping";
		var currencyInfoMapping = $this.data(currencyInfoMappingDataKey);
		if (!currencyInfoMapping) {
			currencyInfoMapping = JSON.parse($this.find("[name='currencyInfoMappingJson']").val() || "{}");
			$this.data(currencyInfoMappingDataKey, currencyInfoMapping);
			// console.log(currencyInfoMapping);
		}
		// 获取匹配器列表，只获取一次
		var matchersDataKey = "matchers";
		var matchers = $this.data(matchersDataKey);
		if (!matchers) {
			matchers = [];
			$this.find("script[type='text/productMatchedDataMatcher']").each(function () {
				var body = $(this).attr("data-body").trim();
				body && matchers.push(eval("(" + body + ")"));
			});
			$this.data(matchersDataKey, matchers);
			// console.log(matchers);
		}
		// 收集所有参数
		// 设置参数到data中，其他注册相同事件的函数可以使用
		var parameters = {};
		$this.find(parameterSelector).each(function () {
			var $parameter = $(this);
			var value = $parameter.val();
			if (value) {
				parameters[$parameter.attr("data-match-parameter")] = JSON.parse(value);
			}
		});
		$this.data("parameters", parameters);
		// 匹配查找对应的价格和库存并显示
		// 没有匹配器时，所有数据都不应该匹配
		var price = null;
		var currency = 0;
		var stock = null;
		matchers && _.each(matchedData, function (data) {
			var matched = _.all(matchers, function (matcher) { return matcher(parameters, data); });
			if (price === null && matched && data.Price !== null) {
				price = data.Price;
				currency = data.PriceCurrency;
			}
			if (stock === null && matched && data.Stock !== null) {
				stock = data.Stock;
			}
		});
		var currencyInfo = currencyInfoMapping[currency] || {};
		$this.find(".product-price .prefix").text(currencyInfo.Prefix || "");
		$this.find(".product-price .price").text(price || 0);
		$this.find(".product-price .suffix").text(currencyInfo.Suffix || "");
		$this.find(".product-estimation .stock").text(stock || 0);
		// console.log(parameters, applyMatchParametersEventName);
	});
	// 提供匹配条件的元素值改变后，触发应用事件
	$productSalesInfo.on("change", parameterSelector, function () {
		$(this).closest($productSalesInfo).trigger(applyMatchParametersEventName);
	});
});

// 选中的规格改变后，把所有选中的规格值储存到提供匹配条件的元素中
// 事件
// collectProperties.productEstimation
//		收集选择的规格并设置到元素[data-match-parameter='Properties']的值
//		格式是 [ { PropertyId: 规格Id, PropertyValueId: 规格值Id }, ... ]
$(function () {
	// 绑定收集选择的规格的事件
	var collectPropertiesEventName = "collectProperties.productEstimation";
	var $productEstimation = $(".product-estimation"); // 规格和数量选择的元素，注意一个页面可能有多个
	var $propertyParameter = $productEstimation.find("[data-match-parameter='Properties']"); // 提供匹配条件的元素
	var propertySelector = "[data-property-id]"; // 单个规格的选择器，元素下包含多个规格值按钮
	var propertyValueSelector = "[data-property-value-id]"; // 单个规格值按钮的选择器
	$productEstimation.on(collectPropertiesEventName, function () {
		var $this = $(this);
		var properties = [];
		$this.find(propertySelector).each(function () {
			var $property = $(this);
			var $selected = $property.find(propertyValueSelector + ".selected").first();
			if ($selected.length) {
				properties.push({
					PropertyId: $property.data("property-id"),
					PropertyValueId: $selected.data("property-value-id"),
					PropertyValueName: $selected.attr("title")
				});
			}
		});
		$propertyParameter.val(JSON.stringify(properties)).change();
	});
	// 点击选择规格后触发收集选择的规格的事件
	$productEstimation.on("click", propertyValueSelector, function () {
		var $this = $(this);
		$this.closest(propertySelector).find(propertyValueSelector).removeClass("selected");
		$this.addClass("selected");
		$this.closest($productEstimation).trigger(collectPropertiesEventName);
	});
	// 打开页面时默认选择每项规格下的第一个规格值，并触发收集选择的规格的事件
	$productEstimation.find(propertySelector).each(function () {
		var $values = $(this).find(propertyValueSelector);
		$values.removeClass("selected");
		$values.first().addClass("selected");
	});
	$productEstimation.trigger(collectPropertiesEventName);
});
