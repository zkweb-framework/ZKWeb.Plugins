using System;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases;
using ZKWeb.Server;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders {
	/// <summary>
	/// 价格
	/// 值名 Price
	/// 格式 浮点数
	/// </summary>
	[ExportMany]
	public class PriceBinder : ProductMatchedDataAffectsBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(Guid? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var pathManager = Application.Ioc.Resolve<PathManager>();
			Header = new T("Price");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.price.html", null);
			Bind = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "affects_binders", "price.bind.js"));
			Collect = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "affects_binders", "price.collect.js"));
			return true;
		}
	}
}
