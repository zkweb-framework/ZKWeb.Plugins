using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using ZKWeb.Database.UserTypes;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericRecord.src.Database {
	/// <summary>
	/// 通用记录
	/// 删除时将直接从数据库中删除
	/// 这个类型只应该用于记录性的数据，不应该储存有业务关联的数据
	/// </summary>
	public class GenericRecord {
		/// <summary>
		/// 记录Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 记录类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 关联的数据Id
		/// </summary>
		public virtual long? ReleatedId { get; set; }
		/// <summary>
		/// 创建记录的用户，系统记录时等于null
		/// </summary>
		public virtual User Creator { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
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
		public virtual Dictionary<string, object> ExtraData { get; set; }
	}

	/// <summary>
	/// 通用记录的数据库结构
	/// </summary>
	[ExportMany]
	public class GenericRecordMap : ClassMap<GenericRecord> {
		/// <summary>
		/// 初始化
		/// </summary>
		public GenericRecordMap() {
			Id(r => r.Id);
			Map(r => r.Type).Not.Nullable().Index("Idx_Type");
			Map(r => r.ReleatedId).Index("Idx_ReleatedId");
			References(r => r.Creator).Nullable();
			Map(r => r.CreateTime);
			Map(r => r.KeepUntil).Nullable().Index("Idx_KeepUntil");
			Map(r => r.Content);
			Map(r => r.ExtraData).CustomType<JsonSerializedType<Dictionary<string, object>>>();
		}
	}
}
