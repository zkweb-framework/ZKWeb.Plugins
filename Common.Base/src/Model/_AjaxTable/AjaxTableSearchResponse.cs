using System.Collections.Generic;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// Ajax表格数据的搜索回应
	/// </summary>
	public class AjaxTableSearchResponse : BaseTableSearchResponse {
		/// <summary>
		/// 表格列列表
		/// </summary>
		public List<AjaxTableColumn> Columns { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public AjaxTableSearchResponse() {
			Columns = new List<AjaxTableColumn>();
		}
	}
}
