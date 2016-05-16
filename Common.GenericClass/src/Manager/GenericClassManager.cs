using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.GenericClass.src.Manager {
	/// <summary>
	/// 通用分类管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericClassManager {
		/// <summary>
		/// 通用分类列表的缓存时间，默认是15秒
		/// </summary>
		public int ClassCacheTime { get; set; }
		/// <summary>
		/// 通用分类列表的缓存，{ 类型: 分类列表 }
		/// </summary>
		protected MemoryCache<string, IList<Database.GenericClass>> ClassCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericClassManager() {
			ClassCacheTime = 15;
			ClassCache = new MemoryCache<string, IList<Database.GenericClass>>();
		}

		/// <summary>
		/// 获取指定类型的分类列表
		/// </summary>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public IList<Database.GenericClass> GetClasses(string type) {
			var classes = ClassCache.GetOrDefault(type);
			if (classes != null) {
				return classes;
			}
			classes = UnitOfWork.ReadData<Database.GenericClass, IList<Database.GenericClass>>(r => {
				return r.GetMany(c => c.Type == type && !c.Deleted)
					.Select(c => new { c, p = c.Parent }).ToList()
					.Select(c => c.c).ToList();
			});
			ClassCache.Put(type, classes, TimeSpan.FromSeconds(ClassCacheTime));
			return classes;
		}
	}
}
