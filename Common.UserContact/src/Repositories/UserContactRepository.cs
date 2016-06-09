using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.UserContact.src.Repositories {
	/// <summary>
	/// 联系信息管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserContactRepository : GenericRepository<Database.UserContact> {
		/// <summary>
		/// 获取用户的联系信息，没有时新建返回
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual Database.UserContact GetContact(long userId) {
			return GetById(userId) ?? new Database.UserContact();
		}

		/// <summary>
		/// 批量获取联系信息
		/// </summary>
		/// <param name="userIds">用户Id列表</param>
		/// <returns></returns>
		public virtual Dictionary<long, Database.UserContact> GetContacts(IList<long> userIds) {
			return Context.Query<Database.UserContact>()
				.Where(c => userIds.Contains(c.User.Id))
				.ToDictionary(c => c.User.Id);
		}

		/// <summary>
		/// 设置用户的联系信息，没有时新建
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="update">更新联系信息的函数</param>
		public virtual void SetContact(long userId, Action<Database.UserContact> update) {
			var contact = GetById(userId);
			if (contact == null) {
				contact = new Database.UserContact() { User = Context.Get<User>(u => u.Id == userId) };
			}
			Save(ref contact, update);
		}
	}
}
