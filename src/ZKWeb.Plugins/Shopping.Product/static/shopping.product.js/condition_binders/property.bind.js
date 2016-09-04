(function ($binder, conditions) {
	var valueMapping = _.indexBy(conditions.Properties, "PropertyId");
	$binder.find("select").each(function () {
		var $select = $(this);
		var value = valueMapping[$select.data("property-id")];
		$select.val(value ? value.PropertyValueId : "").change();
	});
})