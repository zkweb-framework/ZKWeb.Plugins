using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems {
	/// <summary>
	/// 自定义选项列表提供器
	/// 请配合ListItemValueWithProvider使用
	/// </summary>
	public class CustomListItemProvider : IListItemProvider {
		/// <summary>
		/// 选项列表
		/// </summary>
		public IList<ListItem> Items { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="items">选项列表</param>
		public CustomListItemProvider(IList<ListItem> items) {
			Items = items;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="items">选项列表</param>
		public CustomListItemProvider(params ListItem[] items) : this(items.ToList()) { }

		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			return Items;
		}
	}
}
