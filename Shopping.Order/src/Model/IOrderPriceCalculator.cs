using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 订单价格计算器
	/// </summary>
	public interface IOrderPriceCalculator {
		/// <summary>
		/// 计算订单价格
		/// 计算结果请保存到result中
		/// </summary>
		/// <param name="parameters">订单创建参数</param>
		/// <param name="result">计算结果</param>
		void Calculate(CreateOrderParameters parameters, OrderPriceCalcResult result);
	}
}
