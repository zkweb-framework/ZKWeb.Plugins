using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Filters {
	/// <summary>
	/// 自动设置实体的创建时间
	/// </summary>
	[ExportMany]
	public class SaveFilterForCreateTime : IEntitySaveFilter {
		/// <summary>
		/// 自动设置实体的创建时间
		/// </summary>
		public void Filter<TEntity, TPrimaryKey>(TEntity entity)
			where TEntity : class, IEntity<TPrimaryKey> {
			if (entity is IHaveCreateTime) {
				var et = (IHaveCreateTime)entity;
				if (et.CreateTime == default(DateTime)) {
					et.CreateTime = DateTime.UtcNow;
				}
			}
		}
	}
}
