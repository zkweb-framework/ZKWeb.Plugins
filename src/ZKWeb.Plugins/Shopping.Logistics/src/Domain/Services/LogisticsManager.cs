using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Currency.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Region.src.Components.GenericConfigs;
using ZKWeb.Plugins.Common.Region.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Logistics.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Shopping.Logistics.src.Domain.Structs;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Domain.Services {
	/// <summary>
	/// 物流管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class LogisticsManager :
		DomainServiceBase<Entities.Logistics, Guid>, ICacheCleaner {
		/// <summary>
		/// 物流的缓存时间
		/// 默认是3秒，可通过网站配置指定
		/// </summary>
		protected TimeSpan LogisticsCacheTime { get; set; }
		/// <summary>
		/// 物流的缓存
		/// { 物流Id: 物流, ... }
		/// </summary>
		protected IKeyValueCache<Guid, Entities.Logistics> LogisticsCache { get; set; }
		/// <summary>
		/// 物流列表的缓存
		/// { 所属Id: 物流列表, ... }
		/// </summary>
		protected IKeyValueCache<Guid, IList<Entities.Logistics>> LogisticsListCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public LogisticsManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			LogisticsCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				LogisticsExtraConfigKeys.LogisticsCacheTime, 3));
			LogisticsCache = cacheFactory.CreateCache<Guid, Entities.Logistics>();
			LogisticsListCache = cacheFactory.CreateCache<Guid, IList<Entities.Logistics>>();
		}

		/// <summary>
		/// 获取物流
		/// 不存在或已删除时返回null
		/// 结果会按物流Id缓存一定时间
		/// </summary>
		/// <param name="logisticsId">物流Id</param>
		/// <returns></returns>
		public virtual Entities.Logistics GetWithCache(Guid logisticsId) {
			return LogisticsCache.GetOrCreate(
				logisticsId, () => Get(logisticsId), LogisticsCacheTime);
		}

		/// <summary>
		/// 获取物流列表
		/// 结果会按所有人Id缓存一定时间
		/// </summary>
		/// <param name="ownerId">所有人Id，传入null时获取后台添加的物流列表</param>
		/// <returns></returns>
		public virtual IList<Entities.Logistics> GetManyWithCache(Guid? ownerId) {
			return LogisticsListCache.GetOrCreate(ownerId ?? Guid.Empty, () => GetMany(query => {
				return query.Where(l => l.Owner.Id == ownerId).OrderBy(l => l.DisplayOrder).ToList();
			}), LogisticsCacheTime);
		}

		/// <summary>
		/// 计算运费
		/// 返回 ((运费, 货币), 错误信息)
		/// </summary>
		/// <param name="logisticsId">物流Id</param>
		/// <param name="country">国家，等于null时使用默认国家</param>
		/// <param name="regionId">地区Id</param>
		/// <param name="weight">重量，单位是克</param>
		/// <returns></returns>
		public virtual Pair<Pair<decimal, string>, string> CalculateCost(
			Guid logisticsId, string country, long? regionId, decimal weight) {
			// 获取物流
			var logistics = GetWithCache(logisticsId);
			if (logistics == null) {
				return Pair.Create(Pair.Create(0M, ""),
					new T("Selected logistics does not exist").ToString());
			}
			// 匹配运费规则
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var regionSettings = configManager.GetData<RegionSettings>();
			var currencySettings = configManager.GetData<CurrencySettings>();
			var regionManager = Application.Ioc.Resolve<RegionManager>();
			country = string.IsNullOrEmpty(country) ? regionSettings.DefaultCountry : country;
			var matchedRule = logistics.PriceRules.FirstOrDefault(rule => {
				if (rule.Country != null && rule.Country != country) {
					// 国家不匹配
					return false;
				} else if (rule.RegionId != null) {
					if (regionId == null) {
						// 规则中指定了地区，但传入参数没有指定地区
						return false;
					}
					var node = regionManager.GetCountry(country).GetRegionsTreeNode(regionId.Value);
					if (node == null) {
						// 传入参数的地区不存在
						return false;
					} else if (node.Value.Id != rule.RegionId &&
						node.GetParents().All(n => n.Value == null || n.Value.Id != rule.RegionId)) {
						// 传入参数的地区不是规则中的地区或者子地区
						return false;
					}
				}
				return true;
			});
			// 检查运费规则是否已禁用
			matchedRule = matchedRule ?? new PriceRule();
			if (matchedRule.Disabled) {
				return Pair.Create(Pair.Create(0M, ""),
					new T("Selected logistics is not available for this region").ToString());
			}
			// 计算运费并返回
			decimal cost = matchedRule.FirstHeavyCost;
			checked {
				if (weight > matchedRule.FirstHeavyUnit) {
					// 计算续重的运费
					cost += (matchedRule.ContinuedHeavyCost *
						(((weight - matchedRule.FirstHeavyUnit - 1) / matchedRule.ContinuedHeavyUnit) + 1));
				}
			}
			var currency = matchedRule.Currency ?? currencySettings.DefaultCurrency;
			return Pair.Create(Pair.Create(cost, currency), "");
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			LogisticsCache.Clear();
			LogisticsListCache.Clear();
		}
	}
}
