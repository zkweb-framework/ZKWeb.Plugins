using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 商品排序类型的选项列表提供器
	/// </summary>
	[ExportMany(ContractKey = "ProductSortTypeListItemProvider")]
	public class ProductSortTypeListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			yield return new ListItem(new T("Default"), "default");
			yield return new ListItem(new T("BestSales"), "best_sales");
			yield return new ListItem(new T("LowerPrice"), "lower_price");
			yield return new ListItem(new T("HigherPrice"), "higher_price");
			yield return new ListItem(new T("NewestOnSale"), "newest_on_sale");
		}
	}
}
