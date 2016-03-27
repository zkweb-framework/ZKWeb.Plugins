using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Shopping.Product.src.Extensions {
	/// <summary>
	/// 商品的扩展函数
	/// </summary>
	public static class ProductExtensions {
		/// <summary>
		/// 获取商品概述的Html
		/// 一般用于后台商品列表页等表格页面中
		/// </summary>
		/// <param name="product">商品</param>
		/// <param name="truncateSize">商品名称的截取长度</param>
		/// <returns></returns>
		public static HtmlString GetSummaryHtml(this Database.Product product, int truncateSize = 25) {
			var albumManager = Application.Ioc.Resolve<ProductAlbumManager>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var imageWebPath = albumManager.GetAlbumImageWebPath(
				product.Id, null, ProductAlbumImageType.Thumbnail);
			var html = templateManager.RenderTemplate("shopping.product/tmpl.product_summary.html", new {
				productId = product.Id,
				imageWebPath,
				name = product.Name,
				nameTruncated = product.Name.TruncateWithSuffix(truncateSize)
			});
			return new HtmlString(html);
		}
	}
}
