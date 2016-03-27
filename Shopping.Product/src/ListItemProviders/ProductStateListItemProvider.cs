using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Product.src.Model;

namespace ZKWeb.Plugins.Shopping.Product.src.ListItemProviders {
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
