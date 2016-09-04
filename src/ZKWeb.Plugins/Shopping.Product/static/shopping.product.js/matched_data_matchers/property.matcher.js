(function (parameters, data) {
	var exceptedProperties = (data.Conditions || {}).Properties;
	if (!exceptedProperties || !exceptedProperties.length) {
		return true;
	}
	var incomeProperties = parameters.Properties;
	if (!incomeProperties || !incomeProperties.length) {
		return false;
	}
	var incomePropertiesMapping = _.indexBy(incomeProperties, 'PropertyId');
	return _.all(exceptedProperties, function (obj) {
		var incomeObj = incomePropertiesMapping[obj.PropertyId];
		return (incomeObj && incomeObj.PropertyValueId == obj.PropertyValueId);
	});
})