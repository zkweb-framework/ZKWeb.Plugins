namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 文本框
	/// </summary>
	public class TextBoxFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 预置文本
		/// </summary>
		public string PlaceHolder { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="placeHolder">预置文本</param>
		public TextBoxFieldAttribute(string name, string placeHolder = null) {
			Name = name;
			PlaceHolder = placeHolder;
		}
	}
}
