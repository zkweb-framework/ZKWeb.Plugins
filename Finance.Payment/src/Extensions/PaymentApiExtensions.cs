using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Model;

namespace ZKWeb.Plugins.Finance.Payment.src.Extensions {
	/// <summary>
	/// 支付接口的扩展函数
	/// </summary>
	public static class PaymentApiExtensions {
		/// <summary>
		/// 获取支付接口的处理器列表
		/// 没有时抛出未知类型的例外
		/// </summary>
		/// <param name="api">支付接口</param>
		/// <returns></returns>
		public static IList<IPaymentApiHandler> GetHandlers(this PaymentApi api) {
			var handlers = Application.Ioc.ResolveMany<IPaymentApiHandler>().Where(h => h.Type == api.Type).ToList();
			if (!handlers.Any()) {
				throw new BadRequestException(string.Format(new T("Unknown payment api type {0}"), api.Type));
			}
			return handlers;
		}
	}
}
