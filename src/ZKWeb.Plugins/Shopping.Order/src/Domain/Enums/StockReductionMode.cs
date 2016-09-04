using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Enums {
	/// <summary>
	/// 库存减少模式
	/// </summary>
	public enum StockReductionMode {
		/// <summary>
		/// 不减少
		/// </summary>
		NoReduction = 0,
		/// <summary>
		/// 创建订单后减少
		/// </summary>
		AfterCreate = 1,
		/// <summary>
		/// 支付成功后减少
		/// </summary>
		AfterPay = 2
	}
}
