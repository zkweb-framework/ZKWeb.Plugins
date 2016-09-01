using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;

namespace ZKWeb.Plugins.Common.Currency.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 货币列表
	/// </summary>
	public class CurrencyListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			foreach (var currency in Application.Ioc.ResolveMany<ICurrency>()) {
				yield return new ListItem(new T(currency.Type), currency.Type);
			}
		}
	}
}
