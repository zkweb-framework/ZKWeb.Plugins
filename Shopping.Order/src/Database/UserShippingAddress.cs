using DotLiquid;
using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Database {
	/// <summary>
	/// 收货地址
	/// </summary>
	[ExportMany]
	public class UserShippingAddress :
		ILiquidizable, IEntity<long>, IEntityMappingProvider<UserShippingAddress> {
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
		public virtual long? RegionId { get; set; }
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
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public UserShippingAddress() {
			DisplayOrder = 10000;
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public virtual object ToLiquid() {
			return new {
				Id, Country, RegionId, ZipCode, DetailedAddress,
				ReceiverName, ReceiverTel, Summary
			};
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Summary;
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<UserShippingAddress> builder) {
			builder.Id(a => a.Id);
			builder.References(a => a.User, new EntityMappingOptions() {
				Nullable = false
			});
			builder.Map(a => a.Country, new EntityMappingOptions() {
				Nullable = false
			});
			builder.Map(a => a.RegionId);
			builder.Map(a => a.ZipCode);
			builder.Map(a => a.DetailedAddress);
			builder.Map(a => a.ReceiverName);
			builder.Map(a => a.ReceiverTel);
			builder.Map(a => a.Summary);
			builder.Map(a => a.CreateTime);
			builder.Map(a => a.LastUpdated);
			builder.Map(a => a.DisplayOrder);
			builder.Map(a => a.Deleted);
			builder.Map(a => a.Remark);
		}
	}
}
