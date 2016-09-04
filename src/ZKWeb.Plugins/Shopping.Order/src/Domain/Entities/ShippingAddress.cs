using DotLiquid;
using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities {
	/// <summary>
	/// 收货地址
	/// </summary>
	[ExportMany]
	public class ShippingAddress :
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted, IHaveOwner,
		ILiquidizable,
		IEntity<Guid>, IEntityMappingProvider<ShippingAddress> {
		/// <summary>
		/// 收货地址Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 所属的用户
		/// </summary>
		public virtual User Owner { get; set; }
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
		public virtual DateTime UpdateTime { get; set; }
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
		public ShippingAddress() {
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
		public virtual void Configure(IEntityMappingBuilder<ShippingAddress> builder) {
			builder.Id(a => a.Id);
			builder.References(a => a.Owner,
				new EntityMappingOptions() { Nullable = false });
			builder.Map(a => a.Country,
				new EntityMappingOptions() { Nullable = false });
			builder.Map(a => a.RegionId);
			builder.Map(a => a.ZipCode);
			builder.Map(a => a.DetailedAddress);
			builder.Map(a => a.ReceiverName);
			builder.Map(a => a.ReceiverTel);
			builder.Map(a => a.Summary);
			builder.Map(a => a.CreateTime);
			builder.Map(a => a.UpdateTime);
			builder.Map(a => a.DisplayOrder);
			builder.Map(a => a.Deleted);
			builder.Map(a => a.Remark);
		}
	}
}
