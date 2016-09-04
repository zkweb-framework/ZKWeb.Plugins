using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.UserContact.src.Domain.Entities {
	/// <summary>
	/// 用户的联系信息
	/// </summary>
	[ExportMany]
	public class UserContact :
		IEntity<Guid>, IEntityMappingProvider<UserContact> {
		/// <summary>
		/// 联系信息Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 关联的用户
		/// </summary>
		public virtual User User { get; set; }
		/// <summary>
		/// 电话
		/// </summary>
		public virtual string Tel { get; set; }
		/// <summary>
		/// 手机
		/// </summary>
		public virtual string Mobile { get; set; }
		/// <summary>
		/// QQ
		/// </summary>
		public virtual string QQ { get; set; }
		/// <summary>
		/// 邮箱
		/// </summary>
		public virtual string Email { get; set; }
		/// <summary>
		/// 地址
		/// </summary>
		public virtual string Address { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<UserContact> builder) {
			builder.Id(c => c.Id);
			builder.References(c => c.User, new EntityMappingOptions() {
				Nullable = false, Unique = true
			});
			builder.Map(c => c.Tel);
			builder.Map(c => c.Mobile);
			builder.Map(c => c.QQ);
			builder.Map(c => c.Email);
			builder.Map(c => c.Address);
		}
	}
}
