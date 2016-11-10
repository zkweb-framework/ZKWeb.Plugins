using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Interfaces;
using ZKWeb.Web;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Controllers.ActionResults {
	/// <summary>
	/// 静态表格的导出结果
	/// </summary>
	public class StaticTableExportResult : IActionResult {
		/// <summary>
		/// 静态表格
		/// </summary>
		private StaticTableBuilder Table { get; set; }
		/// <summary>
		/// 不带后缀的文件名
		/// </summary>
		private string FilenameWithoutExtensions { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="table">静态表格</param>
		/// <param name="filenameWithoutExtensions">不带后缀的文件名</param>
		public StaticTableExportResult(StaticTableBuilder table, string filenameWithoutExtensions) {
			Table = table;
			FilenameWithoutExtensions = filenameWithoutExtensions;
		}

		/// <summary>
		/// 写入到Http回应
		/// </summary>
		public void WriteResponse(IHttpResponse response) {
			var exporter = Application.Ioc.Resolve<IStaticTableExporter>();
			var filename = FilenameWithoutExtensions + exporter.FileExtension;
			response.StatusCode = 200;
			response.ContentType = MimeUtils.GetMimeType(filename);
			response.AddHeader("Content-Disposition", $"attachment; filename=\"{filename}\"");
			exporter.Export(Table, response.Body);
		}
	}
}
