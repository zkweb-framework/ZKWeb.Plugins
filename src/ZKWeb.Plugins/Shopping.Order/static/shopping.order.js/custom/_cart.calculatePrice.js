/*
	购物车功能
	构建订单创建参数和实时计算价格

	构建订单创建参数
		格式 (同CreateOrderParameters) {
			CreateOrderProductParametersList: [
				{ ProductId: 商品Id, MatchParameters: 商品匹配参数 }
				...
			],
			OrderParameters: {
				ShippingAddress: { Country: 国家Id, RegionId: 地区Id, ... },
				SellerToLogistics: { SellerId: LogisticsId, ... },
				PaymentApiId: 支付接口Id,
				CartProducts: { 购物车商品Id: 数量, ... }
				...
			}
		}
		构建OrderParameters的流程
			查找所有带data-order-parameter属性的元素，把属性值作为OrderParameters中的键
			获取并解析元素的值，参考jquery的dataAttr的处理
			最后把值设置到OrderParameters下

	计算价格
		请求 /api/cart/calculate_price
		参数格式 同以上的"订单创建参数"
		返回格式 {
			priceInfo: {
				orderPriceString: "订单价格的字符串",
				orderPriceDescription: "订单价格的描述",
				orderProductTotalPriceString: "商品总金额的字符串",
				orderProductUnitPrices: [{
					priceString: 商品单价的字符串
					description: 商品单价的描述
				}, ... ],
				availableLogistics: {
					卖家Id: [ { logisticsId: 物流Id, costString: 运费字符串 }, ... ]
				},
				availablePaymentApis: [ { apiId: 支付接口Id, feeString: 手续费字符串 }, ... ]
			}
		}
		失败时的格式 { error: "错误信息" }
		成功时更新价格信息，失败时显示错误信息
		所有更改在1秒后再请求
		记录最后请求的Id，返回结果必须匹配该Id才显示（防止先发起的请求后收到）

	提交订单
		请求 /api/order/create
		参数格式 同以上的"订单创建参数"
		成功时跳转到指定地址，失败时显示错误信息
*/

/* 构建订单创建参数 */
$(function () {
	var $cartContainer = $(".cart-container");
	var $createOrderParameters = $cartContainer.find(".btn-submit-order [name='CreateOrderParameters']");
	var createOrderParametersChangeEventName = "createOrderParametersChange.cartView";
	var collectCreateOrderParametersEventName = "collectCreateOrderParametersEventName.cartView";
	$cartContainer.on(collectCreateOrderParametersEventName, function () {
		// 格式同CreateOrderParameters
		var createOrderParameters = {
			OrderParameters: {},
			OrderProductParametersList: []
		};
		// 收集带data-order-parameter属性的元素的值
		$cartContainer.find("[data-order-parameter]").each(function () {
			var $parameter = $(this);
			var key = $parameter.attr("data-order-parameter");
			var value = $("<div>").attr("data-val", $parameter.val()).data("val");
			if (key === "CreateOrderProductParametersList") {
				createOrderParameters.OrderProductParametersList = value;
			} else {
				createOrderParameters.OrderParameters[key] = value;
			}
		});
		// 保存订单创建参数并触发改变事件
		$createOrderParameters.val(JSON.stringify(createOrderParameters));
		$cartContainer.data("createOrderParameters", createOrderParameters);
		$cartContainer.trigger(createOrderParametersChangeEventName);
	});
	// 带data-order-parameter属性的元素改变时，触发收集事件
	$cartContainer.on("change", "[data-order-parameter]", function () {
		$cartContainer.trigger(collectCreateOrderParametersEventName);
	});
	// 页面载入时触发收集事件
	$cartContainer.trigger(collectCreateOrderParametersEventName);
});

/* 计算价格 */
$(function () {
	// 绑定计算价格的事件
	var $cartContainer = $(".cart-container");
	var $orderPriceDescription = $cartContainer.find(".order-price-description .description");
	var calculatingHtml = $orderPriceDescription.html();
	var calcPriceEventName = "calcPrice.cartView";
	var calcPriceSuccessEventName = "calcPriceSuccess.cartView";
	var calcPriceFailedEventName = "calcPriceFailed.cartView";
	var timeStampKey = "lastCalcPriceTimeStamp";
	var timeoutHandler = null;
	$cartContainer.data("calculatingHtml", calculatingHtml);
	$cartContainer.on(calcPriceEventName, function () {
		// 显示计算中，1秒后再处理
		$orderPriceDescription.html(calculatingHtml);
		clearTimeout(timeoutHandler);
		timeoutHandler = setTimeout(function () {
			// 获取订单创建参数
			var createOrderParameters = $cartContainer.data("createOrderParameters");
			if ($.isEmptyObject(createOrderParameters)) {
				return;
			}
			// 设置时间戳，保证只显示最后一次提交的计算价格
			var timestamp = Date.now();
			$cartContainer.data(timeStampKey, timestamp);
			// 提交到服务端
			var params = { CreateOrderParameters: JSON.stringify(createOrderParameters) };
			$.post("/api/cart/calculate_price", params, function (data) {
				// 判断当前返回的是否最后一次提交的结果
				if (timestamp !== $cartContainer.data(timeStampKey)) {
					return;
				}
				// 成功时显示价格信息，失败时显示错误信息
				var priceInfo = data.priceInfo;
				if (priceInfo) {
					// 订单总价
					$orderPriceDescription.text(
						priceInfo.orderPriceDescription + " = " + priceInfo.orderPriceString);
					// 触发计算价格成功时的事件
					$cartContainer.data("priceInfo", priceInfo);
					$cartContainer.trigger(calcPriceSuccessEventName, [priceInfo]);
				} else {
					$orderPriceDescription.text(data.error);
					$cartContainer.trigger(calcPriceFailedEventName, [data.error]);
				}
			});
		}, 1000);
	});
	// 在订单创建参数改变时触发计算事件
	$cartContainer.on("createOrderParametersChange.cartView", function () {
		$cartContainer.trigger(calcPriceEventName);
	});
	// 页面载入时触发计算事件
	$cartContainer.trigger(calcPriceEventName);
});
