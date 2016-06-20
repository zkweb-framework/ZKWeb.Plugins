using System.Collections.Generic;
using ZKWeb.Plugins.Finance.Payment.src.Database;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 创建订单可使用的支付接口的提供器
	/// </summary>
	public interface IOrderPaymentApiProvider {
		/// <summary>
		/// 获取可使用的支付接口
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="apis">可使用的支付接口列表</param>
		void GetPaymentApis(long? userId, IList<PaymentApi> apis);
	}
}
