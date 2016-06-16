using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 勾选框使用的选项树来源的接口
	/// </summary>
	public interface IListItemTreeProvider {
		/// <summary>
		/// 获取选项树
		/// </summary>
		/// <returns></returns>
		ITreeNode<ListItem> GetTree();
	}
}
