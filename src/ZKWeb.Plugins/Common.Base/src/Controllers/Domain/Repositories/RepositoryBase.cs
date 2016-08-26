using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Repositories {
	/// <summary>
	/// 仓储的基础类
	/// 提供一系列基础操作
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class RepositoryBase<TEntity, TPrimaryKey> :
		IRepository<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 获取工作单元
		/// </summary>
		protected virtual IUnitOfWork UnitOfWork {
			get { return Application.Ioc.Resolve<IUnitOfWork>(); }
		}

		/// <summary>
		/// 查询实体
		/// </summary>
		public virtual IQueryable<TEntity> Query() {
			var uow = UnitOfWork;
			var query = uow.Context.Query<TEntity>();
			return uow.WrapQuery<TEntity, TPrimaryKey>(query);
		}

		/// <summary>
		/// 获取符合条件的单个实体
		/// </summary>
		public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate) {
			return Query().FirstOrDefault(predicate);
		}

		/// <summary>
		/// 计算符合条件的实体数量
		/// </summary>
		public long Count(Expression<Func<TEntity, bool>> predicate) {
			return Query().LongCount(predicate);
		}

		/// <summary>
		/// 添加或更新实体
		/// </summary>
		public virtual void Save(ref TEntity entity, Action<TEntity> update = null) {
			var uow = UnitOfWork;
			update = uow.WrapUpdateMethod<TEntity, TPrimaryKey>(update);
			uow.Context.Save(ref entity, update);
		}

		/// <summary>
		/// 删除实体
		/// </summary>
		public virtual void Delete(TEntity entity) {
			UnitOfWork.Context.Delete(entity);
		}

		/// <summary>
		/// 批量保存实体
		/// </summary>
		public virtual void BatchSave(
			ref IEnumerable<TEntity> entities, Action<TEntity> update = null) {
			var uow = UnitOfWork;
			update = uow.WrapUpdateMethod<TEntity, TPrimaryKey>(update);
			uow.Context.BatchSave(ref entities, update);
		}

		/// <summary>
		/// 批量更新实体
		/// </summary
		public virtual long BatchUpdate(
			Expression<Func<TEntity, bool>> predicate, Action<TEntity> update) {
			var uow = UnitOfWork;
			update = uow.WrapUpdateMethod<TEntity, TPrimaryKey>(update);
			return uow.Context.BatchUpdate(predicate, update);
		}

		/// <summary>
		/// 批量删除实体
		/// </summary>
		public virtual long BatchDelete(Expression<Func<TEntity, bool>> predicate) {
			return UnitOfWork.Context.BatchDelete(predicate);
		}
	}
}
