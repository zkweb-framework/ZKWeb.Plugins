using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.GenericTag.src.Manager;
using ZKWeb.Plugins.Common.GenericTag.src.Scaffolding;

namespace ZKWeb.Plugins.Common.GenericTag.src.ListItemProvider {
	/// <summary>
	/// 通用标签列表的提供器
	/// </summary>
	public class GenericTagListItemProvider<TTag> : IListItemProvider
		where TTag : GenericTagBuilder, new() {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var type = new TTag().Type;
			var manager = Application.Ioc.Resolve<GenericTagManager>();
			var tags = manager.GetTags(type);
			return tags.Select(t => new ListItem(t.Name, t.Id.ToString())).ToList();
		}
	}
}
