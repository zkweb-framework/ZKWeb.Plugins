using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.MenuPageBase.src.Model {
	/// <summary>
	/// 菜单分组和菜单项的提供器接口
	/// 这个接口需要再次继承，请勿直接使用
	/// <example>
	/// public interface IMenuProviderForUserPanel : IMenuProvider { }
	/// [ExportMany]
	/// public class ExampleProvider : IMenuProviderForUserPanel { }
	/// </example>
	/// </summary>
	public interface IMenuProvider {
		/// <summary>
		/// 设置显示的菜单项
		/// </summary>
		/// <param name="groups">菜单项分组列表</param>
		void Setup(List<MenuItemGroup> groups);
	}
}
