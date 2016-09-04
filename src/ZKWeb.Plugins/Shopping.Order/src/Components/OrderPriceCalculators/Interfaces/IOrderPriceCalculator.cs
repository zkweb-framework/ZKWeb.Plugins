using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.OrderPriceCalculators.Interfaces {
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
