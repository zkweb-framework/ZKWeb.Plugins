using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductMatchedDataConditionBinders {
	/// <summary>
	/// 规格条件
	/// 值名 Properties
	/// 格式 [ { PropertyId: ..., PropertyValueId: ... }, ... ]
	/// </summary>
	[ExportMany]
	public class PropertiesBinder : ProductMatchedDataConditionBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(long? categoryId) {
			var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			var category = categoryManager.FindCategory(categoryId ?? 0);
			var salesProperties = (category != null) ?
				categoryManager.GetProperties(category).Where(p => p.IsSaleProperty).ToList() :
				new List<ProductProperty>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var pathManager = Application.Ioc.Resolve<PathManager>();
			Contents = string.Join("", salesProperties.Select(property => {
				// 规格下拉框
				var propertyValues = categoryManager.GetPropertyValues(property);
				return templateManager.RenderTemplate(
					"shopping.product/condition_binder.property.dropdown.html",
					new { property, propertyValues });
			}));
			Bind = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "condition_binders", "property.bind.js"));
			Collect = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "condition_binders", "property.collect.js"));
			Display = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "condition_binders", "property.display.js"));
			return true;
		}
	}
}
