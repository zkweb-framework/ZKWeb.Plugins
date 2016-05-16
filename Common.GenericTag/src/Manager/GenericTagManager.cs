using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Utils.Collections;

namespace ZKWeb.Plugins.Common.GenericTag.src.Manager {
	/// <summary>
	/// 通用标签管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericTagManager {
		/// <summary>
		/// 通用标签列表的缓存时间，默认是15秒
		/// </summary>
		public int TagCacheTime { get; set; }
		/// <summary>
		/// 通用标签列表的缓存，{ 类型: 标签列表 }
		/// </summary>
		private MemoryCache<string, IList<Database.GenericTag>> TagCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericTagManager() {
			TagCacheTime = 15;
			TagCache = new MemoryCache<string, IList<Database.GenericTag>>();
		}

		/// <summary>
		/// 获取指定类型的标签列表
		/// </summary>
		/// <param name="type">标签类型</param>
		/// <returns></returns>
		public IList<Database.GenericTag> GetTags(string type) {
			var tags = TagCache.GetOrDefault(type);
			if (tags != null) {
				return tags;
			}
			tags = UnitOfWork.ReadData<Database.GenericTag, IList<Database.GenericTag>>(r => {
				return r.GetMany(c => c.Type == type && !c.Deleted).ToList();
			});
			TagCache.Put(type, tags, TimeSpan.FromSeconds(TagCacheTime));
			return tags;
		}
	}
}
