using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.CMS.Article.src.StaticTableCallbacks {
	/// <summary>
	/// 前台文章列表使用的文章表格的回调
	/// </summary>
	public class ArticleTableCallback : IStaticTableCallback<Database.Article> {
		/// <summary>
		/// 过滤数据
		/// </summary>
		public void OnQuery(
			StaticTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.Article> query) {
			// 按分类
			var classId = request.Conditions.GetOrDefault<long?>("class");
			if (classId != null) {
				query = query.Where(q => q.Classes.Any(c => c.Id == classId));
			}
			// 按标签
			var tagId = request.Conditions.GetOrDefault<long?>("tag");
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
		public void OnSort(
			StaticTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.Article> query) {
			// 默认先按显示顺序再按更新时间排序
			query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.LastUpdated);
		}

		/// <summary>
		/// 选择数据
		/// </summary>
		public void OnSelect(
			StaticTableSearchRequest request, List<EntityToTableRow<Database.Article>> pairs) {
			foreach (var pair in pairs) {
				var author = pair.Entity.Author;
				var lastClass = pair.Entity.Classes.OrderByDescending(c => c.Id).LastOrDefault();
				pair.Row["Id"] = pair.Entity.Id;
				pair.Row["Title"] = pair.Entity.Title;
				pair.Row["Summary"] = pair.Entity.Summary;
				pair.Row["Author"] = author == null ? null : author.Username;
				pair.Row["AuthorId"] = author == null ? null : (long?)author.Id;
				pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
				pair.Row["LastClass"] = lastClass == null ? null : new T(lastClass.Name).ToString();
				pair.Row["LastClassId"] = lastClass == null ? null : (long?)lastClass.Id;
				pair.Row["Tags"] = pair.Entity.Tags.Select(t => new { t.Id, t.Name }).ToList();
			}
		}
	}
}
