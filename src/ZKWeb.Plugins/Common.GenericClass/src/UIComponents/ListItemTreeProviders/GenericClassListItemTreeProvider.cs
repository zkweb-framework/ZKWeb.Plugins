using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Plugins.Common.GenericClass.src.Controllers.Bases;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Services;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.GenericClass.src.UIComponents.ListItemProviders {
	/// <summary>
	/// 通用分类列表树的提供器
	/// </summary>
	/// <typeparam name="TController">定义通用分类的控制器类型</typeparam>
	public class GenericClassListItemTreeProvider<TController> :
		IListItemTreeProvider
		where TController : GenericClassControllerBase<TController>, new() {
		/// <summary>
		/// 获取选项列表树
		/// </summary>
		/// <returns></returns>
		public ITreeNode<ListItem> GetTree() {
			var type = new TController().Type;
			var manager = Application.Ioc.Resolve<GenericClassManager>();
			var classTree = manager.GetTreeWithCache(type);
			var tree = TreeUtils.Transform(classTree,
				c => c == null ? null : new ListItem(c.Name, c.Id.ToString()));
			return tree;
		}
	}
}
