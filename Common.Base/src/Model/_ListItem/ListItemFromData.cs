using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 根据数据提供选项列表
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	public class ListItemFromData<TData> : IListItemProvider
		where TData : class, IEntity {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.CreateContext()) {
				foreach (var data in context.Query<TData>()) {
					yield return new ListItem(data.ToString(), EntityTrait.GetPrimaryKey(data).ToString());
				}
			}
		}
	}
}
