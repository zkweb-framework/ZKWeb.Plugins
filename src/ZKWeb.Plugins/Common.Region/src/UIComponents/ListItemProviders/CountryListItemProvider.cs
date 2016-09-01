using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Common.Region.src.Components.Countries.Bases;

namespace ZKWeb.Plugins.Common.Region.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 国家/行政区列表
	/// </summary>
	public class CountryListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			foreach (var country in Application.Ioc.ResolveMany<Country>()) {
				yield return new ListItem(new T(country.Name), country.Name);
			}
		}
	}
}
