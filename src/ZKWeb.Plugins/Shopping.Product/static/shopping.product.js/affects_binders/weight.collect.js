(function ($binder, affects) {
	var weight = parseFloat($binder.find("input[type=text]").val());
	affects.Weight = isNaN(weight) ? null : weight;
})