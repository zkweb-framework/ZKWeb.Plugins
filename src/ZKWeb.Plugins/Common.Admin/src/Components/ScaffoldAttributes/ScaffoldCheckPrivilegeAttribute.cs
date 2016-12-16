using System;
using System.FastReflection;
using System.Reflection;
using ZKWeb.Plugins.Common.Admin.src.Components.ActionFilters;
using ZKWeb.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Components.ScaffoldAttributes {
	/// <summary>
	/// ScaffoldController使用的CheckPrivilege属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class ScaffoldCheckPrivilegeAttribute : ScaffoldActionFilterAttribute {
		/// <summary>
		/// 保存要求的用户类型的成员
		/// 类型应该是Type
		/// </summary>
		public string RequiredUserTypeProperty { get; set; }
		/// <summary>
		/// 保存要求的权限列表的成员
		/// 类型应该是string[]
		/// </summary>
		public string RequiredPrivilegesProperty { get; set; }
		/// <summary>
		/// Http方法
		/// 如果等于空则所有Http方法都会检查
		/// </summary>
		public string HttpMethod { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ScaffoldCheckPrivilegeAttribute(
			string requiredUserTypeProperty,
			string requiredPrivilegesProperty) {
			RequiredUserTypeProperty = requiredUserTypeProperty;
			RequiredPrivilegesProperty = requiredPrivilegesProperty;
			HttpMethod = null;
		}

		/// <summary>
		/// 获取实际的Action过滤器
		/// </summary>
		public override IActionFilter GetFilter(IController controller) {
			var controllerType = controller.GetType();
			var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var userTypeProperty = controllerType.FastGetProperty(RequiredUserTypeProperty, bindingFlags);
			var privilegesProperty = controllerType.FastGetProperty(RequiredPrivilegesProperty, bindingFlags);
			var userType = (Type)userTypeProperty.FastGetValue(controller);
			var privileges = (string[])privilegesProperty.FastGetValue(controller);
			return new CheckPrivilegeAttribute(userType, privileges) { HttpMethod = HttpMethod };
		}
	}
}
