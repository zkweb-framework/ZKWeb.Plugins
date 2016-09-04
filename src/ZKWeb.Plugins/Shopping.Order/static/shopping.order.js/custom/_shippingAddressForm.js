/*
	用户收货地址表单
	格式
	<div class="user-shipping-address-form">
		<select name="ShippingAddress"></select>
		<input type="checkbox" name="SaveAddress">
		<input type="hidden" name="ShippingAddressJson" />
		<input type="text" name="Fullname" />
		<input type="text" name="TelOrMobile" />
		<input type="hidden" name="Region" />
		<input type="text" name="ZipCode" />
		<input type="text" name="DetailedAddress" />
	</div>
	功能
	收货地址改变时，把选择的地址复制到各个控件中
	控件值改变时，收集值到ShippingAddressJson控件
*/

$.fn.shippingAddressForm = function () {
	var $form = $(this);
	// 防止重复初始化
	if ($form.data("initialized")) {
		return;
	}
	$form.data("initialized", true);
	// 获取各个元素
	var $shippingAddress = $form.find("[name='ShippingAddress']");
	var $saveAddress = $form.find("[name='SaveAddress']");
	var $shippingAddressJson = $form.find("[name='ShippingAddressJson']");
	var $receiverName = $form.find("[name='Fullname']");
	var $receiverTel = $form.find("[name='TelOrMobile']");
	var $region = $form.find("[name='Region']");
	var $zipCode = $form.find("[name='ZipCode']");
	var $detailedAddress = $form.find("[name='DetailedAddress']");
	// 收货地址改变时，把选择的地址信息复制到各个输入框中
	// 复制时不收集，全部复制完后再触发收集事件
	var copyingLockName = "copyingLock";
	var collectEventName = "collect.shippingAddressForm";
	var onSelectedAddressChanged = function () {
		// 锁定复制锁
		$form.data(copyingLockName, true);
		// 复制地址信息
		var selectedAddress = JSON.parse($shippingAddress.val() || "{}") || {};
		$receiverName.val(selectedAddress.ReceiverName);
		$receiverTel.val(selectedAddress.ReceiverTel);
		$region.val(JSON.stringify({ Country: selectedAddress.Country, RegionId: selectedAddress.RegionId }));
		$region.closest(".region-editor").trigger("bindCountryAndRegion.RegionEditor");
		$zipCode.val(selectedAddress.ZipCode);
		$detailedAddress.val(selectedAddress.DetailedAddress);
		// 解除复制锁
		$form.data(copyingLockName, false);
		// 触发收集事件
		$form.trigger(collectEventName);
	};
	$shippingAddress.on("change", onSelectedAddressChanged);
	// 绑定收集事件
	$form.bind(collectEventName, function () {
		// 复制时不收集
		if ($form.data(copyingLockName)) {
			return;
		}
		// 收集地址信息
		var selectedAddress = JSON.parse($shippingAddress.val() || "{}") || {};
		var region = JSON.parse($region.val() || "{}") || {};
		var address = {
			SelectedAddressId: selectedAddress.Id || null,
			SaveAddress: $saveAddress.is(":checked"),
			ReceiverName: $receiverName.val(),
			ReceiverTel: $receiverTel.val(),
			Country: region.Country || null,
			RegionId: region.RegionId || null,
			ZipCode: $zipCode.val(),
			DetailedAddress: $detailedAddress.val(),
		};
		// 保存到ShippingAddressJson控件
		$shippingAddressJson.val(JSON.stringify(address)).change();
	});
	// 控件值改变时触发收集事件，需要排除ShippingAddressJson控件
	$form.find("input").not($shippingAddressJson).on("change", function () {
		$form.trigger(collectEventName);
	});
	// 初始化时调用收货地址改变时的事件
	onSelectedAddressChanged.call($shippingAddress);
};
