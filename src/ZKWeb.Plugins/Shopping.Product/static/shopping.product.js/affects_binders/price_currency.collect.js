(function ($binder, affects) {
	affects.PriceCurrency = $binder.find("select").val() || "";
})