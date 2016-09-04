using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderPaymentApiProviders.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Components.PaymentTransactionHandlers;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderPaymentApiProviders {
	/// <summary>
	/// 默认的创建订单可使用的支付接口提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderPaymentApiProvider : IOrderPaymentApiProvider {
		/// <summary>
		/// 获取可使用的支付接口
		/// 只返回后台添加的接口
		/// </summary>
		public void GetPaymentApis(Guid? userId, IList<PaymentApi> apis) {
			var paymentApiManager = Application.Ioc.Resolve<PaymentApiManager>();
			apis.AddRange(paymentApiManager.GetManyWithCache(
				null, OrderTransactionHandler.ConstType));
		}
	}
}
