using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// 商品控制器
	/// </summary>
	[ExportMany]
	public class ProductController : IController {
		/// <summary>
		/// 获取指定类目对应的属性编辑器
		/// 这里仅用于获取编辑器Html，不绑定数据也不检查权限
		/// </summary>
		/// <returns></returns>
		[Action("product/property_editor")]
		public IActionResult PropertyEditor() {
			var categoryId = HttpContextUtils.CurrentContext.Request.Get<long>("categoryId");
			var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			// 获取类目
			var category = categoryManager.FindCategory(categoryId);
			if (category == null) {
				return new PlainResult("");
			}
			// 获取销售和非销售属性的Html列表
			var salesProperties = new List<HtmlString>();
			var nonSalesProperties = new List<HtmlString>();
			foreach (var property in categoryManager.GetProperties(category)) {
				var html = property.GetEditHtml(category);
				(property.IsSaleProperty ? salesProperties : nonSalesProperties).Add(html);
			}
			return new TemplateResult("shopping.product/property_editor.html",
				new { salesProperties, nonSalesProperties });
		}

		/// <summary>
		/// 获取类目对应的商品匹配数据的绑定器列表
		/// 这里仅用于获取绑定器的Json，不绑定数据也不检查权限
		/// </summary>
		/// <returns></returns>
		[Action("product/matched_data_binders")]
		public IActionResult MatchedDataBinders() {
			var categoryId = HttpContextUtils.CurrentContext.Request.Get<long?>("categoryId");
			var conditionBinders = Application.Ioc
				.ResolveMany<ProductMatchedDataConditionBinder>()
				.Where(b => b.Init(categoryId)).ToList();
			var affectsBinders = Application.Ioc
				.ResolveMany<ProductMatchedDataAffectsBinder>()
				.Where(b => b.Init(categoryId)).ToList();
			return new JsonResult(new {
				ConditionBinders = conditionBinders,
				AffectsBinders = affectsBinders
			});
		}
	}
}
