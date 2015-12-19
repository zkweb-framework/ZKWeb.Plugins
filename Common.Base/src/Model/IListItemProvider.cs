using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 下拉框或单选按钮使用的选项列表来源的接口
	/// </summary>
	public interface IListItemProvider {
		IEnumerable<ListItem> GetItems();
	}
}
