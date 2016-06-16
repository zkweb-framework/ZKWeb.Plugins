using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.GenericClass.src.Manager;
using ZKWeb.Plugins.Common.GenericClass.src.Scaffolding;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.GenericClass.src.ListItemProviders {
	/// <summary>
	/// 通用分类列表树的提供器
	/// </summary>
	public class GenericClassListItemTreeProvider<TClass> : IListItemTreeProvider
		where TClass : GenericClassBuilder, new() {
		/// <summary>
		/// 获取选项列表树
		/// </summary>
		/// <returns></returns>
		public ITreeNode<ListItem> GetTree() {
			var type = new TClass().Type;
			var manager = Application.Ioc.Resolve<GenericClassManager>();
			var classTree = manager.GetClassTree(type);
			var tree = TreeUtils.Transform(classTree,
				c => c == null ? null : new ListItem(c.Name, c.Id.ToString()));
			return tree;
		}
	}
}
