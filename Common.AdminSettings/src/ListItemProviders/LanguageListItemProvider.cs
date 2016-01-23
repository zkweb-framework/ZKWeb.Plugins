using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.AdminSettings.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.AdminSettings.src.ListItemProviders {
	/// <summary>
	/// 语言列表
	/// </summary>
	public class LanguageListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			foreach (Languages lang in Enum.GetValues(typeof(Languages))) {
				var code = lang.GetDescription();
				yield return new ListItem(new T(code), code);
			}
		}
	}
}
