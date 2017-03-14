using System;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 勾选框分组列表
	/// </summary>
	public class CheckBoxGroupsFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 选项来源，必须继承IListItemGroupsProvider
		/// 如果不指定, 值的类型必须是ListItemValueWithProvider
		/// </summary>
		public Type Source { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public CheckBoxGroupsFieldAttribute(string name) {
			Name = name;
			Source = null;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="source">选项来源，必须继承IListItemGroupsProvider</param>
		public CheckBoxGroupsFieldAttribute(string name, Type source) {
			Name = name;
			Source = source;
		}
	}
}
