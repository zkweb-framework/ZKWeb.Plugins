namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// 多行文本框
	/// </summary>
	public class TextAreaFieldAttribute : TextBoxFieldAttribute {
		/// <summary>
		/// 行数
		/// </summary>
		public int Rows { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="rows">行数</param>
		/// <param name="placeHolder">预置文本</param>
		public TextAreaFieldAttribute(string name, int rows, string placeHolder = null) :
			base(name, placeHolder) {
			Rows = rows;
		}
	}
}
