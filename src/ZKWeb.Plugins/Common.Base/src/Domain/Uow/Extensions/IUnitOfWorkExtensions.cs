using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Uow.Extensions {
	/// <summary>
	/// 工作单元的扩展函数
	/// </summary>
	public static class IUnitOfWorkExtensions {
		/// <summary>
		/// 包装更新函数
		/// 应用工作单元中的过滤器
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="uow">工作单元</param>
		/// <param name="update">更新函数</param>
		/// <returns></returns>
		public static Action<TEntity> WrapUpdateMethod<TEntity, TPrimaryKey>(
			this IUnitOfWork uow, Action<TEntity> update)
			where TEntity : class, IEntity<TPrimaryKey> {
			return e => {
				foreach (var filter in uow.SaveFilters) {
					filter.Filter<TEntity, TPrimaryKey>(e);
				}
				update?.Invoke(e);
			};
		}

		/// <summary>
		/// 包装查询
		/// 应用工作单元中的过滤器
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="uow">工作单元</param>
		/// <param name="query">查询</param>
		/// <returns></returns>
		public static IQueryable<TEntity> WrapQuery<TEntity, TPrimaryKey>(
			this IUnitOfWork uow, IQueryable<TEntity> query)
			where TEntity : class, IEntity<TPrimaryKey> {
			foreach (var filter in uow.QueryFilters) {
				query = filter.Filter<TEntity, TPrimaryKey>(query);
			}
			return query;
		}

		/// <summary>
		/// 在一定范围内禁用指定类型的查询过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filterType">查询过滤器类型</param>
		/// <returns></returns>
		public static IDisposable DisableQueryFilter(this IUnitOfWork uow, Type filterType) {
			var oldFilters = uow.QueryFilters;
			var filterTypeInfo = filterType.GetTypeInfo();
			uow.QueryFilters = uow.QueryFilters.Where(
				f => !filterTypeInfo.IsAssignableFrom(f.GetType())).ToList();
			return new SimpleDisposable(() => uow.QueryFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内禁用所有查询过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <returns></returns>
		public static IDisposable DisableQueryFilters(this IUnitOfWork uow) {
			var oldFilters = uow.QueryFilters;
			uow.QueryFilters = new List<IEntityQueryFilter>();
			return new SimpleDisposable(() => uow.QueryFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内禁用指定类型的保存过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filterType">过滤器类型</param>
		/// <returns></returns>
		public static IDisposable DisableSaveFilter(this IUnitOfWork uow, Type filterType) {
			var oldFilters = uow.SaveFilters;
			var filterTypeInfo = filterType.GetTypeInfo();
			uow.SaveFilters = uow.SaveFilters.Where(
				f => !filterTypeInfo.IsAssignableFrom(f.GetType())).ToList();
			return new SimpleDisposable(() => uow.SaveFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内禁用所有保存过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <returns></returns>
		public static IDisposable DisableSaveFilters(this IUnitOfWork uow) {
			var oldFilters = uow.SaveFilters;
			uow.SaveFilters = new List<IEntitySaveFilter>();
			return new SimpleDisposable(() => uow.SaveFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内使用指定的查询过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filter">查询过滤器</param>
		/// <returns></returns>
		public static IDisposable EnableQueryFilter(this IUnitOfWork uow, IEntityQueryFilter filter) {
			var oldFilters = uow.QueryFilters;
			uow.QueryFilters = uow.QueryFilters.ConcatIfNotNull(filter).ToList();
			return new SimpleDisposable(() => uow.QueryFilters = oldFilters);
		}

		/// <summary>
		/// 在一定范围内使用指定的保存过滤器
		/// </summary>
		/// <param name="uow">工作单元</param>
		/// <param name="filter">保存过滤器</param>
		/// <returns></returns>
		public static IDisposable EnableSaveFilter(this IUnitOfWork uow, IEntitySaveFilter filter) {
			var oldFilters = uow.SaveFilters;
			uow.SaveFilters = uow.SaveFilters.ConcatIfNotNull(filter).ToList();
			return new SimpleDisposable(() => uow.SaveFilters = oldFilters);
		}
	}
}
