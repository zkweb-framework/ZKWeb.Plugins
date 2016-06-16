using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 根据未删除的数据提供选项列表
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	public class ListItemFromDataNotDeleted<TData> : IListItemProvider where TData : class {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			var propertyName = RecyclableTrait.For<TData>().PropertyName;
			var expression = ExpressionUtils.MakeMemberEqualiventExpression<TData>(propertyName, false);
			using (var context = databaseManager.GetContext()) {
				foreach (var data in context.Query<TData>().Where(expression)) {
					yield return new ListItem(data.ToString(), EntityTrait.GetPrimaryKey(data).ToString());
				}
			}
		}
	}
}
