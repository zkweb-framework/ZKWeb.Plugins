using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 购物车控制器
	/// </summary>
	[ExportMany]
	public class CartController : IController {
		/// <summary>
		/// 购物车页
		/// </summary>
		/// <returns></returns>
		[Action("cart")]
		public IActionResult Index() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 立刻购买的购物车页
		/// </summary>
		/// <returns></returns>
		[Action("cart/buynow")]
		public IActionResult Buynow() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 添加商品到购物车
		/// </summary>
		/// <returns></returns>
		[Action("cart/add", HttpMethods.POST)]
		public IActionResult Add() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 删除购物车中的商品
		/// </summary>
		/// <returns></returns>
		[Action("cart/delete", HttpMethods.POST)]
		public IActionResult Delete() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取迷你购物车的内容
		/// </summary>
		/// <returns></returns>
		[Action("cart/minicart_contents")]
		public IActionResult MiniCartContents() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 计算购物车中的商品价格
		/// </summary>
		/// <returns></returns>
		[Action("cart/calculate_price", HttpMethods.POST)]
		public IActionResult CalculatePrice() {
			throw new NotImplementedException();
		}
	}
}
