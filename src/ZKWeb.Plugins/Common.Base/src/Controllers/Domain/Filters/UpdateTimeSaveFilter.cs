using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Filters {
	/// <summary>
	/// 自动设置实体的更新时间
	/// </summary>
	[ExportMany]
	public class UpdateTimeSaveFilter : IEntitySaveFilter {
		/// <summary>
		/// 自动设置实体的更新时间
		/// </summary>
		public virtual void Filter<TEntity, TPrimaryKey>(TEntity entity)
			where TEntity : class, IEntity<TPrimaryKey> {
			if (entity is IHaveUpdateTime) {
				var et = (IHaveUpdateTime)entity;
				et.UpdateTime = DateTime.UtcNow;
			}
		}
	}
}
