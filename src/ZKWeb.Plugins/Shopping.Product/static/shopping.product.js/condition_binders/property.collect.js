(function ($binder, conditions) {
	var valueList = [];
	$binder.find("select").each(function () {
		var $select = $(this);
		var value = $select.val();
		value && valueList.push({
			PropertyId: $select.data("property-id"),
			PropertyValueId: value
		});
	});
	valueList.length ? (conditions.Properties = valueList) : (delete conditions.Properties);
})