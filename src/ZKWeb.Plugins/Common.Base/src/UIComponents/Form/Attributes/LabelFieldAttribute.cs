namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes {
	/// <summary>
	/// 只读文本
	/// </summary>
	public class LabelFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public LabelFieldAttribute(string name) {
			Name = name;
		}
	}
}
