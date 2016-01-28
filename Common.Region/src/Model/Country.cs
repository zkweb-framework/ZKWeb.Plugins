using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
using ZKWeb.Utils.Collections;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Region.src.Model {
	/// <summary>
	/// 国家
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
		/// 地区列表
		/// 在回调中修改后注意需要调用ClearCache清理缓存
		/// </summary>
		public virtual List<Region> Regions { get; set; }
		/// <summary>
		/// 地区树的缓存
		/// </summary>
		protected virtual ITreeNode<Region> RegionsTreeCache { get; set; }
		/// <summary>
		/// 地区Id到地区树节点的缓存
		/// </summary>
		protected virtual IDictionary<long, ITreeNode<Region>> RegionsTreeNodeCache { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Country() {
			Regions = new List<Region>();
			RegionsTreeCache = null;
			RegionsTreeNodeCache = null;
		}

		/// <summary>
		/// 获取地区树
		/// </summary>
		/// <returns></returns>
		public ITreeNode<Region> GetRegionsTree() {
			if (RegionsTreeCache == null) {
				var regionsMapping = Regions.ToDictionary(r => r.Id);
				var tree = TreeUtils.CreateTree(Regions,
					r => r, r => regionsMapping.GetOrDefault(r.ParentId));
				RegionsTreeCache = tree;
			}
			return RegionsTreeCache;
		}

		/// <summary>
		/// 根据地区Id获取地区的树节点，找不到时返回null
		/// </summary>
		/// <param name="regionId">地区Id</param>
		/// <returns></returns>
		public ITreeNode<Region> GetRegionsTreeNode(long regionId) {
			if (RegionsTreeNodeCache == null) {
				var tree = GetRegionsTree();
				var cache = new Dictionary<long, ITreeNode<Region>>();
				foreach (var node in tree.EnumerateAllNodes()) {
					if (node.Value != null) {
						cache[node.Value.Id] = node;
					}
				}
				RegionsTreeNodeCache = cache;
			}
			return RegionsTreeNodeCache.GetOrDefault(regionId);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			RegionsTreeCache = null;
			RegionsTreeNodeCache = null;
		}
	}
}
