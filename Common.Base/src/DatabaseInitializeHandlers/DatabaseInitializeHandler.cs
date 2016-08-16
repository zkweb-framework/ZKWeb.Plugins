using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.DatabaseInitializeHandlers {
	/// <summary>
	/// Add prefix to table name
	/// </summary>
	[ExportMany]
	public class DatabaseInitializeHandler : IDatabaseInitializeHandler {
		public void ConvertTableName(ref string tableName) {
			tableName = "ZKWeb_" + tableName;
		}
	}
}
