(function (parameters, data) {
	var orderCountGE = parseInt((data.Conditions || {}).OrderCountGE);
	if (!orderCountGE || orderCountGE <= 1) {
		return true;
	}
	var orderCount = parseInt(parameters.OrderCount);
	return (orderCount && orderCount >= orderCountGE);
})