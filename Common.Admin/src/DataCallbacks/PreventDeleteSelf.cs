using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Managers;

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
				throw new HttpException(400, new T("Delete yourself is not allowed"));
			}
		}

		public void BeforeDelete(DatabaseContext context, User data) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			if (session.ReleatedId == data.Id) {
				throw new HttpException(400, new T("Delete yourself is not allowed"));
			}
		}
	}
}
