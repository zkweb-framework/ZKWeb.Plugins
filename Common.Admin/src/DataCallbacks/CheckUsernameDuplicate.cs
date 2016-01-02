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

namespace ZKWeb.Plugins.Common.Admin.src.DataCallbacks {
	/// <summary>
	/// 注册用户时检测用户名是否重复
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CheckUsernameDuplicate : IDataSaveCallback<User> {
		public void BeforeSave(DatabaseContext context, User data) {
			if (data.Id <= 0 && context.Count<User>(u => u.Username == data.Username) > 0) {
				throw new HttpException(400, new T("Username is already taken, please choose other username"));
			}
		}

		public void AfterSave(DatabaseContext context, User data) { }
	}
}
