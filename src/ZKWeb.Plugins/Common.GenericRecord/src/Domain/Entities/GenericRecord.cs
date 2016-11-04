using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.GenericRecord.src.Domain.Structs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericRecord.src.Domain.Entities {
	/// <summary>
	/// 通用记录
	/// 删除时将直接从数据库中删除
	/// 这个类型只应该用于记录性的数据，不应该储存有业务关联的数据
	/// </summary>
	[ExportMany]
	public class GenericRecord :
		IHaveCreateTime, IHaveUpdateTime,
		IEntity<Guid>, IEntityMappingProvider<GenericRecord> {
		/// <summary>
		/// 记录Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 记录类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 关联的数据Id
		/// </summary>
		public virtual Guid? ReleatedId { get; set; }
		/// <summary>
		/// 创建记录的用户，系统记录时等于null
		/// </summary>
		public virtual User Creator { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 过期时间，等于null时永久保留
		/// </summary>
		public virtual DateTime? KeepUntil { get; set; }
		/// <summary>
		/// 记录内容
		/// </summary>
		public virtual string Content { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public virtual GenericRecordExtraData ExtraData { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericRecord() {
			ExtraData = new GenericRecordExtraData();
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<GenericRecord> builder) {
			builder.Id(r => r.Id);
			builder.Map(r => r.Type, new EntityMappingOptions() {
				Nullable = false,
				Index = "Idx_Type"
			});
			builder.Map(r => r.ReleatedId, new EntityMappingOptions() { Index = "Idx_ReleatedId" });
			builder.References(r => r.Creator, new EntityMappingOptions() { Nullable = true });
			builder.Map(r => r.CreateTime, new EntityMappingOptions() { Index = "Idx_CreateTime" });
			builder.Map(r => r.UpdateTime);
			builder.Map(r => r.KeepUntil, new EntityMappingOptions() {
				Nullable = true,
				Index = "Idx_KeepUntil"
			});
			builder.Map(r => r.Content);
			builder.Map(r => r.ExtraData, new EntityMappingOptions() { WithSerialization = true });
		}
	}
}
