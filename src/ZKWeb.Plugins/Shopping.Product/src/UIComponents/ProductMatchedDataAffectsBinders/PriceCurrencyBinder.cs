using System;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Currency.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases;
using ZKWeb.Storage;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders {
	/// <summary>
	/// 货币
	/// 值名 PriceCurrency
	/// 格式 字符串（Currency类型）
	/// </summary>
	[ExportMany]
	public class PriceCurrencyBinder : ProductMatchedDataAffectsBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(Guid? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			var currencies = new ListItemsWithOptional<CurrencyListItemProvider>().GetItems();
			Header = new T("PriceCurrency");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.price_currency.html", new { currencies });
			Bind = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "price_currency.bind.js").ReadAllText();
			Collect = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "affects_binders", "price_currency.collect.js").ReadAllText();
			DisplayOrder = 200;
			return true;
		}
	}
}
