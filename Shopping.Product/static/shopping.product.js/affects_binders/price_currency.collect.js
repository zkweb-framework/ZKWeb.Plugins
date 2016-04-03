(function ($binder, affects) {
	affects.PriceCurrency = parseInt($binder.find("select").val()) || "";
})