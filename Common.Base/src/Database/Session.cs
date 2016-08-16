using System;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWebStandard.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Database {
	/// <summary>
	/// 会话
	/// </summary>
	[ExportMany]
	public class Session : IEntity<string>, IEntityMappingProvider<Session> {
		/// <summary>
		/// 会话Id
		/// </summary>
		public virtual string Id { get; set; }
		/// <summary>
		/// 关联Id
		/// 一般是用户Id
		/// </summary>
		public virtual long ReleatedId { get; set; }
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
			builder.Id(s => s.Id);
			builder.Map(s => s.ReleatedId);
			builder.Map(s => s.Items, new EntityMappingOptions() { WithSerialization = true });
			builder.Map(s => s.IpAddress);
			builder.Map(s => s.RememberLogin);
			builder.Map(s => s.Expires, new EntityMappingOptions() { Index = "Idx_Expires" });
		}
	}

	/// <summary>
	/// 会话的扩展函数
	/// </summary>
	public static class SessionExtensions {
		/// <summary>
		/// 重新生成Id
		/// </summary>
		public static void ReGenerateId(this Session session) {
			session.Id = RandomUtils.SystemRandomBytes(20).ToHex();
		}

		/// <summary>
		/// 设置会话最少在指定的时间后过期
		/// 当前会话的过期时间比指定的时间要晚时不更新当前的过期时间
		/// </summary>
		/// <param name="session">会话</param>
		/// <param name="span">最少在这个时间后过期</param>
		public static void SetExpiresAtLeast(this Session session, TimeSpan span) {
			var expires = DateTime.UtcNow + span;
			if (session.Expires < expires) {
				session.Expires = expires;
				session.ExpiresUpdated = true;
			}
		}
	}
}
