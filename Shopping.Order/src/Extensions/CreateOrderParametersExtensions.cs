using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Managers;
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

		/// <summary>
		/// 设置登录信息（用户Id和会话Id）
		/// </summary>
		/// <param name="parameters">订单创建参数</param>
		public static void SetLoginInfo(this CreateOrderParameters parameters) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var user = session.GetUser();
			parameters.UserId = (user == null) ? null : (long?)user.Id;
			parameters.SessionId = session.Id;
		}

		/// <summary>
		/// 复制订单创建参数并使用新的订单商品创建参数
		/// 一般用于把一个订单创建参数分割为多个订单创建参数时使用
		/// </summary>
		/// <param name="parameters">订单创建参数</param>
		/// <param name="productParametersList">订单商品创建参数的列表</param>
		/// <returns></returns>
		public static CreateOrderParameters CloneWith(
			this CreateOrderParameters parameters,
			IList<CreateOrderProductParameters> productParametersList) {
			return new CreateOrderParameters() {
				UserId = parameters.UserId,
				SessionId = parameters.SessionId,
				OrderParameters = parameters.OrderParameters,
				OrderProductParametersList = productParametersList
			};
		}
	}
}
