using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericTag.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.IocContainer;

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
			// 从缓存获取
			var tag = TagCache.GetOrDefault(tagId);
			if (tag != null) {
				return tag;
			}
			// 从数据库获取
			UnitOfWork.ReadData<Database.GenericTag>(r => {
				tag = r.GetByIdWhereNotDeleted(tagId);
				// 保存到缓存
				if (tag != null) {
					TagCache.Put(tagId, tag, TagCacheTime);
				}
			});
			return tag;
		}

		/// <summary>
		/// 获取指定类型的标签列表
		/// 按显示顺序返回，不包括已删除的标签
		/// </summary>
		/// <param name="type">标签类型</param>
		/// <returns></returns>
		public virtual IList<Database.GenericTag> GetTags(string type) {
			// 从缓存获取
			var tags = TagListCache.GetOrDefault(type);
			if (tags != null) {
				return tags;
			}
			// 从数据库获取
			tags = UnitOfWork.ReadData<Database.GenericTag, IList<Database.GenericTag>>(r => {
				return r.GetMany(c => c.Type == type && !c.Deleted)
					.OrderBy(t => t.DisplayOrder).ToList();
			});
			// 保存到缓存并返回
			TagListCache.Put(type, tags, TagCacheTime);
			return tags;
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			TagListCache.Clear();
		}
	}
}
