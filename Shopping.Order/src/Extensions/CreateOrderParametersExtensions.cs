using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Extensions {
	/// <summary>
	/// 创建订单的参数的扩展函数
	/// </summary>
	public static class CreateOrderParametersExtensions {
		/// <summary>
		/// 获取卖家Id到物流Id的索引
		/// </summary>
		/// <param name="parameters">订单创建参数</param>
		/// <returns></returns>
		public static IDictionary<long, long> GetSellerToLogistics(
			this CreateOrderParameters parameters) {
			return parameters.OrderParameters
				.GetOrDefault<IDictionary<long, long>>("SellerToLogistics") ??
				new Dictionary<long, long>();
		}

		/// <summary>
		/// 获取支付接口Id
		/// </summary>
		/// <param name="parameters">订单创建参数</param>
		/// <returns></returns>
		public static long GetPaymentApiId(this CreateOrderParameters parameters) {
			var paymentApiId = parameters.OrderParameters.GetOrDefault<long>("PaymentApiId");
			if (paymentApiId <= 0) {
				throw new BadRequestException(new T("Please select payment api"));
			}
			return paymentApiId;
		}
	}
}
