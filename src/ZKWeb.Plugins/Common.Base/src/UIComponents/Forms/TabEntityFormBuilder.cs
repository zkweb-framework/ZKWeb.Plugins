using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms {
	/// <summary>
	/// 用于编辑指定数据的表单，支持多标签
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	/// <typeparam name="TForm">继承类自身的类型</typeparam>
	public abstract class TabEntityFormBuilder<TEntity, TPrimaryKey, TForm> :
		EntityFormBuilder<TEntity, TPrimaryKey, TForm>
		where TEntity : class, IEntity<TPrimaryKey>, new()
		where TForm : TabEntityFormBuilder<TEntity, TPrimaryKey, TForm> {
		/// <summary>
		/// 初始化
		/// </summary>
		public TabEntityFormBuilder() : base(new TabFormBuilder()) { }
	}
}
