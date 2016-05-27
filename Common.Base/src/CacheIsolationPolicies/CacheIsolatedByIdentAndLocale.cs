using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.CacheIsolationPolicies {
	/// <summary>
	/// 按当前登录用户、语言和时区隔离缓存
	/// </summary>
	[ExportMany(ContractKey = "IdentAndLocale")]
	public class CacheIsolatedByIdentAndLocale : ICacheIsolationPolicy {
		/// <summary>
		/// 获取隔离键
		/// </summary>
		/// <returns></returns>
		public object GetIsolationKey() {
			var identKey = new CacheIsolatedByIdent().GetIsolationKey();
			var localeKey = new CacheIsolatedByLocale().GetIsolationKey();
			return new KeyValuePair<object, object>(identKey, localeKey);
		}
	}
}
