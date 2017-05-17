using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 商品销售属性的选项列表
	/// </summary>
	[ExportMany(ContractKey = "ProductSalesPropertyListItemProvider")]
	public class ProductSalesPropertyListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var categoryManager = Application.Ioc.Resolve<ProductPropertyManager>();
			foreach (var property in categoryManager.GetManyWithCache()) {
				if (property.IsSalesProperty) {
					yield return new ListItem(new T(property.Name), property.Id.ToString());
				}
			}
		}
	}
}
