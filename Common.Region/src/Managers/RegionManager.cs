using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Config;
using ZKWeb.Plugins.Common.Region.src.Config;
using ZKWeb.Plugins.Common.Region.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Region.src.Managers {
	/// <summary>
	/// 地区管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class RegionManager : ICacheCleaner {
		/// <summary>
		/// 国家/行政区名称到信息的缓存
		/// </summary>
		protected IDictionary<string, Country> CountryCache { get; set; }

		/// <summary>
		/// 获取默认的货币信息，找不到时返回null
		/// </summary>
		/// <returns></returns>
		public virtual Country GetDefaultCountry() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<RegionSettings>();
			return GetCountry(settings.DefaultCountry);
		}

		/// <summary>
		/// 获取国家/行政区信息，找不到时返回null
		/// </summary>
		/// <param name="name">国家/行政区名称，区分大小写</param>
		/// <returns></returns>
		public virtual Country GetCountry(string name) {
			if (CountryCache == null) {
				var cache = Application.Ioc.ResolveMany<Country>().ToDictionary(c => c.Name);
				CountryCache = cache;
			}
			return CountryCache.GetOrDefault(name);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			CountryCache = null;
		}
	}
}
