using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Components.EntityOperationHandlers {
	/// <summary>
	/// 防止删除自身
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PreventDeleteSelf : IEntityOperationHandler<User> {
		public void AfterDelete(IDatabaseContext context, User data) { }
		public void BeforeSave(IDatabaseContext context, User data) { }

		public void AfterSave(IDatabaseContext context, User data) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			if (data.Deleted && session.ReleatedId == data.Id) {
				throw new BadRequestException(new T("Delete yourself is not allowed"));
			}
		}

		public void BeforeDelete(IDatabaseContext context, User data) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			if (session.ReleatedId == data.Id) {
				throw new BadRequestException(new T("Delete yourself is not allowed"));
			}
		}
	}
}
