using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.GenericClass.src.Database {
	/// <summary>
	/// 通用分类
	/// </summary>
	public class GenericClass {
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
	}

	/// <summary>
	/// 通用分类的数据库结构
	/// </summary>
	[ExportMany]
	public class GenericClassMap : ClassMap<GenericClass> {
		/// <summary>
		/// 初始化
		/// </summary>
		public GenericClassMap() {
			Id(c => c.Id);
			Map(c => c.Type).Index("Idx_Type");
			References(c => c.Parent).Nullable();
			Map(c => c.Name);
			Map(c => c.CreateTime);
			Map(c => c.DisplayOrder);
			Map(c => c.Remark).Length(0xffff);
			Map(c => c.Deleted);
		}
	}
}
