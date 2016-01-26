using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.GenericTag.src.Database {
	/// <summary>
	/// 通用标签
	/// </summary>
	public class GenericTag {
		/// <summary>
		/// 标签Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 标签类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 标签名称
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
		public GenericTag() {
			// 设置默认显示顺序
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
	/// 通用标签的数据库结构
	/// </summary>
	[ExportMany]
	public class GenericTagMap : ClassMap<GenericTag> {
		/// <summary>
		/// 初始化
		/// </summary>
		public GenericTagMap() {
			Id(t => t.Id);
			Map(t => t.Type).Index("Idx_Type");
			Map(t => t.Name);
			Map(t => t.CreateTime);
			Map(t => t.DisplayOrder);
			Map(t => t.Remark).Length(0xffff);
			Map(t => t.Deleted);
		}
	}
}
