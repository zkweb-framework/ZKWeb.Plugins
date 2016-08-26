namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes {
	/// <summary>
	/// 直接显示Html内容的字段
	/// </summary>
	public class HtmlFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称，目前不会用上但是需要预留</param>
		public HtmlFieldAttribute(string name) {
			Name = name;
		}
	}
}
