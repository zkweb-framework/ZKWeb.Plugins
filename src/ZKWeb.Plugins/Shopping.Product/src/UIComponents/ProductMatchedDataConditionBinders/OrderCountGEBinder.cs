using Newtonsoft.Json;
using System;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataConditionBinders.Bases;
using ZKWeb.Server;
using ZKWeb.Storage;
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
			var fileStorage = Application.Ioc.Resolve<IFileStorage>();
			Contents = templateManager.RenderTemplate(
				"shopping.product/condition_binder.order_count_ge.html", null);
			Bind = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "condition_binders", "order_count_ge.bind.js").ReadAllText();
			Collect = fileStorage.GetResourceFile(
				"static", "shopping.product.js", "condition_binders", "order_count_ge.collect.js").ReadAllText();
			Display = fileStorage
				.GetResourceFile("static", "shopping.product.js", "condition_binders", "order_count_ge.display.js")
				.ReadAllText()
				.Replace("T_OrderCountGE", JsonConvert.SerializeObject(new T("OrderCountGE")));
			return true;
		}
	}
}
