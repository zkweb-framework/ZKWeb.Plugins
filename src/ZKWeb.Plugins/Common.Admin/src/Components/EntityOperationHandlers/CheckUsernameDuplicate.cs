using System;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Components.EntityOperationHandlers {
	/// <summary>
	/// 注册用户时检测用户名是否重复
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CheckUsernameDuplicate : IEntityOperationHandler<User> {
		public void BeforeSave(IDatabaseContext context, User data) {
			if (data.Id == Guid.Empty && context.Count<User>(u => u.Username == data.Username) > 0) {
				throw new BadRequestException(new T("Username is already taken, please choose other username"));
			}
		}

		public void AfterSave(IDatabaseContext context, User data) { }

		public void BeforeDelete(IDatabaseContext context, User entity) { }

		public void AfterDelete(IDatabaseContext context, User entity) { }
	}
}
