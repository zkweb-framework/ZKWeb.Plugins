using System.Collections.Generic;
using System.Linq;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems {
	/// <summary>
	/// 根据数据提供选项列表
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">实体主键</typeparam>
	public class ListItemFromEntities<TEntity, TPrimaryKey> : IListItemProvider
		where TEntity : class, IEntity<TPrimaryKey> {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			var manager = Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
			var entities = manager.GetMany();
			return entities.Select(e => new ListItem(e.ToString(), e.Id.ToString()));
		}
	}
}
