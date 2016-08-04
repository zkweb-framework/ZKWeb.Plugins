namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 订单创建器的接口
	/// 这个接口只应该注册一个实现的类，需要替换默认的订单创建器时请先注销原来的注册
	/// </summary>
	public interface IOrderCreator {
		/// <summary>
		/// 创建订单
		/// </summary>
		/// <param name="parameters">创建参数</param>
		/// <returns></returns>
		CreateOrderResult CreateOrder(CreateOrderParameters parameters);
	}
}
