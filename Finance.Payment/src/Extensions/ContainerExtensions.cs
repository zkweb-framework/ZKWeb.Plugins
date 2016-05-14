using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Finance.Payment.src.Model;

namespace ZKWeb.Plugins.Finance.Payment.src.Extensions {
	/// <summary>
	/// Ioc容器的扩展函数
	/// </summary>
	public static class ContainerExtensions {
		/// <summary>
		/// 根据类型获取对应的支付接口处理器
		/// 没有时抛出未知类型的例外
		/// </summary>
		/// <param name="container">Ioc容器</param>
		/// <param name="type">支付接口的类型</param>
		/// <returns></returns>
		public static List<IPaymentApiHandler> ResolvePaymentApiHandlers(this IContainer container, string type) {
			var handlers = container.ResolveMany<IPaymentApiHandler>().Where(h => h.Type == type).ToList();
			if (!handlers.Any()) {
				throw new HttpException(400, string.Format(new T("Unknown payment api type {0}"), type));
			}
			return handlers;
		}

		/// <summary>
		/// 根据类型获取对应的支付交易处理器
		/// 没有时抛出未知类型的例外
		/// </summary>
		/// <param name="container">Ioc容器</param>
		/// <param name="type">支付交易的类型</param>
		/// <returns></returns>
		public static List<IPaymentTransactionHandler> ResolveTransactionHandlers(this IContainer container, string type) {
			var handlers = container.ResolveMany<IPaymentTransactionHandler>().Where(h => h.Type == type).ToList();
			if (!handlers.Any()) {
				throw new HttpException(400, string.Format(new T("Unknown payment transaction type {0}"), type));
			}
			return handlers;
		}
	}
}
