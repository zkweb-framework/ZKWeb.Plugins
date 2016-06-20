using System.Collections.Generic;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderPaymentApiProviders {
	/// <summary>
	/// 默认的创建订单可使用的支付接口提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderPaymentApiProvider : IOrderPaymentApiProvider {
		/// <summary>
		/// 获取可使用的支付接口
		/// 只返回后台添加的接口
		/// </summary>
		public void GetPaymentApis(long? userId, IList<PaymentApi> apis) {
			var paymentApiManager = Application.Ioc.Resolve<PaymentApiManager>();
			// TODO: 下个版本改成AddRange
			foreach (var api in paymentApiManager.GetPaymentApis(null)) {
				apis.Add(api);
			}
		}
	}
}
