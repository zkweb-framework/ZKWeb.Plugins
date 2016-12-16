using System;
using System.FastReflection;
using System.Reflection;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Components.ScaffoldAttributes {
	/// <summary>
	/// ScaffoldController中使用的Action属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class ScaffoldActionAttribute : Attribute {
		/// <summary>
		/// 保存了路径的成员名称
		/// 成员类型应该是string
		/// </summary>
		public string PathProperty { get; set; }
		/// <summary>
		/// Http方法
		/// </summary>
		public string HttpMethod { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="pathProperty">保存了路径的成员名称</param>
		/// <param name="methods">Http方法</param>
		public ScaffoldActionAttribute(string pathProperty, string httpMethod) {
			PathProperty = pathProperty;
			HttpMethod = httpMethod;
		}

		/// <summary>
		/// 获取路径
		/// </summary>
		/// <param name="controller">控制器的实例</param>
		/// <returns></returns>
		public virtual string GetPath(IController controller) {
			var controllerType = controller.GetType();
			var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var pathProperty = controllerType.FastGetProperty(PathProperty, bindingFlags);
			var path = (string)pathProperty.FastGetValue(controller);
			return path;
		}
	}
}
