using System;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes {
	/// <summary>
	/// 单选按钮列表
	/// </summary>
	public class RadioButtonsFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 选项来源，必须继承IListItemProvider
		/// </summary>
		public Type Source { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="sources">选项来源，必须继承IListItemProvider</param>
		public RadioButtonsFieldAttribute(string name, Type sources) {
			Name = name;
			Source = sources;
		}
	}
}
