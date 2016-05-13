using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.Collections {
	/// <summary>
	/// 根据当前登录的用户和语言时区区分的缓存类
	/// </summary>
	/// <typeparam name="TKey">键类型</typeparam>
	/// <typeparam name="TValue">值类型</typeparam>
	public class MemoryCacheByIdentityAndLocale<TKey, TValue> :
		MemoryCache<Tuple<TKey, long, string, TimeZoneInfo>, TValue> {
		/// <summary>
		/// 生成实际使用的键
		/// </summary>
		public Tuple<TKey, long, string, TimeZoneInfo> GenerateKey(TKey key) {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var releatedId = sessionManager.GetSession().ReleatedId;
			var language = Thread.CurrentThread.CurrentCulture.Name;
			var timezone = HttpContextUtils.GetData<TimeZoneInfo>(LocaleUtils.TimeZoneKey);
			return Tuple.Create(key, releatedId, language, timezone);
		}

		/// <summary>
		/// 设置缓存数据
		/// </summary>
		public void Put(TKey key, TValue value, TimeSpan keepTime) {
			Put(GenerateKey(key), value, keepTime);
		}

		/// <summary>
		/// 获取缓存数据
		/// 没有或已过期时返回默认值
		/// </summary>
		public TValue GetOrDefault(TKey key, TValue defaultValue = default(TValue)) {
			return GetOrDefault(GenerateKey(key), defaultValue);
		}

		/// <summary>
		/// 删除缓存数据
		/// </summary>
		public void Remove(TKey key) {
			Remove(GenerateKey(key));
		}
	}
}
