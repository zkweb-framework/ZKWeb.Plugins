(function ($binder) {
	var str = "";
	$binder.find("select").each(function () {
		var $select = $(this);
		var value = parseInt($select.val());
		var valueName = $select.find("option:selected").text().trim();
		value && (str += $select.attr("data-property-name") + ": " + valueName + " ");
	});
	return str.trim();
})