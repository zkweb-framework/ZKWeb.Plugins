using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 菜单项
	/// 这个类型用于支持List[MenuItem]的扩展函数
	/// </summary>
	public class MenuItem : HtmlItem {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="value"></param>
		public MenuItem(string value) : base(value) { }
	}
}
