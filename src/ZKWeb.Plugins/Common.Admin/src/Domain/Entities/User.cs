using System;
using System.Collections.Generic;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Structs;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Entities {
	/// <summary>
	/// 用户
	/// </summary>
	[ExportMany]
	public class User :
		IEntity<Guid>,
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted,
		IEntityMappingProvider<User> {
		/// <summary>
		/// 用户Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 用户类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 用户名
		/// </summary>
		public virtual string Username { get; set; }
		/// <summary>
		/// 密码信息，json
		/// </summary>
		public virtual PasswordInfo Password { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public virtual UserItems Items { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 关联的用户角色
		/// </summary>
		public virtual ISet<UserRole> Roles { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public User() {
			Items = new UserItems();
			Roles = new HashSet<UserRole>();
		}

		/// <summary>
		/// 显示用户名
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Username;
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<User> builder) {
			builder.Id(u => u.Id);
			builder.Map(u => u.Type, new EntityMappingOptions() {
				Index = "Idx_Type", Length = 255
			});
			builder.Map(u => u.Username, new EntityMappingOptions() {
				Unique = true, Length = 255
			});
			builder.Map(u => u.Password, new EntityMappingOptions() { WithSerialization = true });
			builder.Map(u => u.CreateTime);
			builder.Map(u => u.UpdateTime);
			builder.HasManyToMany(u => u.Roles);
			builder.Map(u => u.Items, new EntityMappingOptions() { WithSerialization = true });
			builder.Map(u => u.Deleted);
		}
	}
}
