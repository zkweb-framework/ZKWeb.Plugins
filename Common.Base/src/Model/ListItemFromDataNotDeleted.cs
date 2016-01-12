using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.TypeTraits;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 根据未删除的数据提供选项列表
	/// 要求数据拥有Id和Deleted成员
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	public class ListItemFromDataNotDeleted<TData> : IListItemProvider where TData : class {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			var exp = ExpressionUtils.MakeMemberEqualiventExpression<TData>(
				IsRecyclable<TData>.PropertyName, false);
			using (var context = databaseManager.GetContext()) {
				foreach (var data in context.Query<TData>().Where(exp)) {
					yield return new ListItem(
						data.ToString(),
						ListItemFromData<TData>.GetId.Value(data).ToString());
				}
			}
		}
	}
}
