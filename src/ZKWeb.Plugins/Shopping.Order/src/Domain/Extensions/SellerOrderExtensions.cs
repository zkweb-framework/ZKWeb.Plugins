using System;
using ZKWeb.Plugins.Shopping.Order.src.Components.OrderCheckers.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions {
	/// <summary>
	/// 卖家订单的扩展函数
	/// </summary>
	public static class SellerOrderExtensions {
		/// <summary>
		/// 检查函数的类型
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="result">判断结果</param>
		public delegate void CheckFunc(SellerOrder order, ref Pair<bool, string> result);

		/// <summary>
		/// 检查订单是否满足指定条件
		/// 返回是否满足和文本信息
		/// </summary>
		/// <param name="order">订单</param>
		/// <param name="getCheckFunc">获取检查函数的函数</param>
		/// <returns></returns>
		public static Pair<bool, string> Check(
			this SellerOrder order, Func<IOrderChecker, CheckFunc> getCheckFunc) {
			var result = Pair.Create(false, "No Result");
			var checkers = Application.Ioc.ResolveMany<IOrderChecker>();
			checkers.ForEach(c => getCheckFunc(c)(order, ref result));
			return result;
		}
	}
}
