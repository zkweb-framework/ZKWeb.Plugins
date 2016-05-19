using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// 数据表格的扩展函数
	/// </summary>
	public static class DataTableExtensions {
		/// <summary>
		/// 转换数据表格到Html
		/// 表格头可以使用DataColumn.ExtendedProperties指定参数allowHtml和width
		/// 表格行中的数据如果类型是HtmlString将会直接描画，否则进行html编码后描画
		/// </summary>
		/// <param name="table">表格数据</param>
		/// <param name="tableClass">表格的css类，默认是table table-bordered table-hover</param>
		/// <param name="tableHeadRowClass">表格头部行的css类，默认是heading</param>
		/// <returns></returns>
		public static HtmlString ToHtml(this DataTable table,
			string tableClass = null, string tableHeadRowClass = null) {
			var htmlBuilder = new StringBuilder();
			tableClass = HttpUtility.HtmlAttributeEncode(tableClass ?? "table table-bordered table-hover");
			tableHeadRowClass = HttpUtility.HtmlAttributeEncode(tableHeadRowClass ?? "heading");
			htmlBuilder.AppendFormat("<table class='{0}'>", tableClass);
			htmlBuilder.AppendFormat("<thead><tr role='row' class='{0}'>", tableHeadRowClass);
			foreach (DataColumn column in table.Columns) {
				var width = HttpUtility.HtmlAttributeEncode(
					(column.ExtendedProperties["width"] ?? "").ToString());
				var allowHtml = column.ExtendedProperties["allowHtml"].ConvertOrDefault<bool>();
				var caption = allowHtml ? column.Caption : HttpUtility.HtmlEncode(column.Caption);
				htmlBuilder.AppendFormat("<th width='{0}'>{1}</th>", width, new T(caption));
			}
			htmlBuilder.Append("</tr></thead><tbody>");
			foreach (DataRow row in table.Rows) {
				htmlBuilder.Append("<tr role='row'>");
				foreach (DataColumn column in table.Columns) {
					var data = row[column];
					var html = (data is HtmlString) ? data : HttpUtility.HtmlEncode((data ?? "").ToString());
					htmlBuilder.AppendFormat("<td>{0}</td>", html);
				}
				htmlBuilder.Append("</tr>");
			}
			htmlBuilder.Append("</tbody></table>");
			return new HtmlString(htmlBuilder.ToString());
		}
	}
}
