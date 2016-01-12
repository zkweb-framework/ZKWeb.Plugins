using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 下拉框或单选按钮使用的选项
	/// </summary>
	public class ListItem {
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 值
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ListItem() { }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		public ListItem(string name, string value) {
			Name = name;
			Value = value;
		}
	}
}
