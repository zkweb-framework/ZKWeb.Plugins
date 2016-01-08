using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 菜单项的Html
	/// 这个类型用于支持List[MenuItem]的扩展函数
	/// </summary>
	public class MenuItem : HtmlString {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="value"></param>
		public MenuItem(string value) : base(value) { }
	}
}
