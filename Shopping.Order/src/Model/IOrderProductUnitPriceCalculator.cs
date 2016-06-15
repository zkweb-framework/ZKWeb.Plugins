using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 订单商品单价的计算器
	/// </summary>
	public interface IOrderProductUnitPriceCalculator {
		/// <summary>
		/// 计算订单商品的单价
		/// 计算结果请保存到result中
		/// </summary>
		/// <param name="userId">用户Id，未登录时等于null</param>
		/// <param name="parameters">订单商品的创建参数</param>
		/// <param name="result">计算结果</param>
		void Calculate(long? userId, CreateOrderProductParameters parameters, OrderPriceCalcResult result);
	}
}
