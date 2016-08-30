using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.GenericClass.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 通用分类列表项的提供器
	/// </summary>
	/// <typeparam name="TController">定义通用分类的控制器类型</typeparam>
	public class GenericClassListItemProvider<TController> : IListItemProvider
		where TController : CrudGenericClassControllerBase, new() {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var provider = new GenericClassListItemTreeProvider<TController>();
			return provider.GetTree().EnumerateAllNodes()
				.Where(n => n.Value != null)
				.Select(n => {
					var prefix = new string('　', n.GetParents().Count() - 1);
					return new ListItem(prefix + n.Value.Name, n.Value.Value);
				});
		}
	}
}
