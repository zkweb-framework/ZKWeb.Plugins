using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.DataCallbacks {
	/// <summary>
	/// 防止删除自身
	/// </summary>
	[ExportMany]
	public class PreventDeleteSelf : IDataDeleteCallback<User>, IDataSaveCallback<User> {
		public void AfterDelete(DatabaseContext context, User data) { }
		public void BeforeSave(DatabaseContext context, User data) { }

		public void AfterSave(DatabaseContext context, User data) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			if (data.Deleted && session.ReleatedId == data.Id) {
				throw new BadRequestException(new T("Delete yourself is not allowed"));
			}
		}

		public void BeforeDelete(DatabaseContext context, User data) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			if (session.ReleatedId == data.Id) {
				throw new BadRequestException(new T("Delete yourself is not allowed"));
			}
		}
	}
}
