using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Currency.src.Config;
using ZKWeb.Plugins.Common.Region.src.Config;
using ZKWeb.Plugins.Common.Region.src.Managers;
using ZKWeb.Plugins.Shopping.Logistics.src.Model;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Manager {
	/// <summary>
	/// 物流管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class LogisticsManager {
		/// <summary>
		/// 物流的缓存时间
		/// 默认是3秒，可通过网站配置指定
		/// </summary>
		public TimeSpan LogisticsCacheTime { get; set; }
		/// <summary>
		/// 物流的缓存
		/// </summary>
		public MemoryCache<long, Database.Logistics> LogisticsCache { get; set; }
		/// <summary>
		/// 物流列表的缓存
		/// </summary>
		public MemoryCache<long, IList<Database.Logistics>> LogisticsListCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public LogisticsManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			LogisticsCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.LogisticsCacheTime, 3));
			LogisticsCache = new MemoryCache<long, Database.Logistics>();
			LogisticsListCache = new MemoryCache<long, IList<Database.Logistics>>();
		}

		/// <summary>
		/// 获取物流
		/// 不存在或已删除时返回null
		/// 结果会按物流Id缓存一定时间
		/// </summary>
		/// <param name="logisticsId">物流Id</param>
		/// <returns></returns>
		public virtual Database.Logistics GetLogistics(long logisticsId) {
			// 从缓存获取
			var logistics = LogisticsCache.GetOrDefault(logisticsId);
			if (logistics != null) {
				return logistics;
			}
			// 从数据库获取
			UnitOfWork.ReadData<Database.Logistics>(r => {
				logistics = r.GetByIdWhereNotDeleted(logisticsId);
				// 保存到缓存
				if (logistics != null) {
					LogisticsCache.Put(logisticsId, logistics, LogisticsCacheTime);
				}
			});
			return logistics;
		}

		/// <summary>
		/// 获取物流列表
		/// 结果会按所有人Id缓存一定时间
		/// </summary>
		/// <param name="ownerId">所有人Id，传入null时获取没有所有人的物流列表</param>
		/// <returns></returns>
		public virtual IList<Database.Logistics> GetLogisticsList(long? ownerId) {
			// 从缓存获取
			var logisticsList = LogisticsListCache.GetOrDefault(ownerId ?? 0);
			if (logisticsList != null) {
				return logisticsList;
			}
			// 从数据库获取
			logisticsList = UnitOfWork.ReadData<Database.Logistics, IList<Database.Logistics>>(r => {
				return r.GetMany(l => !l.Deleted && l.Owner.Id == ownerId)
					.OrderBy(l => l.DisplayOrder).ToList();
			});
			// 保存到缓存并返回
			LogisticsListCache.Put(ownerId ?? 0, logisticsList, LogisticsCacheTime);
			return logisticsList;
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
			long logisticsId, string country, long? regionId, decimal weight) {
			// 获取物流
			var logistics = GetLogistics(logisticsId);
			if (logistics == null) {
				return Pair.Create(Pair.Create(0M, ""),
					new T("Selected logistics does not exist").ToString());
			}
			// 匹配运费规则
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var regionSettings = Application.Ioc.Resolve<RegionSettings>();
			var currencySettings = Application.Ioc.Resolve<CurrencySettings>();
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
	}
}
