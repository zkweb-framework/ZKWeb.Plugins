namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes {
	/// <summary>
	/// 密码框
	/// </summary>
	public class PasswordFieldAttribute : TextBoxFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="placeHolder">预置文本</param>
		public PasswordFieldAttribute(string name, string placeHolder = null) :
			base(name, placeHolder) { }
	}
}
