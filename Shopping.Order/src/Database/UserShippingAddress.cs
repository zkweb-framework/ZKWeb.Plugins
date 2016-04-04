using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	/// <summary>
	/// 收货地址
	/// </summary>
	public class UserShippingAddress {
		/// <summary>
		/// 收货地址Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 所属的用户
		/// </summary>
		public virtual User User { get; set; }
		/// <summary>
		/// 国家
		/// </summary>
		public virtual string Country { get; set; }
		/// <summary>
		/// 地区Id
		/// </summary>
		public virtual long? AreaId { get; set; }
		/// <summary>
		/// 邮政编码
		/// </summary>
		public virtual string ZipCode { get; set; }
		/// <summary>
		/// 详细地址
		/// </summary>
		public virtual string DetailedAddress { get; set; }
		/// <summary>
		/// 收货人姓名
		/// </summary>
		public virtual string ReceiverName { get; set; }
		/// <summary>
		/// 收货人电话/手机
		/// </summary>
		public virtual string ReceiverTel { get; set; }
		/// <summary>
		/// 完整的收货地址字符串
		/// 格式
		/// "{收货人姓名} {国家(设置显示时)}{地区}{详细地址} {收货人电话/手机}"
		/// </summary>
		public virtual string Summary { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 最后更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
		/// <summary>
		/// 显示顺序
		/// </summary>
		public virtual long DisplayOrder { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual long Deleted { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Summary;
		}
	}

	/// <summary>
	/// 收货地址的数据库结构
	/// </summary>
	[ExportMany]
	public class UserShippingAddressMap : ClassMap<UserShippingAddress> {
		/// <summary>
		/// 初始化
		/// </summary>
		public UserShippingAddressMap() {
			Id(a => a.Id);
			References(a => a.User).Not.Nullable();
			Map(a => a.Country).Not.Nullable();
			Map(a => a.AreaId);
			Map(a => a.ZipCode);
			Map(a => a.DetailedAddress).Length(0xffff);
			Map(a => a.ReceiverName).Length(0xffff);
			Map(a => a.ReceiverTel).Length(0xffff);
			Map(a => a.Summary).Length(0xffff);
			Map(a => a.CreateTime);
			Map(a => a.LastUpdated);
			Map(a => a.DisplayOrder);
			Map(a => a.Deleted);
			Map(a => a.Remark).Length(0xffff);
		}
	}
}
