namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 显示提示Html的属性
	/// </summary>
	public class AlertHtmlFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 提示类型，例如primary或info
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <param name="type">提示类型，例如primary或info</param>
		public AlertHtmlFieldAttribute(string name, string type) {
			Name = name;
			Type = type;
		}
	}
}
