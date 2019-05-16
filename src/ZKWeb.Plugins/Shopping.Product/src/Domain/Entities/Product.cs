using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Entities;
using ZKWeb.Plugins.Common.GenericTag.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Domain.Entities {
	/// <summary>
	/// 商品
	/// </summary>
	[ExportMany]
	public class Product :
		IHaveCreateTime, IHaveUpdateTime, IHaveDeleted,
		IEntity<Guid>, IEntityMappingProvider<Product> {
		/// <summary>
		/// 商品Id
		/// </summary>
		public virtual Guid Id { get; set; }
		/// <summary>
		/// 商品类目，没有时等于null
		/// </summary>
		public virtual ProductCategory Category { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 商品介绍，格式是Html
		/// </summary>
		public virtual string Introduction { get; set; }
		/// <summary>
		/// 商品类型，必须是继承IProductType的类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 商品状态，必须是继承IProductState的状态
		/// </summary>
		public virtual string State { get; set; }
		/// <summary>
		/// 卖家，可以等于null
		/// </summary>
		public virtual User Seller { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdateTime { get; set; }
		/// <summary>
		/// 显示顺序
		/// </summary>
		public virtual long DisplayOrder { get; set; }
		/// <summary>
		/// 备注，格式是Html
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 关联的分类
		/// </summary>
		public virtual ISet<GenericClass> Classes { get; set; }
		/// <summary>
		/// 关联的标签
		/// </summary>
		public virtual ISet<GenericTag> Tags { get; set; }
		/// <summary>
		/// 关联的匹配数据
		/// </summary>
		public virtual ISet<ProductMatchedData> MatchedDatas { get; set; }
		/// <summary>
		/// 关联的属性值
		/// </summary>
		public virtual ISet<ProductToPropertyValue> PropertyValues { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Product() {
			DisplayOrder = 10000;
			Classes = new HashSet<GenericClass>();
			Tags = new HashSet<GenericTag>();
			MatchedDatas = new HashSet<ProductMatchedData>();
			PropertyValues = new HashSet<ProductToPropertyValue>();
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Name;
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<Product> builder) {
			builder.Id(p => p.Id);
			builder.References(p => p.Category, new EntityMappingOptions() {
				Index = "Idx_Product_Category"
			});
			builder.Map(p => p.Name);
			builder.Map(p => p.Introduction);
			builder.Map(p => p.Type, new EntityMappingOptions() {
				Nullable = false, Index = "Idx_Product_Type", Length = 255
			});
			builder.Map(p => p.State, new EntityMappingOptions() {
				Nullable = false, Index = "Idx_Product_State", Length = 255
			});
			builder.References(p => p.Seller);
			builder.Map(p => p.CreateTime);
			builder.Map(p => p.UpdateTime, new EntityMappingOptions() { Index = "Idx_Product_UpdateTime" });
			builder.Map(p => p.DisplayOrder);
			builder.Map(p => p.Remark);
			builder.Map(p => p.Deleted);
			builder.HasManyToMany(p => p.Classes);
			builder.HasManyToMany(p => p.Tags);
			builder.HasMany(p => p.MatchedDatas);
			builder.HasMany(p => p.PropertyValues);
		}
	}
}
