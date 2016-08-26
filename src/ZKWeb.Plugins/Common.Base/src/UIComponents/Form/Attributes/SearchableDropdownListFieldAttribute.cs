using System;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes {
	/// <summary>
	/// 可搜索的下拉列表
	/// </summary>
	public class SearchableDropdownListFieldAttribute : DropdownListFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="source">选项来源，必须继承IListItemProvider</param>
		public SearchableDropdownListFieldAttribute(string name, Type source) : base(name, source) { }
	}
}
