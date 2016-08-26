using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Interfaces {
	/// <summary>
	/// 静态表格的扩展处理器
	/// 可以用于扩展原有的处理器
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	/// <typeparam name="TOriginalHandler">原有的处理器</typeparam>
	public interface IStaticTableCallbackExtension<TEntity, TPrimaryKey, TOriginalHandler> :
		IStaticTableHandler<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey>
		where TOriginalHandler : IStaticTableHandler<TEntity, TPrimaryKey> { }
}
