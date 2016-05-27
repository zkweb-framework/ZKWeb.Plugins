using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.CacheIsolationPolicies {
	/// <summary>
	/// 按当前语言和时区隔离缓存
	/// </summary>
	[ExportMany(ContractKey = "Locale")]
	public class CacheIsolatedByLocale : ICacheIsolationPolicy {
		/// <summary>
		/// 获取隔离键
		/// </summary>
		/// <returns></returns>
		public object GetIsolationKey() {
			var language = Thread.CurrentThread.CurrentCulture.Name;
			var timezone = HttpContextUtils.GetData<TimeZoneInfo>(LocaleUtils.TimeZoneKey);
			return new KeyValuePair<string, TimeZoneInfo>(language, timezone);
		}
	}
}
