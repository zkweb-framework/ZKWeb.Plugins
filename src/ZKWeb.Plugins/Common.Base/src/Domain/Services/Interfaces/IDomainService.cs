using System;
using System.Collections.Generic;
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
		/// <summary>
		/// 根据主键获取实体
		/// </summary>
		/// <param name="id">主键Id</param>
		/// <returns></returns>
		TEntity Get(TPrimaryKey id);

		/// <summary>
		/// 根据主键获取实体
		/// 如果实体标记为已删除则返回null
		/// </summary>
		/// <param name="id">主键Id</param>
		/// <returns></returns>
		TEntity GetIfNotDeleted(TPrimaryKey id);

		/// <summary>
		/// 保存实体
		/// </summary>
		/// <param name="entity">实体</param>
		/// <param name="update">更新函数</param>
		void Save(ref TEntity entity, Action<TEntity> update = null);

		/// <summary>
		/// 删除实体
		/// </summary>
		/// <param name="entity">实体</param>
		void Delete(TEntity entity);

		/// <summary>
		/// 批量标记已删除
		/// 不会实际删除
		/// </summary>
		/// <param name="ids">实体Id列表</param>
		/// <returns></returns>
		long BatchSetDeleted(IEnumerable<TPrimaryKey> ids);

		/// <summary>
		/// 批量标记未删除
		/// </summary>
		/// <param name="ids">实体Id列表</param>
		/// <returns></returns>
		long BatchUnsetDeleted(IEnumerable<TPrimaryKey> ids);

		/// <summary>
		/// 批量永久删除
		/// </summary>
		/// <param name="ids">批量永久删除</param>
		/// <returns></returns>
		long BatchDeleteForever(IEnumerable<TPrimaryKey> ids);
	}
}
