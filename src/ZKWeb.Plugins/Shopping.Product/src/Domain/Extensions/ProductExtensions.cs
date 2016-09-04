using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Mscorlib.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions {
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
		public static HtmlString GetSummaryHtml(this Entities.Product product, int truncateSize = 25) {
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
			this Entities.Product product, string name) {
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
		/// 根据属性参数查找商品的属性
		/// 属性在属性参数中时返回该属性
		/// </summary>
		/// <param name="product">商品</param>
		/// <param name="properties">商品属性参数</param>
		/// <returns></returns>
		public static IEnumerable<ProductToPropertyValue> FindPropertyValuesFromPropertyParameters(
			this Entities.Product product, IList<ProductMatchParametersExtensions.PropertyParameter> properties) {
			foreach (var propertyValue in product.PropertyValues) {
				if (propertyValue.PropertyValue != null &&
					properties.Any(p =>
						p.PropertyId == propertyValue.Property.Id &&
						p.PropertyValueId == propertyValue.PropertyValue.Id)) {
					yield return propertyValue;
				}
			}
		}

		/// <summary>
		/// 获取商品状态
		/// </summary>
		/// <param name="product">商品</param>
		/// <returns></returns>
		public static IProductState GetProductState(this Entities.Product product) {
			return Application.Ioc.ResolveMany<IProductState>()
				.FirstOrDefault(s => s.State == product.State);
		}

		/// <summary>
		/// 获取商品类型
		/// </summary>
		/// <param name="product">商品</param>
		/// <returns></returns>
		public static IProductType GetProductType(this Entities.Product product) {
			return Application.Ioc.ResolveMany<IProductType>()
				.FirstOrDefault(t => t.Type == product.Type);
		}
	}
}
