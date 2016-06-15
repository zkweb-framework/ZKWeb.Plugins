using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using ZKWeb.Database.Interfaces;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Demo.src.DatabaseInitializeHandlers {
	/// <summary>
	/// 全局处理表名
	/// </summary>
	[ExportMany]
	public class DatabaseInitializeHandlerDemo : IDatabaseInitializeHandler {
		/// <summary>
		/// 是否启用
		/// 启用后原有的数据表内容不会自动迁移到现有的数据表，请务必做好备份
		/// </summary>
		public static readonly bool Enabled = false;

		/// <summary>
		/// 数据库初始化时的处理
		/// </summary>
		/// <param name="configuration">配置对象</param>
		public void OnInitialize(FluentConfiguration configuration) {
			if (!Enabled) {
				return;
			}
			configuration.Mappings(m => {
				m.FluentMappings.Conventions.Add(
					ConventionBuilder.Class.Always(x =>
					x.Table(string.Format("demo_{0}", x.EntityType.Name.ToLower()))));
				m.FluentMappings.Conventions.Add(
					ConventionBuilder.HasManyToMany.Always(x =>
					x.Table(string.Format("demo_{0}_to_{1}",
						x.EntityType.Name.ToLower(),
						x.ChildType.Name.ToLower()))));
			});
		}
	}
}
