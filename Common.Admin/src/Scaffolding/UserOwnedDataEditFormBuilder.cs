using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.TypeTraits;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Admin.src.Scaffolding {
	/// <summary>
	/// 用于编辑指定数据的表单
	/// 绑定和提交表单时会进行数据所有权的检查，提交时注意调用AssignOwnedUser
	/// </summary>
	/// <typeparam name="TData">编辑的数据类型</typeparam>
	/// <typeparam name="TForm">继承类自身的类型</typeparam>
	public abstract class UserOwnedDataEditFormBuilder<TData, TForm> :
		DataEditFormBuilder<TData, TForm> where TData : class, new() {
		/// <summary>
		/// 初始化
		/// </summary>
		public UserOwnedDataEditFormBuilder(FormBuilder form = null) : base(form) { }

		/// <summary>
		/// 获取传入参数请求的数据Id
		/// 并检查当前登录用户是否有数据的所有权
		/// </summary>
		/// <returns></returns>
		protected override string GetRequestId() {
			// id等于空时表示新建，不需要检查
			var id = base.GetRequestId();
			if (!string.IsNullOrEmpty(id)) {
				var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
				privilegeManager.CheckOwnership<TData>(new[] { id });
			}
			return id;
		}

		/// <summary>
		/// 设置数据的所属用户到当前登录用户
		/// </summary>
		/// <param name="context">数据库上下文</param>
		/// <param name="data">数据</param>
		protected virtual void AssignOwnedUser(DatabaseContext context, TData data) {
			// 通过指定的数据库上下文获取当前登录用户
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var user = context.Get<User>(u => u.Id == session.ReleatedId);
			if (user == null) {
				throw new NullReferenceException("Get user failed");
			}
			// 设置所属用户到当前登录用户
			var userOwnedTrait = UserOwnedTrait.For<TData>();
			if (!userOwnedTrait.IsUserOwned) {
				throw new ArgumentException(string.Format("type {0} not user owned", typeof(TData).Name));
			}
			ReflectionUtils.MakeSetter<TData, User>(userOwnedTrait.PropertyName)(data, user);
		}
	}
}
