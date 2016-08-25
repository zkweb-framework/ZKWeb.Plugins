using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Services {
	/// <summary>
	/// 领域服务的基础类
	/// 提供一系列基础功能
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public class DomainServiceBase<TEntity, TPrimaryKey> :
		IDomainService<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {

	}
}
