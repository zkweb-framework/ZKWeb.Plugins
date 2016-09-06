using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Finance.Payment.src.Components.ExtraConfigKeys;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Domain.Services {
	/// <summary>
	/// 支付接口管理器
	/// </summary>
	[ExportMany]
	public class PaymentApiManager : DomainServiceBase<PaymentApi, Guid> {
		/// <summary>
		/// 支付接口的缓存时间
		/// 默认是3秒，可通过网站配置指定
		/// </summary>
		protected TimeSpan PaymentApiCacheTime { get; set; }
		/// <summary>
		/// 支付接口的缓存
		/// { 接口Id: 接口, ... }
		/// </summary>
		protected MemoryCache<Guid, PaymentApi> PaymentApiCache { get; set; }
		/// <summary>
		/// 支付接口列表的缓存
		/// { (所有人Id, 交易类型): 接口, ... }
		/// </summary>
		protected MemoryCache<Pair<Guid, string>, IList<PaymentApi>> PaymentApisCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PaymentApiManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			var extra = configManager.WebsiteConfig.Extra;
			PaymentApiCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				PaymentExtraConfigKeys.PaymentApiCacheTime, 3));
			PaymentApiCache = new MemoryCache<Guid, PaymentApi>();
			PaymentApisCache = new MemoryCache<Pair<Guid, string>, IList<PaymentApi>>();
		}

		/// <summary>
		/// 获取支付接口
		/// 不存在或已删除时返回null
		/// 结果会按接口Id缓存一定时间
		/// </summary>
		/// <param name="apiId">支付接口的Id</param>
		/// <returns></returns>
		public virtual PaymentApi GetWithCache(Guid apiId) {
			return PaymentApiCache.GetOrCreate(
				apiId, () => Get(apiId), PaymentApiCacheTime);
		}

		/// <summary>
		/// 获取支付接口列表
		/// 结果会按所有人Id和交易类型缓存一定时间
		/// </summary>
		/// <param name="ownerId">所有人Id，传入null时获取后台添加的支付接口列表</param>
		/// <param name="transactionType">交易类型</param>
		/// <returns></returns>
		public virtual IList<PaymentApi> GetManyWithCache(
			Guid? ownerId, string transactionType) {
			var key = Pair.Create(ownerId ?? Guid.Empty, transactionType);
			return PaymentApisCache.GetOrCreate(key, () =>
				GetMany(query => query
					.Where(a => a.Owner.Id == ownerId)
					.OrderBy(a => a.DisplayOrder).ToList()
					.Where(a => a.SupportTransactionTypes.Contains(transactionType))
					.ToList()), PaymentApiCacheTime);
		}

		/// <summary>
		/// 计算支付手续费
		/// </summary>
		/// <param name="apiId">支付接口Id</param>
		/// <param name="amount">支付金额</param>
		/// <returns></returns>
		public virtual decimal CalculatePaymentFee(Guid apiId, decimal amount) {
			var paymentFee = 0M;
			var api = GetWithCache(apiId);
			if (api == null) {
				throw new BadRequestException(new T("Selected payment api does not exist"));
			}
			var handlers = api.GetHandlers();
			handlers.ForEach(h => h.CalculatePaymentFee(api, amount, ref paymentFee));
			return paymentFee;
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			PaymentApiCache.Clear();
			PaymentApisCache.Clear();
		}
	}
}
