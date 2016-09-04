using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Structs {
	/// <summary>
	/// 订单各个状态的切换时间
	/// </summary>
	public class OrderStateTimes : Dictionary<OrderState, DateTime> { }
}
