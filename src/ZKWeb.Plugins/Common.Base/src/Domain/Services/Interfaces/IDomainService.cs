using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces {
	/// <summary>
	/// 领域服务的接口
	/// </summary>
	public interface IDomainService { }

	/// <summary>
	/// 领域服务的接口
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public interface IDomainService<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {

	}
}
