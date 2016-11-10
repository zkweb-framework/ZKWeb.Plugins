using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions {
	/// <summary>
	/// Ajax表格数据的搜索回应的扩展函数
	/// </summary>
	public static class AjaxTableSearchResponseExtensions {
		/// <summary>
		/// 把Ajax表格数据的搜索回应转换为静态表格构建器
		/// </summary>
		/// <param name="response">搜索回应</param>
		/// <returns></returns>
		public static StaticTableBuilder ToTableBuilder(this AjaxTableSearchResponse response) {
			var builder = new StaticTableBuilder();
			// 添加列
			// 不显示Id列，序号列，操作列
			foreach (var column in response.Columns) {
				if (column is AjaxTableIdColumn ||
					column.Key == "No" ||
					column is AjaxTableActionColumn) {
					continue;
				}
				builder.Columns.Add(column.Key);
			}
			// 添加行
			foreach (var row in response.Rows) {
				builder.Rows.Add(row);
			}
			return builder;
		}
	}
}
