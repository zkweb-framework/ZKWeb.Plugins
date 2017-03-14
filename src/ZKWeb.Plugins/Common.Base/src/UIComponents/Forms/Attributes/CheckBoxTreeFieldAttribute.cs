using System;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 勾选框树
	/// </summary>
	public class CheckBoxTreeFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 选项来源，必须继承IListItemTreeProvider
		/// 如果不指定, 值的类型必须是ListItemValueWithProvider
		/// </summary>
		public Type Source { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public CheckBoxTreeFieldAttribute(string name) {
			Name = name;
			Source = null;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="source">选项来源，必须继承IListItemProvider</param>
		public CheckBoxTreeFieldAttribute(string name, Type source) {
			Name = name;
			Source = source;
		}
	}
}
