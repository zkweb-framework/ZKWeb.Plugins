using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductStates.Interfaces;

namespace ZKWeb.Plugins.Shopping.Product.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 商品状态的选项列表
	/// </summary>
	public class ProductStateListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var states = Application.Ioc.ResolveMany<IProductState>();
			return states.Select(t => new ListItem(new T(t.State), t.State));
		}
	}
}
