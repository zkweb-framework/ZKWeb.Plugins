using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.GenericClass.src.Database;
using ZKWeb.Plugins.Common.GenericTag.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Product.src.Database {
	/// <summary>
	/// 商品
	/// </summary>
	public class Product {
		/// <summary>
		/// 商品Id
		/// </summary>
		public virtual long Id { get; set; }
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
		public virtual DateTime LastUpdated { get; set; }
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
	}

	/// <summary>
	/// 商品的数据库结构
	/// </summary>
	[ExportMany]
	public class ProductMap : ClassMap<Product> {
		/// <summary>
		/// 初始化
		/// </summary>
		public ProductMap() {
			Id(p => p.Id);
			References(p => p.Category).Index("Idx_Category");
			Map(p => p.Name).Length(0xffff);
			Map(p => p.Introduction).Length(0xffff);
			Map(p => p.Type).Not.Nullable().Index("Idx_Type");
			Map(p => p.State).Not.Nullable().Index("Idx_State");
			References(p => p.Seller);
			Map(p => p.CreateTime);
			Map(p => p.LastUpdated);
			Map(p => p.DisplayOrder);
			Map(p => p.Remark).Length(0xffff);
			Map(p => p.Deleted);
			HasManyToMany(p => p.Classes);
			HasManyToMany(p => p.Tags);
			HasMany(p => p.MatchedDatas).Cascade.AllDeleteOrphan();
			HasMany(p => p.PropertyValues).Cascade.AllDeleteOrphan();
		}
	}
}
