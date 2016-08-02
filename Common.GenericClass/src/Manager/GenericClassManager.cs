using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericClass.src.Model;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Cache;

namespace ZKWeb.Plugins.Common.GenericClass.src.Manager {
	/// <summary>
	/// 通用分类管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericClassManager : ICacheCleaner {
		/// <summary>
		/// 通用分类列表的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ClassCacheTime { get; set; }
		/// <summary>
		/// 通用分类的缓存，{ Id: 分类 }
		/// </summary>
		protected MemoryCache<long, Database.GenericClass> ClassCache { get; set; }
		/// <summary>
		/// 通用分类列表的缓存，{ 类型: 分类列表 }
		/// </summary>
		protected MemoryCache<string, IList<Database.GenericClass>> ClassListCache { get; set; }
		/// <summary>
		/// 通用分类树的缓存，{ 类型: 分类树 }
		/// </summary>
		protected MemoryCache<string, ITreeNode<Database.GenericClass>> ClassTreeCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericClassManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			ClassCacheTime = TimeSpan.FromSeconds(
				configManager.WebsiteConfig.Extra.GetOrDefault(ExtraConfigKeys.ClassCacheTime, 15));
			ClassCache = new MemoryCache<long, Database.GenericClass>();
			ClassListCache = new MemoryCache<string, IList<Database.GenericClass>>();
			ClassTreeCache = new MemoryCache<string, ITreeNode<Database.GenericClass>>();
		}

		/// <summary>
		/// 获取指定分类
		/// 不存在或已删除时返回null
		/// </summary>
		/// <param name="classId">分类Id</param>
		/// <returns></returns>
		public virtual Database.GenericClass GetClass(long classId) {
			return ClassCache.GetOrCreate(classId, () =>
				UnitOfWork.ReadData<Database.GenericClass, Database.GenericClass>(r => {
					return r.GetByIdWhereNotDeleted(classId);
				}), ClassCacheTime);
		}

		/// <summary>
		/// 获取指定类型的分类列表
		/// 按显示顺序返回，不包括已删除的分类
		/// </summary>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public virtual IList<Database.GenericClass> GetClasses(string type) {
			return ClassListCache.GetOrCreate(type, () =>
				UnitOfWork.ReadData<Database.GenericClass, IList<Database.GenericClass>>(r => {
					return r.GetMany(c => c.Type == type && !c.Deleted)
						.OrderBy(c => c.DisplayOrder)
						.Select(c => new { c, p = c.Parent }).ToList()
						.Select(c => c.c).ToList();
				}), ClassCacheTime);
		}

		/// <summary>
		/// 获取指定类型的分类树
		/// 不包括已删除的分类
		/// </summary>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public virtual ITreeNode<Database.GenericClass> GetClassTree(string type) {
			return ClassTreeCache.GetOrCreate(type, () => {
				var classes = GetClasses(type);
				var classMap = classes.ToDictionary(c => c.Id);
				return TreeUtils.CreateTree(classes,
					c => c, c => c.Parent == null ? null : classMap.GetOrDefault(c.Parent.Id));
			}, ClassCacheTime);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public virtual void ClearCache() {
			ClassCache.Clear();
			ClassListCache.Clear();
			ClassTreeCache.Clear();
		}
	}
}
