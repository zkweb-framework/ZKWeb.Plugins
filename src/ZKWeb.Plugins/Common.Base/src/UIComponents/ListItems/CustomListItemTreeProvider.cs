using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems {
	/// <summary>
	/// 自定义选项树提供器
	/// 请配合ListItemValueWithProvider使用
	/// </summary>
	public class CustomListItemTreeProvider : IListItemTreeProvider {
		/// <summary>
		/// 选项树
		/// </summary>
		public ITreeNode<ListItem> Tree { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="tree">选项树</param>
		public CustomListItemTreeProvider(ITreeNode<ListItem> tree) {
			Tree = tree;
		}

		/// <summary>
		/// 获取选项树
		/// </summary>
		/// <returns></returns>
		public ITreeNode<ListItem> GetTree() {
			return Tree;
		}
	}
}
