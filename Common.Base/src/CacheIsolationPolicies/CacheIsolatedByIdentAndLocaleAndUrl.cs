using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.CacheIsolationPolicies {
	/// <summary>
	/// 按当前登录用户、语言、时区和Url隔离缓存
	/// </summary>
	[ExportMany(ContractKey = "IdentAndLocaleAndUrl")]
	public class CacheIsolatedByIdentAndLocaleAndUrl : ICacheIsolationPolicy {
		/// <summary>
		/// 获取隔离键
		/// </summary>
		/// <returns></returns>
		public object GetIsolationKey() {
			var identAndLocaleKey = new CacheIsolatedByIdentAndLocale().GetIsolationKey();
			var pathAndQuery = HttpContextUtils.CurrentContext.Request.Url.PathAndQuery;
			return new KeyValuePair<object, string>(identAndLocaleKey, pathAndQuery);
		}
	}
}
