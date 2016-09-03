using Newtonsoft.Json;
using System;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataConditionBinders.Bases;
using ZKWeb.Server;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataConditionBinders {
	/// <summary>
	/// 订购数量
	/// 值名 OrderCountGE
	/// 格式 整数
	/// </summary>
	[ExportMany]
	public class OrderCountGEBinder : ProductMatchedDataConditionBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(Guid? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var pathManager = Application.Ioc.Resolve<PathManager>();
			Contents = templateManager.RenderTemplate(
				"shopping.product/condition_binder.order_count_ge.html", null);
			Bind = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "condition_binders", "order_count_ge.bind.js"));
			Collect = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "condition_binders", "order_count_ge.collect.js"));
			Display = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "condition_binders", "order_count_ge.display.js"))
				.Replace("T_OrderCountGE", JsonConvert.SerializeObject(new T("OrderCountGE")));
			return true;
		}
	}
}
