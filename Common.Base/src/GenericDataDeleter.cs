using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 通用数据删除器
	/// 用于删除，恢复，永久删除对象
	/// 删除器中的函数不会进行安全检查，请在调用前或使用回调检查
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericDataDeleter {
		/// <summary>
		/// 批量删除
		/// 返回删除的数量
		/// 这个函数仅设置Deleted为true，不会从数据库中删除
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="idList">Id列表</param>
		public virtual long BatchDelete<TData>(IList<long> idList)
			where TData : class {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			var setter = ReflectionUtils.MakeSetter<TData, bool>(IsRecyclable.PropertyName);
			long count = 0;
			using (var context = databaseManager.GetContext()) {
				foreach (var id in idList) {
					var data = context.Get(ExpressionUtils.MakeMemberEqualiventExpression<TData>("Id", id));
					if (data != null) {
						context.Save(ref data, d => setter(d, true));
						++count;
					}
				}
				context.SaveChanges();
			}
			return count;
		}

		/// <summary>
		/// 批量恢复
		/// 返回恢复的数量
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="idList">Id列表</param>
		/// <returns></returns>
		public virtual long BatchRecover<TData>(IList<long> idList)
			where TData : class {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			var setter = ReflectionUtils.MakeSetter<TData, bool>(IsRecyclable.PropertyName);
			long count = 0;
			using (var context = databaseManager.GetContext()) {
				foreach (var id in idList) {
					var data = context.Get(ExpressionUtils.MakeMemberEqualiventExpression<TData>("Id", id));
					if (data != null) {
						context.Save(ref data, d => setter(d, false));
						++count;
					}
				}
				context.SaveChanges();
			}
			return count;
		}

		/// <summary>
		/// 批量永久删除
		/// 返回删除的数量
		/// 这个函数会把数据从数据库中删除
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="idList">Id列表</param>
		/// <returns></returns>
		public virtual long BatchDeleteForever<TData>(IList<long> idList)
			where TData : class {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			var setter = ReflectionUtils.MakeSetter<TData, bool>(IsRecyclable.PropertyName);
			long count = 0;
			using (var context = databaseManager.GetContext()) {
				foreach (var id in idList) {
					count += context.DeleteWhere(
						ExpressionUtils.MakeMemberEqualiventExpression<TData>("Id", id));
				}
				context.SaveChanges();
			}
			return count;
		}
	}
}
