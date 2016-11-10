using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Interfaces;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTableExporters {
	/// <summary>
	/// 导出静态表格到Csv文件
	/// </summary>
	[ExportMany]
	public class StaticTableCsvExporter : IStaticTableExporter {
		/// <summary>
		/// 用于替换Csv单元格中不应该出现的字符的正则表达式
		/// </summary>
		public static readonly Regex CsvCellRegex = new Regex(@"(,|\s+|(<[\s\S]*?>))");
		/// <summary>
		/// 文件后缀
		/// </summary>
		public string FileExtension { get { return ".csv"; } }

		/// <summary>
		/// 转换到Csv单元格内容
		/// - 如果对象是枚举类型，使用经过翻译的描述
		/// - 否则使用对象ToString的值
		/// - 进行Html解码，解析类似&nbsp;的字符
		/// - 替换逗号，换行符，Html标签到空格
		/// </summary>
		public static string CsvCell(object obj) {
			var str = "";
			if (obj is Enum) {
				str = new T(((Enum)obj).GetDescription());
			} else if (obj != null) {
				str = obj.ToString();
			}
			str = HttpUtils.HtmlDecode(str);
			str = CsvCellRegex.Replace(str, " ");
			str = str.Trim();
			return str;
		}

		/// <summary>
		/// 导出到数据流
		/// </summary>
		public void Export(StaticTableBuilder table, Stream stream) {
			using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true)) {
				// 写入第一行的列名
				writer.WriteLine(string.Join(",",
					table.Columns.Select(c => CsvCell(c.Caption))));
				// 写入数据行
				foreach (var row in table.GetHashRows()) {
					writer.WriteLine(string.Join(",",
						table.Columns.Select(c => CsvCell(row[c.Member]))));
				}
			}
		}
	}
}
