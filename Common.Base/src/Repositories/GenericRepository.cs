using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.Repositories {
	/// <summary>
	/// 通用的数据仓储
	/// 需要在创建后由外部指定数据库上下文，但不负责数据库上下文的提交和释放
	/// 注册这个类型到IoC容器时不可以使用单例，否则多线程下会出现问题
	/// </summary>
	public class GenericRepository<TData> : IRepository
		where TData : class {
		/// <summary>
		/// 数据库上下文，需要在创建后设置
		/// </summary>
		public virtual DatabaseContext Context { get; set; }

		/// <summary>
		/// 获取满足条件的单个对象，找不到时返回null
		/// </summary>
		/// <param name="expression">查询条件</param>
		/// <returns></returns>
		public virtual TData Get(Expression<Func<TData, bool>> expression) {
			return Context.Get(expression);
		}

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="update">更新数据的函数</param>
		public virtual void Save(ref TData data, Action<TData> update = null) {
			Context.Save(ref data, update);
		}

		/// <summary>
		/// 删除数据
		/// </summary>
		/// <param name="data">数据</param>
		public virtual void Delete(TData data) {
			Context.Delete(data);
		}

		/// <summary>
		/// 批量删除 返回删除的数量
		/// </summary>
		/// <param name="expression">删除条件</param>
		/// <returns></returns>
		public virtual long DeleteWhere(Expression<Func<TData, bool>> expression) {
			return Context.DeleteWhere(expression);
		}

		/// <summary>
		/// 获取满足条件的对象数量
		/// </summary>
		/// <param name="expression">表达式</param>
		/// <returns></returns>
		public virtual long Count(Expression<Func<TData, bool>> expression) {
			return Context.Count(expression);
		}

		/// <summary>
		/// 获取指定Id的单个对象，找不到时返回null
		/// </summary>
		/// <param name="id">数据Id</param>
		/// <returns></returns>
		public virtual TData GetById(object id) {
			var trait = EntityTrait.For<TData>();
			return Get(ExpressionUtils.MakeMemberEqualiventExpression<TData>(
				trait.PrimaryKey,
				id.ConvertOrDefault(trait.PrimaryKeyType, null)));
		}

		/// <summary>
		/// 获取指定Id的单个对象，找不到或标记已删除时返回null
		/// </summary>
		/// <param name="id">数据Id</param>
		/// <returns></returns>
		public virtual TData GetByIdWhereNotDeleted(object id) {
			// 判断类型是否可回收
			var entityTrait = EntityTrait.For<TData>();
			var recyclableTrait = RecyclableTrait.For<TData>();
			if (!recyclableTrait.IsRecyclable) {
				throw new ArgumentException(string.Format("entity {0} not recyclable", typeof(TData).Name));
			}
			// 构建表达式 (data => data.Id == id && !data.Deleted)
			var dataParam = Expression.Parameter(typeof(TData), "data");
			var body = Expression.AndAlso(
				Expression.Equal(
					Expression.Property(dataParam, entityTrait.PrimaryKey),
					Expression.Constant(id.ConvertOrDefault(entityTrait.PrimaryKeyType, null))),
				Expression.Not(Expression.Property(dataParam, recyclableTrait.PropertyName)));
			return Get(Expression.Lambda<Func<TData, bool>>(body, dataParam));
		}

		/// <summary>
		/// 获取符合条件的多个对象
		/// </summary>
		/// <param name="predicate">查询条件</param>
		/// <returns></returns>
		public virtual IQueryable<TData> GetMany(Expression<Func<TData, bool>> predicate) {
			return Context.Query<TData>().Where(predicate);
		}

		/// <summary>
		/// 批量删除
		/// 返回删除的数量
		/// 这个函数仅设置Deleted为true，不会从数据库中删除
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="ids">Id列表</param>
		public virtual long BatchDelete(IEnumerable<object> ids) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			var propertyName = RecyclableTrait.For<TData>().PropertyName;
			var setter = ReflectionUtils.MakeSetter<TData, bool>(propertyName);
			var trait = EntityTrait.For<TData>();
			long count = 0;
			foreach (var id in ids) {
				var data = Context.Get(
					ExpressionUtils.MakeMemberEqualiventExpression<TData>(
						trait.PrimaryKey,
						id.ConvertOrDefault(trait.PrimaryKeyType, null)));
				if (data != null) {
					Context.Save(ref data, d => setter(d, true));
					++count;
				}
			}
			return count;
		}

		/// <summary>
		/// 批量恢复
		/// 返回恢复的数量
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="ids">Id列表</param>
		/// <returns></returns>
		public virtual long BatchRecover(IEnumerable<object> ids) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			var propertyName = RecyclableTrait.For<TData>().PropertyName;
			var setter = ReflectionUtils.MakeSetter<TData, bool>(propertyName);
			var trait = EntityTrait.For<TData>();
			long count = 0;
			foreach (var id in ids) {
				var data = Context.Get(
					ExpressionUtils.MakeMemberEqualiventExpression<TData>(
						trait.PrimaryKey,
						id.ConvertOrDefault(trait.PrimaryKeyType, null)));
				if (data != null) {
					Context.Save(ref data, d => setter(d, false));
					++count;
				}
			}
			return count;
		}

		/// <summary>
		/// 批量永久删除
		/// 返回删除的数量
		/// 这个函数会把数据从数据库中删除
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="ids">Id列表</param>
		/// <returns></returns>
		public virtual long BatchDeleteForever(IEnumerable<object> ids) {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			var trait = EntityTrait.For<TData>();
			long count = 0;
			foreach (var id in ids) {
				count += Context.DeleteWhere(
					ExpressionUtils.MakeMemberEqualiventExpression<TData>(
						trait.PrimaryKey,
						id.ConvertOrDefault(trait.PrimaryKeyType, null)));
			}
			return count;
		}
	}
}
