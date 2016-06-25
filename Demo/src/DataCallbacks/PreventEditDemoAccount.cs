using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Demo.src.DataCallbacks {
	/// <summary>
	/// 防止编辑和删除演示用的账号
	/// </summary>
	[ExportMany]
	public class PreventDeleteSelf : IDataDeleteCallback<User>, IDataSaveCallback<User> {
		public void AfterDelete(DatabaseContext context, User data) { }
		public void AfterSave(DatabaseContext context, User data) { }

		public void BeforeSave(DatabaseContext context, User data) {
			if (data.Username == "test" || data.Username == "demo") {
				throw new ForbiddenException(new T("Edit or delete demo account is not allowed"));
			}
		}

		public void BeforeDelete(DatabaseContext context, User data) {
			BeforeSave(context, data);
		}
	}
}
