using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities {
	/// <summary>
	/// 订单留言
	/// </summary>
	[ExportMany]
	public class OrderComment :
		IHaveCreateTime, IHaveUpdateTime, IHaveOwner,
		IEntity<Guid>, IEntityMappingProvider<OrderComment> {
		/// <summary>
		/// 留言Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 所属订单
		/// </summary>
		public virtual SellerOrder Order { get; set; }
		/// <summary>
		/// 所属人(留言人)
		/// 非会员下单留言时等于null
		/// </summary>
		public virtual User Owner { get; set; }
		/// <summary>
		/// 买家或卖家留言
		/// </summary>
		public virtual OrderCommentSide Side { get; set; }
		/// <summary>
		/// 留言内容
		/// </summary>
		public virtual string Contents { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<OrderComment> builder) {
			builder.Id(c => c.Id);
			builder.References(c => c.Order);
			builder.References(c => c.Owner);
			builder.Map(c => c.Side);
			builder.Map(c => c.Contents);
			builder.Map(c => c.CreateTime);
			builder.Map(c => c.UpdateTime);
		}
	}
}
