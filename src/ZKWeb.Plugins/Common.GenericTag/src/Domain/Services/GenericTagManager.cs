using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.GenericTag.src.Components.ExtraConfigKeys;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericTag.src.Domain.Services {
	/// <summary>
	/// 通用标签管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericTagManager :
		DomainServiceBase<Entities.GenericTag, Guid>, ICacheCleaner {
		/// <summary>
		/// 通用标签列表的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan TagCacheTime { get; set; }
		/// <summary>
		/// 通用标签的缓存，{ Id: 标签 }
		/// </summary>
		protected IKeyValueCache<Guid, Entities.GenericTag> TagCache { get; set; }
		/// <summary>
		/// 通用标签列表的缓存，{ 类型: 标签列表 }
		/// </summary>
		protected IKeyValueCache<string, IList<Entities.GenericTag>> TagListCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericTagManager() {
			var configManager = Application.Ioc.Resolve<WebsiteConfigManager>();
			var cacheFactory = Application.Ioc.Resolve<ICacheFactory>();
			var extra = configManager.WebsiteConfig.Extra;
			TagCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				GenericTagExtraConfigKeys.TagCacheTime, 15));
			TagCache = cacheFactory.CreateCache<Guid, Entities.GenericTag>();
			TagListCache = cacheFactory.CreateCache<string, IList<Entities.GenericTag>>();
		}

		/// <summary>
		/// 检查指定的标签列表是否都属于指定的类型
		/// 如果标签不存在会被跳过
		/// 常用于防止越权操作
		/// </summary>
		/// <param name="ids">标签的Id列表</param>
		/// <param name="type">标签类型</param>
		/// <returns></returns>
		public virtual bool IsAllTagsTypeEqualTo(IList<Guid> ids, string type) {
			return Count(t => ids.Contains(t.Id) && t.Type != type) == 0;
		}

		/// <summary>
		/// 获取指定标签，不存在或已删除时返回null
		/// </summary>
		/// <param name="tagId">标签Id</param>
		/// <returns></returns>
		public virtual Entities.GenericTag GetWithCache(Guid tagId) {
			return TagCache.GetOrCreate(tagId, () => Get(tagId), TagCacheTime);
		}

		/// <summary>
		/// 获取指定类型的标签列表
		/// 按显示顺序返回，不包括已删除的标签
		/// </summary>
		/// <param name="type">标签类型</param>
		/// <returns></returns>
		public virtual IList<Entities.GenericTag> GetManyWithCache(string type) {
			return TagListCache.GetOrCreate(type, () => GetMany(query =>
				query.Where(c => c.Type == type)
					.OrderBy(t => t.DisplayOrder).ToList()), TagCacheTime);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			TagListCache.Clear();
		}
	}
}
