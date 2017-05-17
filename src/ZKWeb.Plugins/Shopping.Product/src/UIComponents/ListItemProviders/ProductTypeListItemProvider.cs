using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductTypes.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 商品类型的选项列表
	/// </summary>
	[ExportMany(ContractKey = "ProductTypeListItemProvider")]
	public class ProductTypeListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var types = Application.Ioc.ResolveMany<IProductType>();
			return types.Select(t => new ListItem(new T(t.Type), t.Type));
		}
	}
}
