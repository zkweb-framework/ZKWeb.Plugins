using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Managers;

namespace ZKWeb.Plugins.Common.Base.src.CacheIsolationPolicies {
	/// <summary>
	/// 按当前登录用户隔离缓存
	/// </summary>
	[ExportMany(ContractKey = "Ident")]
	public class CacheIsolateByIdent : ICacheIsolationPolicy {
		/// <summary>
		/// 获取隔离键
		/// </summary>
		/// <returns></returns>
		public object GetIsolationKey() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			return sessionManager.GetSession().ReleatedId;
		}
	}
}
