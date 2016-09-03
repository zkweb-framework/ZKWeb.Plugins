(function ($binder) {
	var countGE = parseInt($binder.find("input[type=text]").val()) || 1;
	return (countGE > 1) ? (T_OrderCountGE + " " + countGE) : "";
})