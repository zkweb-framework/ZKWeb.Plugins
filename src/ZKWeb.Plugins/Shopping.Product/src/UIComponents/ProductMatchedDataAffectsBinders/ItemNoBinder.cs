using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases;
using ZKWeb.Storage;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders {
	/// <summary>
	/// 货号
	/// 值名 ItemNo
	/// 格式 字符串
	/// </summary>
	[ExportMany]
	public class ItemNoBinder : ProductMatchedDataAffectsBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(Guid? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			Header = new T("ItemNo");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.item_no.html", null);
			Bind = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "item_no.bind.js").ReadAllText();
			Collect = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "item_no.collect.js").ReadAllText();
			DisplayOrder = 500;
			return true;
		}
	}
}
