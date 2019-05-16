using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities.Bases;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities {
	/// <summary>
	/// 买家订单
	/// 需要获取参数和状态时应该查看关联的卖家订单
	/// 需要修改参数和状态时应该修改关联的卖家订单
	/// </summary>
	[ExportMany]
	public class BuyerOrder : OrderBase<BuyerOrder> {
		/// <summary>
		/// 卖家订单
		/// </summary>
		public virtual SellerOrder SellerOrder { get; set; }
		/// <summary>
		/// 买家的会话Id
		/// 仅用于非会员下单
		/// </summary>
		public virtual Guid? BuyerSessionId { get; set; }

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return SellerOrder.Serial;
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public override void Configure(IEntityMappingBuilder<BuyerOrder> builder) {
			base.Configure(builder);
			builder.References(o => o.SellerOrder);
			builder.Map(o => o.BuyerSessionId,
				new EntityMappingOptions() { Index = "Idx_BuyerOrder_BuyerSessionId" });
		}
	}
}
