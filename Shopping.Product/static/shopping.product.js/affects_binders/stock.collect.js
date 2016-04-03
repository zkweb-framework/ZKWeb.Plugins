(function ($binder, affects) {
	var stock = parseInt($binder.find("input[type=text]").val());
	affects.Stock = isNaN(stock) ? null : stock;
})