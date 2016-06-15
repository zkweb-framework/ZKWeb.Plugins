using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Region.src.Model {
	/// <summary>
	/// 地区
	/// 这个对象中的值生成后不应该修改
	/// </summary>
	public class Region {
		/// <summary>
		/// 地区Id
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 上级地区的Id，没有时等于0
		/// </summary>
		public long ParentId { get; set; }
		/// <summary>
		/// 地区名称
		/// </summary>
		public string Name { get; set; }
	}
}
