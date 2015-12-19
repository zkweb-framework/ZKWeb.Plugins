using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表单属性
	/// 用于指定表单名称，可省略
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class FormAttribute : Attribute {
		/// <summary>
		/// 表单名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">表单名称</param>
		public FormAttribute(string name) {
			Name = name;
		}
	}
}
