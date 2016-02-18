using DryIocAttributes;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKWeb.Plugins.Common.Base.src.Database {
	/// <summary>
	/// 通用配置
	/// </summary>
	public class GenericConfig {
		/// <summary>
		/// 主键，没有意义
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 所属插件 + 配置键名
		/// </summary>
		public virtual string Key { get; set; }
		/// <summary>
		/// 配置值（json）
		/// </summary>
		public virtual string Value { get; set; }
		/// <summary>
		/// 最后更新时间
		/// </summary>
		public virtual DateTime LastUpdated { get; set; }
	}

	/// <summary>
	/// 通用配置的数据库结构
	/// </summary>
	[ExportMany]
	public class GenericConfigMap : ClassMap<GenericConfig> {
		/// <summary>
		/// 初始化
		/// key在sqlserver中是关键字，需要改名
		/// </summary>
		public GenericConfigMap() {
			Id(c => c.Id);
			Map(c => c.Key).Column("key_").Length(255).Unique();
			Map(c => c.Value).Length(0xffff);
			Map(c => c.LastUpdated);
		}
	}
}
