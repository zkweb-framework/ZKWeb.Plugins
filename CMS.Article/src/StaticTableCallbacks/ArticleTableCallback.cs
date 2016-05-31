using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
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
			StaticTableSearchRequest request, List<KeyValuePair<Database.Article, Dictionary<string, object>>> pairs) {
			foreach (var pair in pairs) {
				var author = pair.Key.Author;
				var lastClass = pair.Key.Classes.OrderByDescending(c => c.Id).LastOrDefault();
				pair.Value["Id"] = pair.Key.Id;
				pair.Value["Title"] = pair.Key.Title;
				pair.Value["Summary"] = pair.Key.Summary;
				pair.Value["Author"] = author == null ? null : author.Username;
				pair.Value["AuthorId"] = author == null ? null : (long?)author.Id;
				pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
				pair.Value["LastClass"] = lastClass == null ? null : lastClass.Name;
				pair.Value["LastClassId"] = lastClass == null ? null : (long?)lastClass.Id;
				pair.Value["Tags"] = pair.Key.Tags.Select(t => new { t.Id, t.Name }).ToList();
			}
		}
	}
}
