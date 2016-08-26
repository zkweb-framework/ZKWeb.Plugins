namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes {
	/// <summary>
	/// 隐藏字段
	/// </summary>
	public class HiddenFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		public HiddenFieldAttribute(string name) {
			Name = name;
		}
	}
}
