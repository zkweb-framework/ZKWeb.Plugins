(function ($binder, conditions) {
	var countGE = parseInt($binder.find("input[type=text]").val()) || 1;
	(countGE > 1) ? (conditions.OrderCountGE = countGE) : (delete conditions.OrderCountGE);
})