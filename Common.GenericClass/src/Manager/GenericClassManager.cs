using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericClass.src.Model;
using ZKWeb.Server;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Utils.IocContainer;

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
			// 从缓存获取
			var classObj = ClassCache.GetOrDefault(classId);
			if (classObj != null) {
				return classObj;
			}
			// 从数据库获取
			UnitOfWork.ReadData<Database.GenericClass>(r => {
				classObj = r.GetByIdWhereNotDeleted(classId);
				// 保存到缓存
				if (classObj != null) {
					ClassCache.Put(classId, classObj, ClassCacheTime);
				}
			});
			return classObj;
		}

		/// <summary>
		/// 获取指定类型的分类列表
		/// 按显示顺序返回，不包括已删除的分类
		/// </summary>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public virtual IList<Database.GenericClass> GetClasses(string type) {
			// 从缓存获取
			var classes = ClassListCache.GetOrDefault(type);
			if (classes != null) {
				return classes;
			}
			// 从数据库获取
			classes = UnitOfWork.ReadData<Database.GenericClass, IList<Database.GenericClass>>(r => {
				return r.GetMany(c => c.Type == type && !c.Deleted)
					.OrderBy(c => c.DisplayOrder)
					.Select(c => new { c, p = c.Parent }).ToList()
					.Select(c => c.c).ToList();
			});
			// 保存到缓存并返回
			ClassListCache.Put(type, classes, ClassCacheTime);
			return classes;
		}

		/// <summary>
		/// 获取指定类型的分类树
		/// 不包括已删除的分类
		/// </summary>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public virtual ITreeNode<Database.GenericClass> GetClassTree(string type) {
			// 从缓存获取
			var classTree = ClassTreeCache.GetOrDefault(type);
			if (classTree != null) {
				return classTree;
			}
			// 生成分类树
			var classes = GetClasses(type);
			var classMap = classes.ToDictionary(c => c.Id);
			classTree = TreeUtils.CreateTree(classes,
				c => c, c => c.Parent == null ? null : classMap.GetOrDefault(c.Parent.Id));
			// 保存到缓存并返回
			ClassTreeCache.Put(type, classTree, ClassCacheTime);
			return classTree;
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
