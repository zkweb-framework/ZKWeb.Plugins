using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.GenericClass.src.Manager;
using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.GenericClass.src.ListItemProviders {
	/// <summary>
	/// 通用分类列表的提供器
	/// </summary>
	public class GenericClassListItemProvider<TClass> : IListItemProvider
		where TClass : GenericClassBuilder, new() {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var provider = new GenericClassListItemTreeProvider<TClass>();
			return provider.GetTree().EnumerateAllNodes()
				.Where(n => n.Value != null)
				.Select(n => {
					var prefix = new string('　', n.GetParents().Count() - 1);
					return new ListItem(prefix + n.Value.Name, n.Value.Value);
				});
		}
	}
}
