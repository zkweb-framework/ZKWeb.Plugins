using System;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Components.ScaffoldAttributes {
	/// <summary>
	/// ScaffoldController使用的Action过滤器属性的基类
	/// </summary>
	public abstract class ScaffoldActionFilterAttribute : Attribute {
		/// <summary>
		/// 获取实际的Action过滤器
		/// </summary>
		/// <param name="controller">控制器的实例</param>
		/// <returns></returns>
		public abstract IActionFilter GetFilter(IController controller);
	}
}
