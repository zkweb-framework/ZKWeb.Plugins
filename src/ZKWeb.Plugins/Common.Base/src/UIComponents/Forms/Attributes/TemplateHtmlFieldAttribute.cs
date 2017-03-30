namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 显示模板Html的属性
	/// </summary>
	public class TemplateHtmlFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 模板路径
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <param name="path">模板路径</param>
		public TemplateHtmlFieldAttribute(string name, string path) {
			Name = name;
			Path = path;
		}
	}
}
