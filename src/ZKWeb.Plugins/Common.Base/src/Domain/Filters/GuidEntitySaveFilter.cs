using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Filters {
	/// <summary>
	/// 自动设置Guid主键值
	/// </summary>
	[ExportMany]
	public class GuidEntitySaveFilter : IEntitySaveFilter {
		/// <summary>
		/// 自动设置Guid主键值
		/// </summary>
		public virtual void Filter<TEntity, TPrimaryKey>(TEntity entity)
			where TEntity : class, IEntity<TPrimaryKey> {
			if (typeof(TPrimaryKey) == typeof(Guid)) {
				var eg = (IEntity<Guid>)entity;
				if (eg.Id == default(Guid)) {
					eg.Id = GuidUtils.SequentialGuid(DateTime.UtcNow);
				}
			}
		}
	}
}
