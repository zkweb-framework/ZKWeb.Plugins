using System.Collections.Generic;
using System.Linq;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Bases;
using System;

namespace ZKWeb.Plugins.CMS.Article.src.UIComponents.StaticTableHandlers {
	using Article = Domain.Entities.Article;

	/// <summary>
	/// 前台文章列表使用的文章表格的回调
	/// </summary>
	public class ArticleTableHandler :
		StaticTableHandlerBase<Article, Guid> {
		/// <summary>
		/// 过滤数据
		/// </summary>
		public override void OnQuery(
			StaticTableSearchRequest request, ref IQueryable<Article> query) {
			// 按分类
			var classId = request.Conditions.GetOrDefault<Guid?>("class");
			if (classId != null) {
				query = query.Where(q => q.Classes.Any(c => c.Id == classId));
			}
			// 按标签
			var tagId = request.Conditions.GetOrDefault<Guid?>("tag");
			if (tagId != null) {
				query = query.Where(q => q.Tags.Any(t => t.Id == tagId));
			}
			// 按关键词
			if (!string.IsNullOrEmpty(request.Keyword)) {
				query = query.Where(q =>
					q.Title.Contains(request.Keyword) ||
					q.Summary.Contains(request.Keyword) ||
					q.Author.Username == request.Keyword);
			}
			// 只显示未删除的文章
			query = query.Where(q => !q.Deleted);
		}

		/// <summary>
		/// 排序数据
		/// </summary>
		public override void OnSort(
			StaticTableSearchRequest request, ref IQueryable<Article> query) {
			// 默认先按显示顺序再按更新时间排序
			query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.UpdateTime);
		}

		/// <summary>
		/// 选择数据
		/// </summary>
		public override void OnSelect(
			StaticTableSearchRequest request, IList<EntityToTableRow<Article>> pairs) {
			foreach (var pair in pairs) {
				var author = pair.Entity.Author;
				var lastClass = pair.Entity.Classes.OrderByDescending(c => c.Id).LastOrDefault();
				pair.Row["Id"] = pair.Entity.Id;
				pair.Row["Title"] = pair.Entity.Title;
				pair.Row["Summary"] = pair.Entity.Summary;
				pair.Row["Author"] = author?.Username;
				pair.Row["AuthorId"] = author?.Id;
				pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
				pair.Row["LastClass"] = new T(lastClass?.Name).ToString();
				pair.Row["LastClassId"] = lastClass?.Id;
				pair.Row["Tags"] = pair.Entity.Tags.Select(t => new { t.Id, t.Name }).ToList();
			}
		}
	}
}
