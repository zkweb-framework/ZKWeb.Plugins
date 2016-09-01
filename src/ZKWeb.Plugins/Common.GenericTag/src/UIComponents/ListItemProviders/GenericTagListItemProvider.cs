using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Common.GenericTag.src.Controllers.Bases;
using ZKWeb.Plugins.Common.GenericTag.src.Domain.Services;

namespace ZKWeb.Plugins.Common.GenericTag.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 通用标签列表的提供器
	/// </summary>
	/// <typeparam name="TController">定义通用标签的控制器类型</typeparam>
	public class GenericTagListItemProvider<TController> : IListItemProvider
		where TController : GenericTagControllerBase<TController>, new() {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var type = new TController().Type;
			var manager = Application.Ioc.Resolve<GenericTagManager>();
			var tags = manager.GetManyWithCache(type);
			return tags.Select(t => new ListItem(t.Name, t.Id.ToString())).ToList();
		}
	}
}
