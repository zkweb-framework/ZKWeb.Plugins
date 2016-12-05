namespace ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable {
	/// <summary>
	/// 表格列信息
	/// </summary>
	public class AjaxTableColumn {
		/// <summary>
		/// 列的识别键
		/// </summary>
		public virtual string Key { get; set; }
		/// <summary>
		/// 宽度
		/// </summary>
		public virtual string Width { get; set; }
		/// <summary>
		/// 生成头部Html的模板代码
		/// 格式是underscore.js的默认格式
		/// </summary>
		public virtual string HeadTemplate { get; set; }
		/// <summary>
		/// 生成单元格Html的模板代码
		/// 格式是underscore.js的默认格式
		/// </summary>
		public virtual string CellTemplate { get; set; }
		/// <summary>
		/// 添加到th或者td的css类名
		/// </summary>
		public virtual string CssClass { get; set; }
	}
}
