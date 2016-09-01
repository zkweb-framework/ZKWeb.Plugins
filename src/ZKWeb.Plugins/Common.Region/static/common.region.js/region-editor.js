/*
	用于初始化地区联动下拉框
*/

(function () {
	// 以下是全局变量，整个页面中只获取或生成一次
	// 国家信息
	var countryInfo = null;
	// { 国家: 地区树, ... }
	var regionTrees = {};
	// 根据regionTrees生成
	// { 国家: { 地区Id: 节点, ... }, ... }
	var regionTreeNodes = {};

	// 更新国家信息，更新完成后执行onUpdated
	var updateCountryInfo = function (onUpdated) {
		// 已经更新时直接执行onUpdated
		if (countryInfo !== null) {
			onUpdated();
			return;
		}
		// 请求远程更新，更新完成后执行onUpdated
		$.post("/api/region/country_info", function (data) {
			countryInfo = data;
			onUpdated();
		});
	};

	// 更新地区信息，更新完成后执行onUpdated
	var updateRegionTrees = function (country, onUpdated) {
		// 已经更新时直接执行onUpdated
		var regionTree = regionTrees[country] || null;
		if (regionTree !== null) {
			onUpdated();
			return;
		}
		// 请求远程更新，更新完成后执行onUpdated
		$.post("/api/region/region_tree_from_country", { "country": country }, function (data) {
			var tree = data.tree;
			regionTrees[country] = tree;
			var treeNodes = {};
			var appendChildsToTreeNodes = function (node) {
				_.each(node.Childs, function (childNode) {
					treeNodes[childNode.Value.Id] = childNode;
					appendChildsToTreeNodes(childNode);
				});
			};
			appendChildsToTreeNodes(tree);
			regionTreeNodes[country] = treeNodes;
			onUpdated();
		});
	};

	// 地区联动下拉框的jQuery函数
	$.fn.regionEditor = function () {
		var $editor = $(this);
		// 避免重复初始化
		var initialized = $editor.data("initialized");
		if (initialized) {
			return;
		}
		$editor.data("initialized", true);
		// 获取参数和生成子div
		var $field = $(this).find("input[type=hidden]");
		var displayCountryDropdown = $editor.data("display-country-dropdown");
		var $countryDropdownContainer = $("<div>").attr("class", "country-dropdown").appendTo($editor);
		var $regionsDropdownContainer = $("<div>").attr("class", "regions-dropdown").appendTo($editor);
		// 获取和设置字段值的函数
		var getFieldValue = function () { return JSON.parse($field.val() || "{}") || {}; };
		var setFieldValue = function (value) { $field.val(JSON.stringify(value)).change(); };
		// 获取子地区下拉框的函数，没有子地区时返回空对象
		var getChildNodesDropdown = function (node) {
			if ($.isEmptyObject(node.Childs)) {
				return $();
			}
			var $dropdown = $("<select>").addClass("form-control");
			$dropdown.append($("<option>").text("-").val(""));
			_.each(node.Childs, function (childNode) {
				$dropdown.append($("<option>").text(childNode.Value.Name).val(childNode.Value.Id));
			});
			return $dropdown;
		};
		// 绑定国家的事件
		var bindCountryEventName = "bindCountry.regionEditor";
		$editor.on(bindCountryEventName, function () {
			var value = getFieldValue();
			value.Country = value.Country || countryInfo.defaultCountry;
			setFieldValue(value);
			$countryDropdownContainer.find("select").val(value.Country);
		});
		// 绑定地区的事件
		var bindRegionEventName = "bindRegion.regionEditor";
		$editor.on(bindRegionEventName, function () {
			var value = getFieldValue();
			var rootNode = regionTrees[value.Country] || {};
			var treeNodes = regionTreeNodes[value.Country] || {}; // { 地区Id: 节点, ... }
			var nodes = []; // 从上到下的地区列表 例如 [ 天津, 天津市, 和平区 ]
			var node = treeNodes[value.RegionId];
			while (node) {
				nodes.push(node);
				node = treeNodes[node.Value.ParentId];
			}
			nodes.reverse();
			$regionsDropdownContainer.empty();
			// 枚举地区列表添加下拉框并选中每个地区对应的Id
			_.each(nodes, function (node) {
				var parentNode = treeNodes[node.Value.ParentId] || rootNode;
				var $dropdown = getChildNodesDropdown(parentNode);
				$dropdown.val(node.Value.Id);
				$regionsDropdownContainer.append($dropdown);
			});
			// 添加最后一个地区的下级下拉框
			$regionsDropdownContainer.append(getChildNodesDropdown(_.last(nodes) || rootNode));
		});
		// 国家改变时的事件
		$countryDropdownContainer.on("change", "select", function () {
			// 保存到字段
			var country = $(this).val();
			setFieldValue({ Country: country, RegionId: null });
			// 更新地区信息
			updateRegionTrees(country, function () {
				// 绑定地区下拉框
				$editor.trigger(bindRegionEventName);
			});
		});
		// 地区改变时的事件
		$regionsDropdownContainer.on("change", "select", function () {
			// 保存到字段
			// 这里获取地区Id还需要根据之前的下拉框，例如选择了省市镇后，再取消选择市时这里需要使用省的值
			var $select = $(this);
			var country = $countryDropdownContainer.find("select").val();
			var regionId = null;
			$select.nextAll().remove();
			$select.parent().find("select").each(function () {
				var id = $(this).val();
				if (id) {
					regionId = id;
				}
			});
			setFieldValue({ Country: country, RegionId: regionId });
			// 重新生成下级下拉框
			var node = (regionTreeNodes[country] || {})[$select.val()];
			node && $regionsDropdownContainer.append(getChildNodesDropdown(node));
		});
		// 更新国家和地区信息，并绑定地区
		var bindCountryAndRegionEventName = "bindCountryAndRegion.RegionEditor";
		$editor.on(bindCountryAndRegionEventName, function () {
			updateCountryInfo(function () {
				// 添加国家下拉框
				$countryDropdownContainer.empty();
				var $dropdown = $("<select>").addClass("form-control");
				_.each(countryInfo.countries, function (country) {
					$dropdown.append($("<option>").text(country.Name).val(country.Value));
				});
				displayCountryDropdown ? $dropdown.show() : $dropdown.hide();
				$countryDropdownContainer.append($dropdown);
				// 绑定国家下拉框
				$editor.trigger(bindCountryEventName);
				// 更新地区信息
				updateRegionTrees($dropdown.val(), function () {
					// 绑定地区下拉框
					$editor.trigger(bindRegionEventName);
				});
			});
		}).trigger(bindCountryAndRegionEventName);
	};
})();

$(function () {
	// 自动初始化[data-trigger=region-editor]
	var rule = "[data-trigger=region-editor]";
	$(rule).each(function () { $(this).regionEditor(); });
	$(document).on("dynamicLoaded", function (e, contents) {
		$(contents).find(rule).each(function () { $(this).regionEditor(); });
	});
});
