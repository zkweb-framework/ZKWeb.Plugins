(function ($binder, affects) {
	$binder.find("select").val(affects.PriceCurrency || "");
})