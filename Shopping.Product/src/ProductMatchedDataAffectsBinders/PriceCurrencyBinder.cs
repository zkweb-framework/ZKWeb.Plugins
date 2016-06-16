using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Currency.src.ListItemProviders;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductMatchedDataAffectsBinders {
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
		public override bool Init(long? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var currencies = new ListItemsWithOptional<CurrencyListItemProvider>().GetItems();
			Header = new T("PriceCurrency");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.price_currency.html", new { currencies });
			Bind = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "affects_binders", "price_currency.bind.js"));
			Collect = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "affects_binders", "price_currency.collect.js"));
			return true;
		}
	}
}
