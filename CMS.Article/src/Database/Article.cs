using System;
using System.Collections.Generic;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.GenericClass.src.Database;
using ZKWeb.Plugins.Common.GenericTag.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Database {
	/// <summary>
	/// 文章
	/// </summary>
	[ExportMany]
	public class Article : IEntity<long>, IEntityMappingProvider<Article> {
		/// <summary>
		/// 文章Id
		/// </summary>
		public virtual long Id { get; set; }
		/// <summary>
		/// 文章标题
		/// </summary>
		public virtual string Title { get; set; }
		/// <summary>
		/// 文章摘要
		/// </summary>
		public virtual string Summary { get; set; }
		/// <summary>
		/// 文章内容，格式是Html
		/// </summary>
		public virtual string Contents { get; set; }
		/// <summary>
		/// 发布人
		/// </summary>
		public virtual User Author { get; set; }
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
		/// 初始化
		/// </summary>
		public Article() {
			DisplayOrder = 10000;
			Classes = new HashSet<GenericClass>();
			Tags = new HashSet<GenericTag>();
		}

		/// <summary>
		/// 显示名称
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Title;
		}

		/// <summary>
		/// 配置数据库结构
		/// </summary>
		public virtual void Configure(IEntityMappingBuilder<Article> builder) {
			builder.Id(a => a.Id);
			builder.Map(a => a.Title);
			builder.Map(a => a.Summary);
			builder.Map(a => a.Contents);
			builder.References(a => a.Author);
			builder.Map(a => a.CreateTime);
			builder.Map(a => a.LastUpdated);
			builder.Map(a => a.DisplayOrder);
			builder.Map(a => a.Remark);
			builder.Map(a => a.Deleted);
			builder.HasManyToMany(a => a.Classes);
			builder.HasManyToMany(a => a.Tags);
		}
	}
}
