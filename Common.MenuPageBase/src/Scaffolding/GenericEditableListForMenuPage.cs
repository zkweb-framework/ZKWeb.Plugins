using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.MenuPageBase.src.Scaffolding {
	/// <summary>
	/// 用于增删查改数据的菜单页面构建器
	/// 这个接口需要再次继承，请勿直接使用
	/// <example>
	/// public abstract class GenericEditableListForUserPanel :
	///		GenericEditableListForMenuPage, IMenuProviderForUserPanel { }
	/// [ExportMany]
	/// public class ExampleEditableList : GenericEditableListForUserPanel { }
	/// </example>
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	public abstract class GenericEditableListForMenuPage<TData> : GenericListForMenuPage<TData>
		where TData : class {

	}
}
