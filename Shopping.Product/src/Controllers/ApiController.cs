using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ApiController : IController {
		/// <summary>
		/// 获取商品信息
		/// 不存在时返回null
		/// </summary>
		/// <returns></returns>
		[Action("api/product/info", HttpMethods.POST)]
		public IActionResult ProductInfo() {
			var id = HttpContextUtils.CurrentContext.Request.Get<long>("id");
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var info = productManager.GetProductApiInfo(id);
			return new JsonResult(info);
		}

		/// <summary>
		/// 获取商品匹配数据的匹配器列表
		/// </summary>
		/// <returns></returns>
		[Action("api/product/matched_data_matchers", HttpMethods.POST)]
		public IActionResult MatchedDataMatchers() {
			var matchers = Application.Ioc.ResolveMany<IProductMatchedDataMatcher>();
			return new JsonResult(matchers.Select(m => m.GetJavascriptMatchFunction()).ToList());
		}
	}
}
