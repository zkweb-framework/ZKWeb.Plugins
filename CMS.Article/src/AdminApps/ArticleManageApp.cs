using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Database;
using ZKWeb.Plugins.CMS.Article.src.Database;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Localize;
using ZKWeb.Utils.Extensions;
using ZKWeb.Plugins.Common.Admin.src.AdminApps;

namespace ZKWeb.Plugins.CMS.Article.src.AdminApps {
	/// <summary>
	/// 文章管理
	/// </summary>
	[ExportMany]
	public class ArticleManageApp : AdminAppBuilder<Database.Article, ArticleManageApp> {
		public override string Name { get { return "ArticleManage"; } }
		public override string Url { get { return "/admin/articles"; } }
		public override string TileClass { get { return "tile bg-blue"; } }
		public override string IconClass { get { return "fa fa-pencil"; } }
		protected override IAjaxTableCallback<Database.Article> GetTableCallback() { return new TableCallback(); }
		protected override IModelFormBuilder GetAddForm() { throw new NotImplementedException(); }
		protected override IModelFormBuilder GetEditForm() { throw new NotImplementedException(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<Database.Article> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditActionForAdminApp<ArticleManageApp>();
				table.MenuItems.AddAddActionForAdminApp<ArticleManageApp>();
				searchBar.KeywordPlaceHolder = new T("Title/Summary/Author");
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddActionForAdminApp<ArticleManageApp>();
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.Article> query) {
				// 按回收站
				query = query.FilterByRecycleBin(request);
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Title.Contains(request.Keyword) ||
						q.Summary.Contains(request.Keyword) ||
						q.Author.Username == request.Keyword);
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<Database.Article> query) {
				query = query.OrderByDescending(a => a.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<Database.Article, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					var author = pair.Key.Author;
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Title"] = pair.Key.Title;
					pair.Value["Author"] = author == null ? null : author.Username;
					pair.Value["AuthorId"] = author == null ? null : (long?)author.Id;
					pair.Value["ArticleClass"] = string.Join(", ", pair.Key.Classes.Select(c => c.Name));
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
					pair.Value["LastUpdated"] = pair.Key.LastUpdated.ToClientTimeString();
					pair.Value["DisplayOrder"] = pair.Key.DisplayOrder;
					pair.Value["Deleted"] = pair.Key.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Title", "45%");
				response.Columns.AddEditColumnForAdminApp<UserManageApp>("Author", "AuthorId");
				response.Columns.AddMemberColumn("ArticleClass");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn("150");
				actionColumn.AddEditActionForAdminApp<ArticleManageApp>();
				idColumn.AddDivider();
				idColumn.AddDeleteActionsForAdminApp<ArticleManageApp>(request);
			}
		}
	}
}
