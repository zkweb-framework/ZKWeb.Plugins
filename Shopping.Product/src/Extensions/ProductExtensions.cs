using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.TypeTraits;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;

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

		/// <summary>
		/// 根据名称查找商品的属性
		/// 属性名包含指定的名称时返回该属性
		/// </summary>
		/// <param name="product">商品</param>
		/// <param name="name">属性名称，可以是翻译之前的</param>
		/// <returns></returns>
		public static IEnumerable<ProductToPropertyValue> FindPropertyValuesWhereNameContains(
			this Database.Product product, string name) {
			// 商品没有类目时结束查找
			if (product.Category == null) {
				yield break;
			}
			// 获取属性名称的所有翻译
			var translateManager = Application.Ioc.Resolve<TranslateManager>();
			var languages = Application.Ioc.ResolveMany<ILanguage>();
			var translatedNames = new HashSet<string>(
				languages.Select(l => translateManager.Translate(name, l.Name)));
			// 过滤包含指定名称的属性，并返回属性值
			var productCategoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			var propertyNames = product.Category.Properties.ToDictionary(p => p.Id, p => p.Name);
			foreach (var propertyValue in product.PropertyValues) {
				var propertyName = propertyValue.Property.Name ?? "";
				if (translatedNames.Any(n => propertyName.Contains(n))) {
					yield return propertyValue;
				}
			}
		}

		/// <summary>
		/// 获取商品状态的特征
		/// </summary>
		/// <param name="product">商品</param>
		/// <returns></returns>
		public static ProductStateTrait GetStateTrait(this Database.Product product) {
			var type = Application.Ioc.ResolveMany<IProductState>()
				.First(s => s.State == product.State).GetType();
			return ProductStateTrait.For(type);
		}

		/// <summary>
		/// 获取商品类型的特征
		/// </summary>
		/// <param name="product">商品</param>
		/// <returns></returns>
		public static ProductTypeTrait GetTypeTrait(this Database.Product product) {
			var type = Application.Ioc.ResolveMany<IProductType>()
				.First(t => t.Type == product.Type).GetType();
			return ProductTypeTrait.For(type);
		}
	}
}
