using System.IO;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Interfaces {
	/// <summary>
	/// 静态表格导出器的接口
	/// </summary>
	public interface IStaticTableExporter {
		/// <summary>
		/// 文件后缀，例如".csv"
		/// </summary>
		string FileExtension { get; }
		/// <summary>
		/// 导出表格到数据流
		/// </summary>
		/// <param name="table">表格对象</param>
		/// <param name="stream">数据流</param>
		void Export(StaticTableBuilder table, Stream stream);
	}
}
