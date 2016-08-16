using System;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.GenericClass.src.Database {
	/// <summary>
	/// 通用分类
	/// </summary>
	[ExportMany]
	public class GenericClass : IEntity<long>, IEntityMappingProvider<GenericClass> {
		/// <summary>
		/// 分类Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 分类类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 上级分类，根节点时等于null
		/// </summary>
		public virtual GenericClass Parent { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 显示顺序，从小到大
		/// </summary>
		public virtual long DisplayOrder { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public GenericClass() {
			DisplayOrder = 10000;
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
		public virtual void Configure(IEntityMappingBuilder<GenericClass> builder) {
			builder.Id(c => c.Id);
			builder.Map(c => c.Type, new EntityMappingOptions() { Index = "Idx_Type" });
			builder.References(c => c.Parent, new EntityMappingOptions() { Nullable = true });
			builder.Map(c => c.Name);
			builder.Map(c => c.CreateTime);
			builder.Map(c => c.DisplayOrder);
			builder.Map(c => c.Remark);
			builder.Map(c => c.Deleted);
		}
	}
}
