using System;
using System.Collections.Generic;

namespace ZKWeb.Plugins.Shopping.Order.src.Model {
	/// <summary>
	/// 订单各个状态的切换时间
	/// </summary>
	public class OrderStateTimes : Dictionary<OrderState, DateTime> { }
}
