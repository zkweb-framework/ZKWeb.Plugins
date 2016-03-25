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
using ZKWeb.Plugins.UI.CKEditor.src.FormFieldAttributes;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Base.src.Managers;
using DryIoc;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.GenericClass.src.Database;
using ZKWeb.Plugins.Common.GenericTag.src.Database;
using ZKWeb.Plugins.CMS.Article.src.GenericClasses;
using ZKWeb.Plugins.Common.GenericClass.src.ListItemProviders;
using ZKWeb.Plugins.CMS.Article.src.GenericTags;
using ZKWeb.Plugins.Common.GenericTag.src.ListItemProvider;

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
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

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
				response.Columns.AddMemberColumn("Title", "25%");
				response.Columns.AddEditColumnForAdminApp<UserManageApp>("Author", "AuthorId");
				response.Columns.AddMemberColumn("ArticleClass");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddEditActionForAdminApp<ArticleManageApp>();
				idColumn.AddDivider();
				idColumn.AddDeleteActionsForAdminApp<ArticleManageApp>(request);
			}
		}

		/// <summary>
		/// 添加和编辑使用的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<Database.Article, Form> {
			/// <summary>
			/// 标题
			/// </summary>
			[Required]
			[StringLength(255, MinimumLength = 1)]
			[TextBoxField("Title", "Title")]
			public string Title { get; set; }
			/// <summary>
			/// 摘要
			/// </summary>
			[StringLength(255, MinimumLength = 1)]
			[TextBoxField("Summary", "Summary")]
			public string Summary { get; set; }
			/// <summary>
			/// 文章分类
			/// </summary>
			[CheckBoxTreeField("ArticleClass", typeof(GenericClassListItemTreeProvider<ArticleClass>))]
			public HashSet<long> ArticleClass { get; set; }
			/// <summary>
			/// 文章标签
			/// </summary>
			[CheckBoxGroupField("ArticleTag", typeof(GenericTagListItemProvider<ArticleTag>))]
			public HashSet<long> ArticleTag { get; set; }
			/// <summary>
			/// 显示顺序
			/// </summary>
			[Required]
			[TextBoxField("DisplayOrder", "Order from small to large")]
			public long DisplayOrder { get; set; }
			/// <summary>
			/// 内容
			/// </summary>
			[CKEditor("Contents")]
			public string Contents { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[CKEditor("Remark", Group = "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, Database.Article bindFrom) {
				Title = bindFrom.Title;
				Summary = bindFrom.Summary;
				ArticleClass = new HashSet<long>(bindFrom.Classes.Select(c => c.Id));
				ArticleTag = new HashSet<long>(bindFrom.Tags.Select(t => t.Id));
				DisplayOrder = bindFrom.DisplayOrder;
				Contents = bindFrom.Contents;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, Database.Article saveTo) {
				if (saveTo.Id <= 0) {
					var sessionManager = Application.Ioc.Resolve<SessionManager>();
					var session = sessionManager.GetSession();
					var userRepository = RepositoryResolver.Resolve<User>(context);
					saveTo.Author = userRepository.GetById(session.ReleatedId);
					saveTo.CreateTime = DateTime.UtcNow;
				}
				saveTo.Title = Title;
				saveTo.Summary = Summary;
				var classRepository = RepositoryResolver.Resolve<GenericClass>(context);
				var tagRepository = RepositoryResolver.Resolve<GenericTag>(context);
				saveTo.Classes = new HashSet<GenericClass>(classRepository.GetMany(c => ArticleClass.Contains(c.Id)));
				saveTo.Tags = new HashSet<GenericTag>(tagRepository.GetMany(t => ArticleTag.Contains(t.Id)));
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Contents = Contents;
				saveTo.Remark = Remark;
				saveTo.LastUpdated = DateTime.UtcNow;
				return new {
					message = new T("Saved Successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}
		}
	}
}
