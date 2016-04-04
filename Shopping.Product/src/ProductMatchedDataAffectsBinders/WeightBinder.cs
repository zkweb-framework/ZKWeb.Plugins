using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductMatchedDataAffectsBinders {
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
		public override bool Init(long? categoryId) {
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
