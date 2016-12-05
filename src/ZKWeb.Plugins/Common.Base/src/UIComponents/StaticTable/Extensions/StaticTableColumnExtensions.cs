using System.Collections.Generic;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions {
	/// <summary>
	/// 静态表格的列的扩展函数
	/// </summary>
	public static class StaticTableColumnExtensions {
		/// <summary>
		/// 添加列
		/// </summary>
		/// <param name="columns">列列表</param>
		/// <param name="member">成员</param>
		/// <param name="width">宽度</param>
		/// <param name="allowHtml">是否允许标题和成员使用Html，默认不允许</param>
		/// <param name="caption">标题，默认使用成员名称</param>
		/// <returns></returns>
		public static StaticTableBuilder.Column Add(
			this IList<StaticTableBuilder.Column> columns,
			string member, string width = null,
			bool allowHtml = false, string caption = null, string cssClass = null) {
			var column = new StaticTableBuilder.Column(member, width, allowHtml, caption, cssClass);
			columns.Add(column);
			return column;
		}
	}
}
