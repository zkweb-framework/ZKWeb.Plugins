using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 给选项列表添加"请选择"的空值选项
	/// 例子
	/// [DropdownListField("Type", typeof(ListItemsWithOptional[ListItemFromEnum[TestTypes]]))]
	/// </summary>
	/// <typeparam name="TBaseProvider"></typeparam>
	public class ListItemsWithOptional<TBaseProvider> : IListItemProvider
		where TBaseProvider : IListItemProvider, new() {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			yield return new ListItem(new T("Please Select"), "");
			var baseProvider = new TBaseProvider();
			foreach (var item in baseProvider.GetItems()) {
				yield return item;
			}
		}
	}
}
