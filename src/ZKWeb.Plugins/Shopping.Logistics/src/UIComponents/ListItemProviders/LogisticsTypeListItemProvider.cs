using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Shopping.Logistics.src.Components.LogisticsTypes.Interfaces;

namespace ZKWeb.Plugins.Shopping.Logistics.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 物流类型的选项列表
	/// </summary>
	public class LogisticsTypeListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var types = Application.Ioc.ResolveMany<ILogisticsType>();
			return types.Select(t => new ListItem(new T(t.Type), t.Type));
		}
	}
}
