using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Region.src.Model {
	/// <summary>
	/// 国家/行政区
	/// 例子（必须使用SingletonReuse，用于支持在其他插件添加地区）
	/// [ExportMany, SingletonReuse] China : Country { }
	/// </summary>
	public abstract class Country : ICacheCleaner {
		/// <summary>
		/// 国家名称
		/// 格式是两位大写英文（ISO 3166）
		/// 储存时请以这个成员为准
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// 地区列表的缓存
		/// </summary>
		protected LazyCache<List<Region>> RegionsCache { get; set; }
		/// <summary>
		/// 地区树的缓存
		/// </summary>
		protected LazyCache<ITreeNode<Region>> RegionsTreeCache { get; set; }
		/// <summary>
		/// 地区Id到地区树节点的缓存
		/// </summary>
		protected LazyCache<Dictionary<long, ITreeNode<Region>>> RegionsTreeNodeCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Country() {
			RegionsCache = LazyCache.Create(() => new List<Region>());
			RegionsTreeCache = LazyCache.Create(() => {
				var regions = RegionsCache.Value;
				var regionsMapping = regions.ToDictionary(r => r.Id);
				return TreeUtils.CreateTree(regions,
					r => r, r => regionsMapping.GetOrDefault(r.ParentId));
			});
			RegionsTreeNodeCache = LazyCache.Create(() => {
				var tree = GetRegionsTree();
				return tree.EnumerateAllNodes()
					.Where(n => n.Value != null).ToDictionary(n => n.Value.Id);
			});
		}

		/// <summary>
		/// 获取地区列表
		/// </summary>
		/// <returns></returns>
		public virtual IList<Region> GetRegions() {
			return RegionsCache.Value;
		}

		/// <summary>
		/// 获取地区树
		/// </summary>
		/// <returns></returns>
		public virtual ITreeNode<Region> GetRegionsTree() {
			return RegionsTreeCache.Value;
		}

		/// <summary>
		/// 根据地区Id获取地区的树节点，找不到时返回null
		/// </summary>
		/// <param name="regionId">地区Id</param>
		/// <returns></returns>
		public virtual ITreeNode<Region> GetRegionsTreeNode(long regionId) {
			return RegionsTreeNodeCache.Value.GetOrDefault(regionId);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			RegionsCache.Reset();
			RegionsTreeCache.Reset();
			RegionsTreeNodeCache.Reset();
		}
	}
}
