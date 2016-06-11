using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.TypeTraits;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Admin.src.Extensions {
	/// <summary>
	/// 数据查询对象的扩展函数
	/// </summary>
	public static class IQueryableExtensions {
		/// <summary>
		/// 按回收站状态过滤数据
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="query">查询对象</param>
		/// <param name="request">搜索请求</param>
		public static IQueryable<TData> FilterByRecycleBin<TData>(
			this IQueryable<TData> query, AjaxTableSearchRequest request) {
			var deleted = request.Conditions.GetOrDefault<bool>("Deleted");
			var propertyName = RecyclableTrait.For<TData>().PropertyName;
			return query.Where(
				ExpressionUtils.MakeMemberEqualiventExpression<TData>(propertyName, deleted));
		}

		/// <summary>
		/// 按所属用户等于指定用户过滤数据
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="query">查询对象</param>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public static IQueryable<TData> FilterByOwnedUser<TData>(
			this IQueryable<TData> query, long userId) {
			// 构建表达式 (data => data.Owner.Id == userId)
			var userOwnedTrait = UserOwnedTrait.For<TData>();
			var userEntityTrait = EntityTrait.For<User>();
			var dataParam = Expression.Parameter(typeof(TData), "data");
			var body = Expression.Equal(
				Expression.Property(
					Expression.Property(dataParam, userOwnedTrait.PropertyName),
					userEntityTrait.PrimaryKey),
				Expression.Constant(userId));
			return query.Where(Expression.Lambda<Func<TData, bool>>(body, dataParam));
		}

		/// <summary>
		/// 按所属用户等于当前登录用户过滤数据
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="query">查询对象</param>
		/// <returns></returns>
		public static IQueryable<TData> FilterByOwnedUser<TData>(
			this IQueryable<TData> query) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user == null) {
				return query.Take(0);
			}
			return query.FilterByOwnedUser(user.Id);
		}
	}
}
