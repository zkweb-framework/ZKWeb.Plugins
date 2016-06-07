using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWeb.Templating;
using ZKWeb.Utils.IocContainer;

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
			var category = categoryManager.GetCategory(categoryId ?? 0);
			var salesProperties = (category != null) ?
				category.OrderedProperties().Where(p => p.IsSalesProperty).ToList() :
				new List<ProductProperty>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var pathManager = Application.Ioc.Resolve<PathManager>();
			Contents = string.Join("", salesProperties.Select(property => {
				// 规格下拉框
				var propertyValues = property.OrderedPropertyValues().ToList();
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
