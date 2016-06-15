using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Region.src.Extensions {
	/// <summary>
	/// 地区节点的扩展函数
	/// </summary>
	public static class RegionNodeException {
		/// <summary>
		/// 获取地区节点的完整名称
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static string GetFullname(this ITreeNode<Model.Region> node) {
			var names = new List<string>() { node.Value.Name };
			names.AddRange(node.GetParents().Where(p => p.Value != null).Select(p => p.Value.Name));
			return string.Join("", names.Reverse<string>());
		}
	}
}
