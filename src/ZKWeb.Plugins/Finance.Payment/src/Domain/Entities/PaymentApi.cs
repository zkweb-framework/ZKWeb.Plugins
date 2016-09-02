using DotLiquid;
using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Domain.Entities {
	/// <summary>
	/// 支付接口
	/// 用于用户支付金钱给网站或其他用户
	/// </summary>
	[ExportMany]
	public class PaymentApi :
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted,
		IEntity<Guid>, IEntityMappingProvider<PaymentApi>,
		ILiquidizable {
		/// <summary>
		/// 接口Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 接口类型
		/// 创建后不能修改
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 接口的所属用户，null时属于网站
		/// 创建后不能修改
		/// </summary>
		public virtual User Owner { get; set; }
		/// <summary>
		/// 附加数据
		/// </summary>
		public virtual PaymentApiExtraData ExtraData { get; set; }
		/// <summary>
		/// 支持的交易类型列表
		/// </summary>
		public virtual List<string> SupportTransactionTypes { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 显示顺序，从小到大
		/// </summary>
		public virtual long DisplayOrder { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public PaymentApi() {
			DisplayOrder = 10000;
			ExtraData = new PaymentApiExtraData();
			SupportTransactionTypes = new List<string>();
		}

		/// <summary>
		/// 允许描画到模板
		/// </summary>
		/// <returns></returns>
		public virtual object ToLiquid() {
			return new { Id, Name, Type };
		}

		/// <summary>
		/// 返回支付接口名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return new T(Name);
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<PaymentApi> builder) {
			builder.Id(a => a.Id);
			builder.Map(a => a.Name);
			builder.Map(a => a.Type, new EntityMappingOptions() {
				Index = "Idx_Type"
			});
			builder.References(a => a.Owner, new EntityMappingOptions() {
				Nullable = true
			});
			builder.Map(a => a.ExtraData, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(a => a.SupportTransactionTypes, new EntityMappingOptions() {
				WithSerialization = true
			});
			builder.Map(a => a.CreateTime);
			builder.Map(a => a.UpdateTime);
			builder.Map(a => a.Deleted);
			builder.Map(a => a.DisplayOrder);
			builder.Map(a => a.Remark);
		}

		/// <summary>
		/// 支付接口的附加数据
		/// </summary>
		public class PaymentApiExtraData : Dictionary<string, object> { }
	}
}
