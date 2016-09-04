(function ($binder, affects) {
	var price = parseFloat($binder.find("input[type=text]").val());
	affects.Price = isNaN(price) ? null : price;
})