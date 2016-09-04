using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions {
	/// <summary>
	/// 订单参数的扩展函数
	/// </summary>
	public static class OrderParametersExtensions {
		/// <summary>
		/// 获取购物车商品Id => 订购数量
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static IDictionary<Guid, long> GetCartProducts(this OrderParameters parameters) {
			return parameters.GetOrDefault<IDictionary<Guid, long>>("CartProducts") ??
				new Dictionary<Guid, long>();
		}

		/// <summary>
		/// 获取收货地址信息
		/// 注意返回的信息是新建的，没有绑定用户和Id
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static ShippingAddress GetShippingAddress(this OrderParameters parameters) {
			return parameters.GetOrDefault<ShippingAddress>("ShippingAddress") ??
				new ShippingAddress();
		}

		/// <summary>
		/// 获取需要保存收货地址的Id
		/// 如果不保存则返回null，如果更新则返回id，如果添加则返回0
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static Guid? GetSaveShipppingAddressId(this OrderParameters parameters) {
			var shippingAddress = parameters.GetOrDefault<IDictionary<string, object>>("ShippingAddress");
			var selectedAddressId = shippingAddress.GetOrDefault<Guid>("SelectedAddressId");
			var saveAddress = shippingAddress.GetOrDefault<bool>("SaveAddress");
			return saveAddress ? (Guid?)selectedAddressId : null;
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
		/// 没有卖家时卖家Id等于Guid.Empty
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static IDictionary<Guid, Guid> GetSellerToLogistics(this OrderParameters parameters) {
			return parameters.GetOrDefault<IDictionary<Guid, Guid>>("SellerToLogistics") ??
				new Dictionary<Guid, Guid>();
		}

		/// <summary>
		/// 获取支付接口Id
		/// </summary>
		/// <param name="parameters">订单参数</param>
		/// <returns></returns>
		public static Guid GetPaymentApiId(this OrderParameters parameters) {
			return parameters.GetOrDefault<Guid>("PaymentApiId");
		}
	}
}
