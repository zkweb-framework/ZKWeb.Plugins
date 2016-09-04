using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWebStandard.Web;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Structs;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Extensions;

namespace ZKWeb.Plugins.Shopping.Order.src.Controllers {
	/// <summary>
	/// 购物车Api控制器
	/// </summary>
	[ExportMany]
	public class CartApiController : ControllerBase {
		/// <summary>
		/// 获取购物车商品的总数量
		/// </summary>
		/// <returns></returns>
		[Action("api/cart/product_total_count", HttpMethods.POST)]
		public IActionResult CartProductTotalCount() {
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			var totalCount = cartProductManager.GetCartProductTotalCount(CartProductType.Default);
			return new JsonResult(new { totalCount });
		}

		/// <summary>
		/// 获取迷你购物车的信息
		/// </summary>
		/// <returns></returns>
		[Action("api/cart/minicart_info")]
		public IActionResult MiniCartInfo() {
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			var info = cartProductManager.GetMiniCartApiInfo();
			return new JsonResult(info);
		}

		/// <summary>
		/// 获取购物车信息
		/// </summary>
		/// <returns></returns>
		[Action("api/cart/info", HttpMethods.POST)]
		public IActionResult CartInfo() {
			var type = Request.Get<string>("type");
			var cartProductType = (type == "buynow") ? CartProductType.Buynow : CartProductType.Default;
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			var info = cartProductManager.GetCartApiInfo(cartProductType);
			return new JsonResult(info);
		}

		/// <summary>
		/// 添加商品到购物车
		/// </summary>
		/// <returns></returns>
		[Action("api/cart/add", HttpMethods.POST)]
		public IActionResult Add() {
			this.RequireAjaxRequest();
			var request = Request;
			var productId = request.Get<Guid>("productId");
			var matchParameters = request.Get<ProductMatchParameters>("matchParameters");
			var isBuyNow = request.Get<bool>("isBuyNow");
			var cartProductType = isBuyNow ? CartProductType.Buynow : CartProductType.Default;
			// 添加购物车商品，抛出无权限错误时跳转到登陆页面
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			try {
				cartProductManager.AddCartProduct(productId, cartProductType, matchParameters);
			} catch (HttpException ex) {
				if (ex.StatusCode == 403) {
					return new JsonResult(new { redirectTo = "/user/login" });
				}
				throw;
			}
			// 立刻购买时跳转到购物车页面
			if (isBuyNow) {
				return new JsonResult(new { redirectTo = "/cart?type=buynow" });
			}
			// 加入购物车时显示弹出框，包含商品总数和价格
			var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
			var totalCount = cartProductManager.GetCartProductTotalCount(cartProductType);
			var totalPrices = cartProductManager.GetCartProductTotalPrice(
				cartProductManager.GetCartProducts(cartProductType));
			var totalPriceString = string.Join(", ", totalPrices.Select(
				pair => currencyManager.GetCurrency(pair.Key).Format(pair.Value)));
			return new JsonResult(new { showDialog = new { totalCount, totalPriceString } });
		}

		/// <summary>
		/// 删除购物车中的商品
		/// </summary>
		/// <returns></returns>
		[Action("api/cart/delete", HttpMethods.POST)]
		public IActionResult Delete() {
			this.RequireAjaxRequest();
			var id = Request.Get<Guid>("id");
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			cartProductManager.DeleteCartProduct(id);
			return new JsonResult(new { message = new T("Delete Successfully") });
		}

		/// <summary>
		/// 更新购物车中的商品数量
		/// </summary>
		/// <returns></returns>
		[Action("api/cart/update_counts", HttpMethods.POST)]
		public IActionResult UpdateCounts() {
			this.RequireAjaxRequest();
			var counts = Request.Get<IDictionary<long, long>>("counts");
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			cartProductManager.UpdateCartProductCounts(counts);
			return new JsonResult(new { });
		}

		/// <summary>
		/// 计算购物车中的商品价格
		/// </summary>
		/// <returns></returns>
		[Action("api/cart/calculate_price", HttpMethods.POST)]
		public IActionResult CalculatePrice() {
			this.RequireAjaxRequest();
			var createOrderParameters = Request
				.Get<CreateOrderParameters>("CreateOrderParameters") ?? new CreateOrderParameters();
			createOrderParameters.SetLoginInfo();
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			try {
				var info = cartProductManager.GetCartCalculatePriceApiInfo(createOrderParameters);
				return new JsonResult(new { priceInfo = info });
			} catch (Exception ex) {
				return new JsonResult(new { error = ex.Message });
			}
		}
	}
}
