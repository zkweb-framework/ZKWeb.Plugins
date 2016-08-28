namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 勾选框
	/// </summary>
	public class CheckBoxFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public CheckBoxFieldAttribute(string name) {
			Name = name;
		}
	}
}
