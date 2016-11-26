using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.UserContact.src.Domain.Services {
	/// <summary>
	/// 用户联系信息管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class UserContactManager : DomainServiceBase<Entities.UserContact, Guid> {
		/// <summary>
		/// 获取用户的联系信息，没有时新建返回
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <returns></returns>
		public virtual Entities.UserContact GetContact(Guid userId) {
			return Get(c => c.User.Id == userId) ?? new Entities.UserContact();
		}

		/// <summary>
		/// 批量获取联系信息
		/// </summary>
		/// <param name="userIds">用户Id列表</param>
		/// <returns></returns>
		public virtual IDictionary<Guid, Entities.UserContact> GetContacts(IList<Guid> userIds) {
			return GetMany(c => userIds.Contains(c.User.Id)).ToDictionary(c => c.User.Id);
		}

		/// <summary>
		/// 设置用户的联系信息，没有时新建
		/// </summary>
		/// <param name="userId">用户Id</param>
		/// <param name="update">更新联系信息的函数</param>
		public virtual void SetContact(Guid userId, Action<Entities.UserContact> update) {
			var uow = UnitOfWork;
			using (uow.Scope()) {
				uow.Context.BeginTransaction();
				var contact = Get(c => c.User.Id == userId);
				if (contact == null) {
					var userService = Application.Ioc.Resolve<UserManager>();
					contact = new Entities.UserContact() {
						User = userService.Get(userId)
					};
				}
				Save(ref contact, update);
				uow.Context.FinishTransaction();
			}
		}
	}
}
