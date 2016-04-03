(function ($binder, conditions) {
	var valueList = [];
	$binder.find("select").each(function () {
		var $select = $(this);
		var value = parseInt($select.val());
		value && valueList.push({
			PropertyId: parseInt($select.data("property-id")),
			PropertyValueId: value
		});
	});
	valueList.length ? (conditions.Properties = valueList) : (delete conditions.Properties);
})