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
	public class GuidEntityFilter : IEntityOperationFilter {
		/// <summary>
		/// 自动设置Guid主键值
		/// </summary>
		void IEntityOperationFilter.FilterSave<TEntity, TPrimaryKey>(TEntity entity) {
			if (typeof(TPrimaryKey) == typeof(Guid)) {
				var eg = (IEntity<Guid>)entity;
				if (eg.Id == default(Guid)) {
					// 主键是空时自动生成主键
					eg.Id = GuidUtils.SequentialGuid(DateTime.UtcNow);
				}
			}
		}

		/// <summary>
		/// 不需要处理删除
		/// </summary>
		void IEntityOperationFilter.FilterDelete<TEntity, TPrimaryKey>(TEntity entity) { }
	}
}
