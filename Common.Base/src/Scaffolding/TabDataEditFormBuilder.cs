using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Scaffolding {
	/// <summary>
	/// 用于编辑指定数据的表单，支持多标签
	/// </summary>
	/// <typeparam name="TData">编辑的数据类型</typeparam>
	/// <typeparam name="TForm">继承类自身的类型</typeparam>
	public abstract class TabDataEditFormBuilder<TData, TForm> :
		DataEditFormBuilder<TData, TForm> where TData : class, IEntity, new() {
		/// <summary>
		/// 初始化
		/// </summary>
		public TabDataEditFormBuilder() : base(new TabFormBuilder()) { }
	}
}
