using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 网站附加配置中使用的键
	/// </summary>
	internal class ExtraConfigKeys {
		/// <summary>
		/// 商品类目信息的缓存时间，单位是秒
		/// </summary>
		public const string ProductCategoryCacheTime = "Shopping.Product.ProductCategoryCacheTime";
		/// <summary>
		/// 商品的缓存时间，单位是秒
		/// </summary>
		public const string ProductCacheTime = "Shopping.Product.ProdutCacheTime";
		/// <summary>
		/// 商品信息的缓存时间，单位是秒
		/// </summary>
		public const string ProductApiInfoCacheTime = "Shopping.Product.ProductApiInfoCacheTime";
		/// <summary>
		/// 商品搜索结果的缓存时间，单位是秒
		/// </summary>
		public const string ProductSearchResultCacheTime = "Shopping.Product.ProductSearchResultCacheTime";
	}
}
