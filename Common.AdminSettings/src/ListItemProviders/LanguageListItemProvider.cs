using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;

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
			var languages = Application.Ioc.ResolveMany<ILanguage>();
			foreach (var language in languages) {
				yield return new ListItem(new T(language.Name), language.Name);
			}
		}
	}
}
