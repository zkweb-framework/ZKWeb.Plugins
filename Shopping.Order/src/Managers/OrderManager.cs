using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Order.src.Managers {
	/// <summary>
	/// 订单管理器
	/// </summary>
	[ExportMany]
	public class OrderManager {
		/// <summary>
		/// 计算订单商品的单价
		/// 返回价格大于或等于0
		/// </summary>
		/// <param name="userId">用户Id，未登录时等于null</param>
		/// <param name="parameters">创建订单商品的参数</param>
		/// <returns></returns>
		public virtual OrderPriceCalcResult CalculateOrderProductUnitPrice(
			long? userId, CreateOrderProductParameters parameters) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 计算订单的价格
		/// 返回价格大于0
		/// </summary>
		/// <param name="parameters">创建订单的参数</param>
		/// <returns></returns>
		public virtual OrderPriceCalcResult CalculateOrderPrice(
			CreateOrderParameters parameters) {
			throw new NotImplementedException();
		}
	}
}
