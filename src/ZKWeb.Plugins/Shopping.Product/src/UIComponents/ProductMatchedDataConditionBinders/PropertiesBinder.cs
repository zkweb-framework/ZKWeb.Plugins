using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataConditionBinders.Bases;
using ZKWeb.Server;
using ZKWeb.Storage;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataConditionBinders {
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
		public override bool Init(Guid? categoryId) {
			var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			var category = categoryManager.GetWithCache(categoryId ?? Guid.Empty);
			var salesProperties = (category != null) ?
				category.OrderedProperties().Where(p => p.IsSalesProperty).ToList() :
				new List<ProductProperty>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			Contents = string.Join("", salesProperties.Select(property => {
				// 规格下拉框
				var propertyValues = property.OrderedPropertyValues().ToList();
				return templateManager.RenderTemplate(
					"shopping.product/condition_binder.property.dropdown.html",
					new { property, propertyValues });
			}));
			Bind = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "condition_binders", "property.bind.js").ReadAllText();
			Collect = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "condition_binders", "property.collect.js").ReadAllText();
			Display = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "condition_binders", "property.display.js").ReadAllText();
			DisplayOrder = 200;
			return true;
		}
	}
}
