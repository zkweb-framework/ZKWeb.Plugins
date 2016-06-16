using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductMatchedDataAffectsBinders {
	/// <summary>
	/// 备注
	/// 值名 Remark
	/// 格式 字符串
	/// </summary>
	[ExportMany]
	public class RemarkBinder : ProductMatchedDataAffectsBinder {
		/// <summary>
		/// 初始化绑定器
		/// </summary>
		public override bool Init(long? categoryId) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var pathManager = Application.Ioc.Resolve<PathManager>();
			Header = new T("Remark");
			Contents = templateManager.RenderTemplate(
				 "shopping.product/affects_binder.remark.html", null);
			Bind = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "affects_binders", "remark.bind.js"));
			Collect = File.ReadAllText(pathManager.GetResourceFullPath(
				"static", "shopping.product.js", "affects_binders", "remark.collect.js"));
			return true;
		}
	}
}
