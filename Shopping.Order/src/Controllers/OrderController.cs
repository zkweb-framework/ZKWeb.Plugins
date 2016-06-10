using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Utils.IocContainer;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 订单控制器
	/// </summary>
	[ExportMany]
	public class OrderController : IController {
		/// <summary>
		/// 跳转到订单支付页面
		/// </summary>
		/// <returns></returns>
		[Action("order/pay")]
		public IActionResult Pay() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 确认收货
		/// </summary>
		/// <returns></returns>
		[Action("order/confirm")]
		[Action("order/confirm", HttpMethods.POST)]
		public IActionResult Confirm() {
			throw new NotImplementedException();
		}
	}
}
