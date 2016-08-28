using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces {
	/// <summary>
	/// 实体表单使用的附加处理器
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	/// <typeparam name="TForm">表单类型</typeparam>
	public interface IEntityFormExtraHandler<TEntity, TPrimaryKey, TForm>
		where TEntity : class, IEntity<TPrimaryKey>, new() {
		/// <summary>
		/// 表单创建时的处理
		/// </summary>
		/// <param name="form">表单</param>
		void OnCreated(TForm form);

		/// <summary>
		/// 绑定数据到表单的处理，这个函数会在原表单绑定后调用
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="bindFrom">来源的实体</param>
		void OnBind(TForm form, TEntity bindFrom);

		/// <summary>
		/// 保存表单到数据，这个函数会在原表单保存后调用
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="saveTo">保存到的实体</param>
		void OnSubmit(TForm form, TEntity saveTo);

		/// <summary>
		/// 数据保存后的处理，用于添加关联数据等
		/// </summary>
		/// <param name="form">表单</param>
		/// <param name="context">数据库上下文</param>
		/// <param name="saved">已保存的实体，Id已分配</param>
		void OnSubmitSaved(TForm form, TEntity saved);
	}
}
