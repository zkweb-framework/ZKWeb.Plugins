using System;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases;
using ZKWeb.Server;
using ZKWeb.Storage;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders {
	/// <summary>
	/// 重量
	/// 值名 Weight
	/// 格式 小数
	/// </summary>
	[ExportMany]
	public class WeightBinder : ProductMatchedDataAffectsBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(Guid? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			Header = new T("Weight(g)");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.weight.html", null);
			Bind = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "weight.bind.js").ReadAllText();
			Collect = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "weight.collect.js").ReadAllText();
			return true;
		}
	}
}
