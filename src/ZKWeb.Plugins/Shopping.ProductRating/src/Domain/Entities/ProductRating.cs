using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Entities;
using ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Enums;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.ProductRating.src.Domain.Entities {
	using Product = Product.src.Domain.Entities.Product;

	/// <summary>
	/// 商品评价
	/// </summary>
	[ExportMany]
	public class ProductRating :
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted,
		IHaveOwner,
		IEntity<Guid>, IEntityMappingProvider<ProductRating> {
		/// <summary>
		/// 评价Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 评价的订单商品
		/// </summary>
		public virtual OrderProduct OrderProduct { get; set; }
		/// <summary>
		/// 评价的商品，是OrderProduct的冗余，用于改进查询速度
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 评价人
		/// </summary>
		public virtual User Owner { get; set; }
		/// <summary>
		/// 评价级别
		/// </summary>
		public virtual ProductRatingRank Rank { get; set; }
		/// <summary>
		/// 评价内容
		/// </summary>
		public virtual string Comment { get; set; }
		/// <summary>
		/// 描述相符的分数，范围是1~5
		/// </summary>
		public virtual int DescriptionMatchScore { get; set; }
		/// <summary>
		/// 服务质量的分数，范围是1~5
		/// </summary>
		public virtual int ServiceQualityScore { get; set; }
		/// <summary>
		/// 发货速度的分数，范围是1~5
		/// </summary>
		public virtual int DeliverySpeedScore { get; set; }
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
		public virtual void Configure(IEntityMappingBuilder<ProductRating> builder) {
			builder.Id(r => r.Id);
			builder.References(r => r.OrderProduct, new EntityMappingOptions() { Nullable = false });
			builder.References(r => r.Product, new EntityMappingOptions() { Nullable = false });
			builder.References(r => r.Owner, new EntityMappingOptions() { Nullable = false });
			builder.Map(r => r.Rank);
			builder.Map(r => r.Comment);
			builder.Map(r => r.DescriptionMatchScore);
			builder.Map(r => r.ServiceQualityScore);
			builder.Map(r => r.DeliverySpeedScore);
			builder.Map(r => r.CreateTime);
			builder.Map(r => r.UpdateTime);
			builder.Map(r => r.Deleted);
		}

		/// <summary>
		/// 最大分数值
		/// </summary>
		public static readonly int MaxScore = 5;

		/// <summary>
		/// 检查分数值是否有效
		/// </summary>
		/// <param name="score">分数值</param>
		/// <returns></returns>
		public static bool CheckScore(int score) {
			return score >= 1 && score <= MaxScore;
		}
	}
}
