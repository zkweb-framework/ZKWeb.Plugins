using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 下拉框或单选按钮使用的选项列表来源的接口
	/// </summary>
	public interface IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		IEnumerable<ListItem> GetItems();
	}
}
