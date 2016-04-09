using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Shopping.Logistics.src.Model;

namespace ZKWeb.Plugins.Shopping.Logistics.src.Database {
	/// <summary>
	/// 物流
	/// </summary>
	public class Logistics {
		/// <summary>
		/// 物流Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 物流名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 物流类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 物流费用计算规则
		/// </summary>
		public virtual List<PriceRule> PriceRules { get; set; }
		/// <summary>
		/// 所有人，没有时等于null
		/// </summary>
		public virtual User Owner { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateTime { get; set; }
		/// <summary>
		/// 最后更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
		/// <summary>
		/// 是否已删除
		/// </summary>
		public virtual bool Deleted { get; set; }
		/// <summary>
		/// 显示顺序，从小到大
		/// </summary>
		public virtual long DisplayOrder { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public Logistics() {
			PriceRules = new List<PriceRule>();
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
	/// 物流的数据库结构
	/// </summary>
	[ExportMany]
	public class LogisticsMap : ClassMap<Logistics> {
		/// <summary>
		/// 初始化
		/// </summary>
		public LogisticsMap() {
			Id(l => l.Id);
			Map(l => l.Name);
			Map(l => l.Type).Index("Idx_Type");
			Map(l => l.PriceRules).CustomType<JsonSerializedType<List<PriceRule>>>();
			References(l => l.Owner);
			Map(l => l.CreateTime);
			Map(l => l.LastUpdated);
			Map(l => l.Deleted);
			Map(l => l.DisplayOrder);
			Map(l => l.Remark).Length(0xffff);
		}
	}
}
