using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Shopping.Product.src.Database;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductSalesInfoDisplayFields {
	/// <summary>
	/// 品牌
	/// </summary>
	[ExportMany]
	public class BrandDisplayField : IProductSalesInfoDisplayField {
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get { return "Brand"; } }

		/// <summary>
		/// 获取显示的Html
		/// </summary>
		public string GetDisplayHtml(DatabaseContext context, Database.Product product) {
			// 获取名称中带品牌的属性并返回该值
			// 没有时返回null
			var value = product.FindPropertyValuesWhereNameContains(Name).FirstOrDefault();
			if (value != null) {
				return HttpUtils.HtmlEncode(new T(value.PropertyValueName));
			}
			return null;
		}
	}
}
