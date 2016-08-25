using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Utils;

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
		/// 包装更新函数
		/// </summary>
		protected virtual Action<TEntity> WrapUpdateMethod(Action<TEntity> update) {
			return e => {
				// 自动设置创建时间
				var now = DateTime.UtcNow;
				if (e is IHaveCreateTime) {
					var et = (IHaveCreateTime)e;
					if (et.CreateTime == default(DateTime)) {
						et.CreateTime = now;
					}
				}
				// 自动设置更新时间
				if (e is IHaveUpdateTime) {
					((IHaveUpdateTime)e).UpdateTime = now;
				}
				// 自动设置guid主键
				if (typeof(TPrimaryKey) == typeof(Guid)) {
					var eg = (IEntity<Guid>)e;
					if (eg.Id == Guid.Empty) {
						eg.Id = GuidUtils.SequentialGuid(now);
					}
				}
				update?.Invoke(e);
			};
		}

		/// <summary>
		/// 查询实体
		/// </summary>
		public virtual IQueryable<TEntity> Query() {
			return UnitOfWork.Context.Query<TEntity>();
		}

		/// <summary>
		/// 获取符合条件的单个实体
		/// </summary>
		public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate) {
			return UnitOfWork.Context.Get(predicate);
		}

		/// <summary>
		/// 计算符合条件的实体数量
		/// </summary>
		public long Count(Expression<Func<TEntity, bool>> predicate) {
			return UnitOfWork.Context.Count(predicate);
		}

		/// <summary>
		/// 添加或更新实体
		/// </summary>
		public virtual void Save(ref TEntity entity, Action<TEntity> update) {
			UnitOfWork.Context.Save(ref entity, WrapUpdateMethod(update));
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
			ref IEnumerable<TEntity> entities, Action<TEntity> update) {
			UnitOfWork.Context.BatchSave(ref entities, WrapUpdateMethod(update));
		}

		/// <summary>
		/// 批量更新实体
		/// </summary
		public virtual long BatchUpdate(
			Expression<Func<TEntity, bool>> predicate, Action<TEntity> update) {
			return UnitOfWork.Context.BatchUpdate(predicate, WrapUpdateMethod(update));
		}

		/// <summary>
		/// 批量删除实体
		/// </summary>
		public virtual long BatchDelete(Expression<Func<TEntity, bool>> predicate) {
			return UnitOfWork.Context.BatchDelete(predicate);
		}
	}
}
