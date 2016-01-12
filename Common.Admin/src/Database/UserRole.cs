using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.Database {
	/// <summary>
	/// 用户角色
	/// </summary>
	public class UserRole {
		/// <summary>
		/// 角色Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 角色名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 权限列表
		/// </summary>
		public virtual HashSet<string> Privileges { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public UserRole() {
			Privileges = new HashSet<string>();
		}

		/// <summary>
		/// 显示角色名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Name;
		}
	}

	/// <summary>
	/// 用户角色的数据库结构
	/// </summary>
	[ExportMany]
	public class UserRoleMap : ClassMap<UserRole> {
		public UserRoleMap() {
			Id(r => r.Id);
			Map(r => r.Name);
			Map(r => r.Privileges).CustomType<JsonSerializedType<HashSet<string>>>();
			Map(r => r.CreateTime);
			Map(r => r.LastUpdated);
			Map(r => r.Remark).Length(0xffff);
			Map(r => r.Deleted);
		}
	}
}
