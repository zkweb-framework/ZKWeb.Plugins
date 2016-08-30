using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.GenericClass.src.Components.ExtraConfigKeys;
using ZKWeb.Server;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.GenericClass.src.Domain.Services {
	/// <summary>
	/// 通用分类管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class GenericClassManager : DomainServiceBase<Entities.GenericClass, Guid> {
		/// <summary>
		/// 通用分类列表的缓存时间
		/// 默认是15秒，可通过网站配置指定
		/// </summary>
		public TimeSpan ClassCacheTime { get; set; }
		/// <summary>
		/// 通用分类的缓存，{ Id: 分类 }
		/// </summary>
		protected MemoryCache<Guid, Entities.GenericClass> ClassCache { get; set; }
		/// <summary>
		/// 通用分类列表的缓存，{ 类型: 分类列表 }
		/// </summary>
		protected MemoryCache<string, IList<Entities.GenericClass>> ClassListCache { get; set; }
		/// <summary>
		/// 通用分类树的缓存，{ 类型: 分类树 }
		/// </summary>
		protected MemoryCache<string, ITreeNode<Entities.GenericClass>> ClassTreeCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericClassManager() {
			var configManager = Application.Ioc.Resolve<ConfigManager>();
			var extra = configManager.WebsiteConfig.Extra;
			ClassCacheTime = TimeSpan.FromSeconds(extra.GetOrDefault(
				GenericConfigExtraConfigKeys.ClassCacheTime, 15));
			ClassCache = new MemoryCache<Guid, Entities.GenericClass>();
			ClassListCache = new MemoryCache<string, IList<Entities.GenericClass>>();
			ClassTreeCache = new MemoryCache<string, ITreeNode<Entities.GenericClass>>();
		}

		/// <summary>
		/// 检查指定的分类列表是否都属于指定的类型
		/// 如果分类不存在会被跳过
		/// 常用于防止越权操作
		/// </summary>
		/// <param name="ids">分类的Id列表</param>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public virtual bool IsAllClassesTypeEqualTo(IList<Guid> ids, string type) {
			return Count(t => ids.Contains(t.Id) && t.Type != type) == 0;
		}

		/// <summary>
		/// 获取分类，不存在或已删除时返回null
		/// </summary>
		/// <param name="classId">分类Id</param>
		/// <returns></returns>
		public virtual Entities.GenericClass GetWithCache(Guid classId) {
			return ClassCache.GetOrCreate(classId, () => Get(classId), ClassCacheTime);
		}

		/// <summary>
		/// 获取指定类型的分类列表，按显示顺序返回
		/// </summary>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public virtual IList<Entities.GenericClass> GetManyWithCache(string type) {
			return ClassListCache.GetOrCreate(type, () => GetMany(query =>
				query.Where(c => c.Type == type)
					.OrderBy(c => c.DisplayOrder)
					.Select(c => new { c, p = c.Parent }).ToList()
					.Select(c => c.c).ToList()), ClassCacheTime);
		}

		/// <summary>
		/// 获取指定类型的分类树
		/// </summary>
		/// <param name="type">分类类型</param>
		/// <returns></returns>
		public virtual ITreeNode<Entities.GenericClass> GetTreeWithCache(string type) {
			return ClassTreeCache.GetOrCreate(type, () => {
				var classes = GetManyWithCache(type);
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
