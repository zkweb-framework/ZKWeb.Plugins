using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Services {
	/// <summary>
	/// 领域服务的基础类
	/// 提供一系列基础功能
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class DomainServiceBase<TEntity, TPrimaryKey> :
		IDomainService<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 获取工作单元
		/// </summary>
		protected virtual IUnitOfWork UnitOfWork {
			get { return Application.Ioc.Resolve<IUnitOfWork>(); }
		}

		/// <summary>
		/// 获取仓储
		/// </summary>
		protected virtual IRepository<TEntity, TPrimaryKey> Repository {
			get { return Application.Ioc.Resolve<IRepository<TEntity, TPrimaryKey>>(); }
		}

		/// <summary>
		/// 根据主键获取实体
		/// </summary>
		public virtual TEntity Get(TPrimaryKey id) {
			using (UnitOfWork.Scope()) {
				return Repository.Get(e => e.Id.Equals(id));
			}
		}

		/// <summary>
		/// 根据主键获取实体
		/// 如果实体标记为已删除则返回null
		/// </summary>
		public virtual TEntity GetIfNotDeleted(TPrimaryKey id) {
			var entity = Get(id);
			if (entity == null ||
				(entity is IHaveDeleted) && ((IHaveDeleted)entity).Deleted) {
				return null;
			}
			return entity;
		}

		/// <summary>
		/// 保存实体
		/// </summary>
		public virtual void Save(ref TEntity entity, Action<TEntity> update) {
			using (UnitOfWork.Scope()) {
				Repository.Save(ref entity, update);
			}
		}

		/// <summary>
		/// 删除实体
		/// </summary>
		public virtual void Delete(TEntity entity) {
			using (UnitOfWork.Scope()) {
				Repository.Delete(entity);
			}
		}

		/// <summary>
		/// 批量标记已删除
		/// 不会实际删除
		/// </summary>
		public virtual long BatchSetDeleted(IEnumerable<TPrimaryKey> ids) {
			using (UnitOfWork.Scope()) {
				var entities = Repository.Query()
					.Where(e => ids.Contains(e.Id)).AsEnumerable();
				Repository.BatchSave(
					ref entities, e => ((IHaveDeleted)e).Deleted = true);
				return entities.LongCount();
			}
		}

		/// <summary>
		/// 批量标记未删除
		/// </summary>
		public virtual long BatchUnsetDeleted(IEnumerable<TPrimaryKey> ids) {
			using (UnitOfWork.Scope()) {
				var entities = Repository.Query()
					.Where(e => ids.Contains(e.Id)).AsEnumerable();
				Repository.BatchSave(
					ref entities, e => ((IHaveDeleted)e).Deleted = false);
				return entities.LongCount();
			}
		}

		/// <summary>
		/// 批量永久删除
		/// </summary>
		public virtual long BatchDeleteForever(IEnumerable<TPrimaryKey> ids) {
			using (UnitOfWork.Scope()) {
				return Repository.BatchDelete(e => ids.Contains(e.Id));
			}
		}
	}
}
