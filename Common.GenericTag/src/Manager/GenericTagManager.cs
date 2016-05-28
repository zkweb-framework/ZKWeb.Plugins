using DryIoc;
using DryIocAttributes;
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
		/// 通用标签列表的缓存，{ 类型: 标签列表 }
		/// </summary>
		private MemoryCache<string, IList<Database.GenericTag>> TagCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericTagManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			TagCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.TagCacheTime, 15));
			TagCache = new MemoryCache<string, IList<Database.GenericTag>>();
		}

		/// <summary>
		/// 获取指定类型的标签列表，按显示顺序返回
		/// </summary>
		/// <param name="type">标签类型</param>
		/// <returns></returns>
		public virtual IList<Database.GenericTag> GetTags(string type) {
			var tags = TagCache.GetOrDefault(type);
			if (tags != null) {
				return tags;
			}
			tags = UnitOfWork.ReadData<Database.GenericTag, IList<Database.GenericTag>>(r => {
				return r.GetMany(c => c.Type == type && !c.Deleted)
					.OrderBy(t => t.DisplayOrder).ToList();
			});
			TagCache.Put(type, tags, TagCacheTime);
			return tags;
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			TagCache.Clear();
		}
	}
}
