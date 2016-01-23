using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;

namespace ZKWeb.Plugins.Common.UserContact.src.Database {
	/// <summary>
	/// 用户的联系信息
	/// </summary>
	public class UserContact {
		/// <summary>
		/// 信息Id
		/// </summary>
		public virtual long Id { get; set; }
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
	}

	/// <summary>
	/// 联系信息的数据库结构
	/// </summary>
	[ExportMany]
	public class UserContactMap : ClassMap<UserContact> {
		/// <summary>
		/// 初始化
		/// </summary>
		public UserContactMap() {
			Id(c => c.Id);
			References(c => c.User).Not.Nullable().Unique();
			Map(c => c.Tel);
			Map(c => c.Mobile);
			Map(c => c.QQ);
			Map(c => c.Email);
			Map(c => c.Address).Length(0xffff);
		}
	}
}
