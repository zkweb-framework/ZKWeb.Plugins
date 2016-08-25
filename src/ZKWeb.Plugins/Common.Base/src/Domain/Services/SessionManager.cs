using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Services {
	/// <summary>
	/// 会话管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class SessionManager : DomainServiceBase<Session, Guid> {
		/// <summary>
		/// 删除已过期的会话
		/// </summary>
		public virtual long RemoveExpiredSessions() {
			var now = DateTime.UtcNow;
			using (UnitOfWork.Scope()) {
				return Repository.BatchDelete(s => s.Expires < now);
			}
		}
	}
}
