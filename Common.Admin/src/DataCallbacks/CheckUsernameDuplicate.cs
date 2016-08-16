using System;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.DataCallbacks {
	/// <summary>
	/// 注册用户时检测用户名是否重复
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CheckUsernameDuplicate : IEntityOperationHandler<User> {
		public void BeforeSave(IDatabaseContext context, User data) {
			if (data.Id <= 0 && context.Count<User>(u => u.Username == data.Username) > 0) {
				throw new BadRequestException(new T("Username is already taken, please choose other username"));
			}
		}

		public void AfterSave(IDatabaseContext context, User data) { }

		public void BeforeDelete(IDatabaseContext context, User entity) { }

		public void AfterDelete(IDatabaseContext context, User entity) { }
	}
}
