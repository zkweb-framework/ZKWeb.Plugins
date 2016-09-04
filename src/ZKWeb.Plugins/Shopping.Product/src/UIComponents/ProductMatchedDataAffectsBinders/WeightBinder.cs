using System;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases;
using ZKWeb.Server;
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
			var pathManager = Application.Ioc.Resolve<PathManager>();
			Header = new T("Weight(g)");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.weight.html", null);
			Bind = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "affects_binders", "weight.bind.js"));
			Collect = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "affects_binders", "weight.collect.js"));
			return true;
		}
	}
}
