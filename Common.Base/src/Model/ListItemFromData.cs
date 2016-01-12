using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 根据数据提供选项列表
	/// 要求数据拥有Id成员
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	public class ListItemFromData<TData> : IListItemProvider where TData : class {
		/// <summary>
		/// 从数据获取Id的函数
		/// </summary>
		public readonly static Lazy<Func<TData, object>> GetId =
			new Lazy<Func<TData, object>>(() => ReflectionUtils.MakeGetter<TData, object>("Id"));

		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var databaseManager = Application.Ioc.Resolve<DatabaseManager>();
			using (var context = databaseManager.GetContext()) {
				foreach (var data in context.Query<TData>()) {
					yield return new ListItem(data.ToString(), GetId.Value(data).ToString());
				}
			}
		}
	}
}
