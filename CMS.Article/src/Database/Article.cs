using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.GenericClass.src.Database;
using ZKWeb.Plugins.Common.GenericTag.src.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Database {
	/// <summary>
	/// 文章
	/// </summary>
	public class Article {
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
	}

	/// <summary>
	/// 文章的数据库结构
	/// </summary>
	[ExportMany]
	public class ArticleMap : ClassMap<Article> {
		/// <summary>
		/// 初始化
		/// </summary>
		public ArticleMap() {
			Id(a => a.Id);
			Map(a => a.Title).Length(0xffff);
			Map(a => a.Summary).Length(0xffff);
			Map(a => a.Contents).Length(0xffff);
			References(a => a.Author);
			Map(a => a.CreateTime);
			Map(a => a.LastUpdated);
			Map(a => a.DisplayOrder);
			Map(a => a.Remark).Length(0xffff);
			Map(a => a.Deleted);
			HasManyToMany(a => a.Classes);
			HasManyToMany(a => a.Tags);
		}
	}
}
