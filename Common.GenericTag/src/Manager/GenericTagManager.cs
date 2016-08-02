using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericTag.src.Model;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericTag.src.Manager {
	/// <summary>
	/// 通用标签管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericTagManager : ICacheCleaner {
		/// <summary>
		/// 通用标签列表的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan TagCacheTime { get; set; }
		/// <summary>
		/// 通用标签的缓存，{ Id: 标签 }
		/// </summary>
		protected MemoryCache<long, Database.GenericTag> TagCache { get; set; }
		/// <summary>
		/// 通用标签列表的缓存，{ 类型: 标签列表 }
		/// </summary>
		protected MemoryCache<string, IList<Database.GenericTag>> TagListCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericTagManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			TagCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.TagCacheTime, 15));
			TagCache = new MemoryCache<long, Database.GenericTag>();
			TagListCache = new MemoryCache<string, IList<Database.GenericTag>>();
		}

		/// <summary>
		/// 获取指定标签
		/// 不存在或已删除时返回null
		/// </summary>
		/// <param name="tagId">标签Id</param>
		/// <returns></returns>
		public virtual Database.GenericTag GetTag(long tagId) {
			return TagCache.GetOrCreate(tagId, () =>
				UnitOfWork.ReadData<Database.GenericTag, Database.GenericTag>(r => {
					return r.GetByIdWhereNotDeleted(tagId);
				}), TagCacheTime);
		}

		/// <summary>
		/// 获取指定类型的标签列表
		/// 按显示顺序返回，不包括已删除的标签
		/// </summary>
		/// <param name="type">标签类型</param>
		/// <returns></returns>
		public virtual IList<Database.GenericTag> GetTags(string type) {
			return TagListCache.GetOrCreate(type, () =>
				UnitOfWork.ReadData<Database.GenericTag, IList<Database.GenericTag>>(r => {
					return r.GetMany(c => c.Type == type && !c.Deleted)
						.OrderBy(t => t.DisplayOrder).ToList();
				}), TagCacheTime);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			TagListCache.Clear();
		}
	}
}
