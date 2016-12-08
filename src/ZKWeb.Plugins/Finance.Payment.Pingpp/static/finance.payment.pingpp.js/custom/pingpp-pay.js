/* Ping++支付功能 */

$(function () {
	var $pay = $(".pingpp-pay");
	var $allChannel = $pay.find(".payment-channels .payment-channel");
	var $channelField = $pay.find("[name='PaymentChannel']");
	var activedClass = "border-themed";

	var setActivedChannel = function ($channel) {
		$allChannel.find(".title").removeClass(activedClass);
		$channel.find(".title").addClass(activedClass);
		$channelField.val($channel.data("value"));
	};

	$allChannel.on("click", function () { setActivedChannel($(this)); });
	setActivedChannel($allChannel.first());
});
