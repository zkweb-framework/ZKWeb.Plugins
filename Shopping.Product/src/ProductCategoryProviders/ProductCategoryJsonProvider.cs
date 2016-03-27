using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Collections;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductCategoryProviders {
	/// <summary>
	/// 商品类目提供器
	/// 来源 Json文件
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ProductCategoryJsonProvider : IProductCategoryProvider, ICacheCleaner {
		/// <summary>
		/// 类目列表的缓存
		/// </summary>
		protected LazyCache<IList<ProductCategory>> Categories = LazyCache.Create(() => {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetResourceFullPath("texts", "product_categories.json");
			var json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<IList<ProductCategory>>(json);
		});
		/// <summary>
		/// 属性列表的缓存
		/// </summary>
		protected LazyCache<IList<ProductProperty>> Properties = LazyCache.Create(() => {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetResourceFullPath("texts", "product_properties.json");
			var json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<IList<ProductProperty>>(json);
		});
		/// <summary>
		/// 属性值列表的缓存
		/// </summary>
		protected LazyCache<IList<ProductPropertyValue>> PropertyValues = LazyCache.Create(() => {
			var pathManager = Application.Ioc.Resolve<PathManager>();
			var path = pathManager.GetResourceFullPath("texts", "product_property_values.json");
			var json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<IList<ProductPropertyValue>>(json);
		});

		/// <summary>
		/// 获取类目列表
		/// </summary>
		public IEnumerable<ProductCategory> GetCategories() {
			return Categories.Value;
		}

		/// <summary>
		/// 获取属性列表
		/// </summary>
		public IEnumerable<ProductProperty> GetProperties() {
			return Properties.Value;
		}

		/// <summary>
		/// 获取属性值列表
		/// </summary>
		public IEnumerable<ProductPropertyValue> GetPropertyValues() {
			return PropertyValues.Value;
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			Categories.Reset();
			Properties.Reset();
			PropertyValues.Reset();
		}
	}
}
