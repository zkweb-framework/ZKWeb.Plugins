using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Config;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Currency.src.Managers {
	/// <summary>
	/// 货币管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class CurrencyManager : ICacheCleaner {
		/// <summary>
		/// 货币类型到货币信息的缓存
		/// </summary>
		protected LazyCache<Dictionary<string, ICurrency>> CurrencyCache = LazyCache.Create(() => {
			return Application.Ioc.ResolveMany<ICurrency>().ToDictionary(c => c.Type);
		});

		/// <summary>
		/// 获取默认的货币信息，找不到时返回null
		/// </summary>
		/// <returns></returns>
		public virtual ICurrency GetDefaultCurrency() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<CurrencySettings>();
			return GetCurrency(settings.DefaultCurrency);
		}

		/// <summary>
		/// 获取货币信息，找不到时返回null
		/// </summary>
		/// <param name="type">货币类型，区分大小写</param>
		/// <returns></returns>
		public virtual ICurrency GetCurrency(string type) {
			return CurrencyCache.Value.GetOrDefault(type);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			CurrencyCache.Reset();
		}
	}
}
