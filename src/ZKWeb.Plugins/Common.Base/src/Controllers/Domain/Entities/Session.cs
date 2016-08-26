using System;
using ZKWebStandard.Ioc;
using ZKWeb.Database;
using System.Collections.Generic;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Entities {
	/// <summary>
	/// 会话
	/// </summary>
	[ExportMany]
	public class Session : IEntity<Guid>, IEntityMappingProvider<Session> {
		/// <summary>
		/// 会话Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 关联Id
		/// 一般是用户Id
		/// </summary>
		public virtual Guid ReleatedId { get; set; }
		/// <summary>
		/// 会话数据
		/// </summary>
		public virtual SessionItems Items { get; set; }
		/// <summary>
		/// 会话对应的Ip地址
		/// </summary>
		public virtual string IpAddress { get; set; }
		/// <summary>
		/// 是否记住登录
		/// </summary>
		public virtual bool RememberLogin { get; set; }
		/// <summary>
		/// 过期时间
		/// </summary>
		public virtual DateTime Expires { get; set; }

		/// <summary>
		/// 过期时间是否已更新
		/// 这个值只用于检测是否应该把新的过期时间发送到客户端
		/// </summary>
		public virtual bool ExpiresUpdated { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Session() {
			Items = new SessionItems();
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<Session> builder) {
			builder.Id(s => s.Id, new EntityMappingOptions() { CustomSqlType = "uuid" });
			builder.Map(s => s.ReleatedId);
			builder.Map(s => s.Items, new EntityMappingOptions() { WithSerialization = true });
			builder.Map(s => s.IpAddress);
			builder.Map(s => s.RememberLogin);
			builder.Map(s => s.Expires, new EntityMappingOptions() { Index = "Idx_Expires" });
		}

		/// <summary>
		/// 会话数据
		/// </summary>
		public class SessionItems : Dictionary<string, object> { }
	}
}
