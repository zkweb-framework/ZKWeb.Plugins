using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Extensions {
	/// <summary>
	/// 订单参数的扩展函数
	/// </summary>
	public static class OrderParametersExtensions {
		/// <summary>
		/// 获取购物车商品Id => 订购数量
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static IDictionary<long, long> GetCartProducts(this OrderParameters parameters) {
			return parameters.GetOrDefault<IDictionary<long, long>>("CartProducts") ??
				new Dictionary<long, long>();
		}

		/// <summary>
		/// 获取收货地址信息
		/// 注意返回的信息是新建的，没有绑定用户和Id
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static UserShippingAddress GetShippingAddress(this OrderParameters parameters) {
			return parameters.GetOrDefault<UserShippingAddress>("ShippingAddress") ??
				new UserShippingAddress();
		}

		/// <summary>
		/// 获取订单留言
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static string GetOrderComment(this OrderParameters parameters) {
			return parameters.GetOrDefault<string>("OrderComment");
		}

		/// <summary>
		/// 获取卖家Id => 选中的物流Id
		/// 没有卖家时卖家Id等于0
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static IDictionary<long, long> GetSellerToLogistics(this OrderParameters parameters) {
			return parameters.GetOrDefault<IDictionary<long, long>>("SellerToLogistics") ??
				new Dictionary<long, long>();
		}

		/// <summary>
		/// 获取支付接口Id
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static long GetPaymentApiId(this OrderParameters parameters) {
			return parameters.GetOrDefault<long>("PaymentApiId");
		}
	}
}
