using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions {
	/// <summary>
	/// 创建订单的参数的扩展函数
	/// </summary>
	public static class CreateOrderParametersExtensions {
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
