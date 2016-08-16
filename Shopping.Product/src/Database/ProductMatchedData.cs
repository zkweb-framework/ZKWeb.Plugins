using ZKWeb.Database;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Database {
	/// <summary>
	/// 商品匹配数据
	/// </summary>
	[ExportMany]
	public class ProductMatchedData : IEntity<long>, IEntityMappingProvider<ProductMatchedData> {
		/// <summary>
		/// 数据Id
		/// 因为数据在编辑时会删除重建，其他表不能关联这里的Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 属于的商品
		/// </summary>
		public virtual Product Product { get; set; }
		/// <summary>
		/// 匹配条件
		/// </summary>
		public virtual ProductMatchedDataConditions Conditions { get; set; }
		/// <summary>
		/// 影响数据
		/// </summary>
		public virtual ProductMatchedDataAffects Affects { get; set; }
		/// <summary>
		/// 价格，等于null时继续匹配下一项
		/// </summary>
		public virtual decimal? Price { get; set; }
		/// <summary>
		/// 价格的货币，跟随价格匹配
		/// </summary>
		public virtual string PriceCurrency { get; set; }
		/// <summary>
		/// 重量，等于null时继续匹配下一项
		/// </summary>
		public virtual decimal? Weight { get; set; }
		/// <summary>
		/// 库存，等于null时继续匹配下一项
		/// </summary>
		public virtual long? Stock { get; set; }
		/// <summary>
		/// 匹配顺序，从小到大
		/// </summary>
		public virtual long MatchOrder { get; set; }
		/// <summary>
		/// 备注，纯文本
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public ProductMatchedData() {
			Conditions = new ProductMatchedDataConditions();
			Affects = new ProductMatchedDataAffects();
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<ProductMatchedData> builder) {
			builder.Id(d => d.Id);
			builder.References(d => d.Product);
			builder.Map(d => d.Conditions, new EntityMappingOptions() { WithSerialization = true });
			builder.Map(d => d.Affects, new EntityMappingOptions() { WithSerialization = true });
			builder.Map(d => d.Price);
			builder.Map(d => d.PriceCurrency);
			builder.Map(d => d.Weight);
			builder.Map(d => d.Stock);
			builder.Map(d => d.MatchOrder);
			builder.Map(d => d.Remark);
		}
	}
}
