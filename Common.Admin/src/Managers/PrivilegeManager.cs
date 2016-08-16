using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Admin.src.TypeTraits;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Base.src.TemplateFilters;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWeb.Web;
using ZKWebStandard.Web;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Admin.src.Managers {
	/// <summary>
	/// 权限管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PrivilegeManager {
		/// <summary>
		/// 获取网站中注册的所有权限，并且去除重复项
		/// </summary>
		/// <returns></returns>
		public virtual List<string> GetPrivileges() {
			var providers = Application.Ioc.ResolveMany<IPrivilegesProvider>();
			var privileges = providers.SelectMany(p => p.GetPrivileges()).Distinct().ToList();
			return privileges;
		}

		/// <summary>
		/// 检查当前登录用户是否指定的用户类型，且是否拥有指定的权限
		/// 如果用户类型不匹配且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		/// <param name="type">指定的用户类型</param>
		/// <param name="privileges">要求的权限列表</param>
		public virtual void Check(UserTypes type, params string[] privileges) {
			Check(new[] { type }, privileges);
		}

		/// <summary>
		/// 检查当前登录用户是否指定的用户类型，且是否拥有指定的权限
		/// 如果用户类型不匹配且当前请求是get则跳转到登陆页面，否则抛出403错误
		/// </summary>
		/// <param name="types">指定的用户类型列表</param>
		/// <param name="privileges">要求的权限列表</param>
		public virtual void Check(UserTypes[] types, params string[] privileges) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var context = HttpManager.CurrentContext;
			if (context != null && context.Request.Method == HttpMethods.GET &&
				(user == null || !types.Contains(user.Type))) {
				// 包含普通用户时跳转到前台登陆页面，否则跳转到后台登陆页面
				context.Response.RedirectByScript(BaseFilters.Url(
					types.Contains(UserTypes.User) ? "/user/login" : "/admin/login"));
				return;
			} else if (types.Contains(user.Type) && HasPrivileges(user, privileges)) {
				// 检查通过
				return;
			} else if (privileges != null && privileges.Length > 0) {
				// 无权限403
				var translator = Application.Ioc.Resolve<PrivilegesTranslator>();
				throw new ForbiddenException(string.Format(
					new T("Action require {0}, and {1} privileges"),
					string.Join(",", types.Select(t => new T(t.GetDescription()))),
					string.Join(",", privileges.Select(p => translator.Translate(p)))));
			} else {
				// 用户类型不符合，或未登录403
				throw new ForbiddenException(string.Format(
					new T("Action require {0}"),
					string.Join(",", types.Select(t => new T(t.GetDescription())))));
			}
		}

		/// <summary>
		/// 判断用户是否拥有指定的权限
		/// </summary>
		/// <param name="user">用户</param>
		/// <param name="privileges">权限列表</param>
		/// <returns></returns>
		public virtual bool HasPrivileges(User user, params string[] privileges) {
			if (user.Type == UserTypes.SuperAdmin) {
				// 超级管理员拥有所有权限
				return true;
			}
			if (privileges != null && privileges.Length > 0) {
				foreach (var privilege in privileges) {
					if (!user.Roles.Any(r => r.Privileges.Contains(privilege))) {
						// 未包含指定的所有权限
						return false;
					}
				}
			}
			// 检查通过
			return true;
		}

		/// <summary>
		/// 检查当前登录用户是否有指定数据的所有权
		/// 如果没有部分或全部数据的所有权，或部分数据不存在，则抛出403错误
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="ids">数据Id列表</param>
		public virtual void CheckOwnership<TData>(IList<object> ids)
			where TData : class, IEntity {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			if (user == null || !HasOwnership<TData>(user.Id, ids)) {
				throw new ForbiddenException(string.Format(
					new T("Action require the ownership of {0}: {1}"),
					new T(typeof(TData).Name), string.Join(", ", ids)));
			}
		}

		/// <summary>
		/// IList[object].Contains的函数信息
		/// </summary>
		protected static readonly Lazy<MethodInfo> IListContainsMethodInfo =
			new Lazy<MethodInfo>(() => typeof(Enumerable).GetMethods()
				.First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
				.MakeGenericMethod(typeof(object)));

		/// <summary>
		/// 判断用户是否有指定数据的所有权
		/// 如果部分数据不存在，会返回false
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="userId">用户Id</param>
		/// <param name="ids">数据Id列表</param>
		/// <returns></returns>
		public virtual bool HasOwnership<TData>(long userId, IList<object> ids)
			where TData : class, IEntity {
			// 检查数据类型是否有所属用户
			var userOwnedTrait = UserOwnedTrait.For<TData>();
			if (!userOwnedTrait.IsUserOwned) {
				throw new ArgumentException(string.Format("type {0} not user owned", typeof(TData).Name));
			}
			// 构建表达式 (data => ids.Contains(data.Id) && data.Owner.Id == userId)
			var entityTrait = EntityTrait.For<TData>();
			var userEntityTrait = EntityTrait.For<User>();
			var dataParam = Expression.Parameter(typeof(TData), "data");
			var body = Expression.AndAlso(
				Expression.Call(
					IListContainsMethodInfo.Value,
					Expression.Constant(ids.Select(id =>
						id.ConvertOrDefault(entityTrait.PrimaryKeyType, null)).ToList()),
					Expression.Convert(
						Expression.Property(dataParam, entityTrait.PrimaryKey), typeof(object))),
				Expression.Equal(
					Expression.Property(
						Expression.Property(dataParam, userOwnedTrait.PropertyName),
						userEntityTrait.PrimaryKey),
					Expression.Constant(userId)));
			var count = UnitOfWork.ReadData<TData, long>(r => {
				return r.Count(Expression.Lambda<Func<TData, bool>>(body, dataParam));
			});
			return count == ids.Count;
		}
	}
}
