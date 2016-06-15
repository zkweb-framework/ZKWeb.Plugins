using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.ListItemProviders {
	/// <summary>
	/// 商品类目的选项列表
	/// </summary>
	public class ProductCategoryListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			var categoryList = categoryManager.GetCategoryList();
			foreach (var category in categoryList) {
				yield return new ListItem(new T(category.Name), category.Id.ToString());
			}
		}
	}
}
