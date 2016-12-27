using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductBookmark.src.Domain.Entities {
	using Product = Product.src.Domain.Entities.Product;

	/// <summary>
	/// 商品收藏
	/// </summary>
	[ExportMany]
	public class ProductBookmark :
		IHaveCreateTime, IHaveUpdateTime,
		IHaveOwner,
		IEntity<Guid>, IEntityMappingProvider<ProductBookmark> {
		/// <summary>
		/// 收藏Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 收藏的商品
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 收藏商品的用户
		/// </summary>
		public virtual User Owner { get; set; }
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
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<ProductBookmark> builder) {
			builder.Id(b => b.Id);
			builder.References(b => b.Product, new EntityMappingOptions() { Nullable = false });
			builder.References(b => b.Owner, new EntityMappingOptions() { Nullable = false });
			builder.Map(b => b.CreateTime);
			builder.Map(b => b.UpdateTime);
			builder.Map(b => b.Deleted);
		}
	}
}
