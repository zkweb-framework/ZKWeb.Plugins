using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Logistics.src.Model;

namespace ZKWeb.Plugins.Shopping.Logistics.src.ListItemProviders {
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
