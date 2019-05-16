using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Enums;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Structs;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Domain.Entities {
	using Product = Product.src.Domain.Entities.Product;

	/// <summary>
	/// 购物车商品
	/// 可以属于用户也可以属于会话，属于会话时需要指定比较短的过期时间
	/// 类型可以是默认也可以是立刻购买，立刻购买时需要指定比较短的过期时间
	/// </summary>
	[ExportMany]
	public class CartProduct :
		IHaveCreateTime, IHaveUpdateTime, IHaveOwner,
		IEntity<Guid>, IEntityMappingProvider<CartProduct> {
		/// <summary>
		/// 数据Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 类型
		/// 默认或立刻购买
		/// </summary>
		public virtual CartProductType Type { get; set; }
		/// <summary>
		/// 所属人用户
		/// 没有时等于null
		/// </summary>
		public virtual User Owner { get; set; }
		/// <summary>
		/// 所属人会话Id
		/// 已有用户时等于null
		/// </summary>
		public virtual Guid? OwnerSessionId { get; set; }
		/// <summary>
		/// 商品
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 购买数量
		/// </summary>
		public virtual long Count { get; set; }
		/// <summary>
		/// 商品匹配参数
		/// 包含规格等信息
		/// 这里会包含购买数量但不应该使用这里的数量
		/// </summary>
		public virtual ProductMatchParameters MatchParameters { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 过期时间
		/// </summary>
		public virtual DateTime ExpireTime { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public CartProduct() {
			MatchParameters = new ProductMatchParameters();
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<CartProduct> builder) {
			builder.Id(c => c.Id);
			builder.Map(c => c.Type);
			builder.References(c => c.Owner);
			builder.Map(c => c.OwnerSessionId,
				new EntityMappingOptions() { Index = "Idx_CartProduct_OwnerSessionId" });
			builder.References(c => c.Product);
			builder.Map(c => c.Count);
			builder.Map(c => c.MatchParameters,
				new EntityMappingOptions() { WithSerialization = true });
			builder.Map(c => c.CreateTime);
			builder.Map(c => c.UpdateTime);
			builder.Map(c => c.ExpireTime,
				new EntityMappingOptions() { Index = "Idx_CartProduct_ExpireTime" });
		}
	}
}
