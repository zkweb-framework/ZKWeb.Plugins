using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases;
using ZKWeb.Storage;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders {
	/// <summary>
	/// 库存
	/// 值名 Stock
	/// 格式 整数
	/// </summary>
	[ExportMany]
	public class StockBinder : ProductMatchedDataAffectsBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(Guid? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			Header = new T("Stock");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.stock.html", null);
			Bind = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "stock.bind.js").ReadAllText();
			Collect = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "stock.collect.js").ReadAllText();
			return true;
		}
	}
}
