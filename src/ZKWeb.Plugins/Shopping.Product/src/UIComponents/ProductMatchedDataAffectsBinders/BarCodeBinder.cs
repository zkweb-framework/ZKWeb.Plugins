using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases;
using ZKWeb.Storage;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders {
	/// <summary>
	/// 条形码
	/// 值名 BarCode
	/// 格式 字符串
	/// </summary>
	[ExportMany]
	public class BarCodeBinder : ProductMatchedDataAffectsBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(Guid? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			Header = new T("BarCode");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.barcode.html", null);
			Bind = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "barcode.bind.js").ReadAllText();
			Collect = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "barcode.collect.js").ReadAllText();
			DisplayOrder = 600;
			return true;
		}
	}
}
