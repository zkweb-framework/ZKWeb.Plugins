using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Config;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Utils.IocContainer;
using ZKWeb.Web.ActionResults;
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
			return new PlainResult("TODO: not implemented");
		}

		/// <summary>
		/// 添加商品到购物车
		/// </summary>
		/// <returns></returns>
		[Action("cart/add", HttpMethods.POST)]
		public IActionResult Add() {
			var request = HttpContextUtils.CurrentContext.Request;
			var productId = request.Get<long>("productId");
			var matchParameters = request.Get<IDictionary<string, object>>("matchParameters");
			var isBuyNow = request.Get<bool>("isBuyNow");
			// 添加购物车商品，抛出无权限错误时跳转到登陆页面
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			try {
				cartProductManager.AddCartProduct(productId,
					isBuyNow ? Model.CartProductType.Buynow : Model.CartProductType.Default, matchParameters);
			} catch (HttpException ex) {
				if (ex.GetHttpCode() == 403) {
					return new JsonResult(new { redirectTo = "/user/login" });
				}
				throw;
			}
			// 立刻购买时跳转到购物车页面，否则显示弹出框
			if (isBuyNow) {
				return new JsonResult(new { redirectTo = "/cart?type=buynow" });
			}
			return new JsonResult(new { showDialog = new { totalCount = 0, totalPriceString = 1 } });
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
