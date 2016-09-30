using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Controllers;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Entities;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Services;
using ZKWeb.Plugins.Common.GenericClass.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Common.GenericTag.src.Domain.Entities;
using ZKWeb.Plugins.Common.GenericTag.src.Domain.Services;
using ZKWeb.Plugins.Common.GenericTag.src.UIComponents.ListItemProviders;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.CMS.Article.src.Controllers {
	using Article = Domain.Entities.Article;

	/// <summary>
	/// 文章管理
	/// </summary>
	[ExportMany]
	public class ArticleCrudController : CrudAdminAppControllerBase<Article, Guid> {
		public override string Group { get { return "CMS"; } }
		public override string GroupIconClass { get { return "fa fa-pencil"; } }
		public override string Name { get { return "ArticleManage"; } }
		public override string Url { get { return "/admin/articles"; } }
		public override string TileClass { get { return "tile bg-green"; } }
		public override string IconClass { get { return "fa fa-pencil"; } }
		protected override IAjaxTableHandler<Article, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 表格处理器
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<Article, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<ArticleCrudController>();
				searchBar.StandardSetupFor<ArticleCrudController>("Title/Summary/Author");
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<Article> query) {
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
			public override void OnSort(
				AjaxTableSearchRequest request, ref IQueryable<Article> query) {
				query = query.OrderByDescending(a => a.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<Article>> pairs) {
				foreach (var pair in pairs) {
					var author = pair.Entity.Author;
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Title"] = pair.Entity.Title;
					pair.Row["Author"] = author?.Username;
					pair.Row["AuthorId"] = author?.Id;
					pair.Row["ArticleClass"] = string.Join(", ", pair.Entity.Classes.Select(c => c.Name));
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["UpdateTime"] = pair.Entity.UpdateTime.ToClientTimeString();
					pair.Row["DisplayOrder"] = pair.Entity.DisplayOrder;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<ArticleCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Title", "25%");
				response.Columns.AddEditColumnFor<UserCrudController>("Author", "AuthorId");
				response.Columns.AddMemberColumn("ArticleClass");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("UpdateTime");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddButtonForOpenLink(new T("Preview"),
					"btn btn-xs btn-success", "fa fa-eye", "/article/view?id=<%-row.Id%>", "_blank");
				actionColumn.StandardSetupFor<ArticleCrudController>(request);
			}
		}

		/// <summary>
		/// 添加和编辑使用的表单
		/// </summary>
		public class Form : TabEntityFormBuilder<Article, Guid, Form> {
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
			[CheckBoxTreeField("ArticleClass",
				typeof(GenericClassListItemTreeProvider<ArticleClassController>))]
			public HashSet<Guid> ArticleClass { get; set; }
			/// <summary>
			/// 文章标签
			/// </summary>
			[CheckBoxGroupField("ArticleTag",
				typeof(GenericTagListItemProvider<ArticleTagController>))]
			public HashSet<Guid> ArticleTag { get; set; }
			/// <summary>
			/// 显示顺序
			/// </summary>
			[Required]
			[TextBoxField("DisplayOrder", "Order from small to large")]
			public long DisplayOrder { get; set; }
			/// <summary>
			/// 内容
			/// </summary>
			[RichTextEditor("Contents", ImageBrowserUrl = "/image_browser/article")]
			public string Contents { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[RichTextEditor("Remark", ImageBrowserUrl = "/image_browser/article", Group = "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind(Article bindFrom) {
				Title = bindFrom.Title;
				Summary = bindFrom.Summary;
				ArticleClass = new HashSet<Guid>(bindFrom.Classes.Select(c => c.Id));
				ArticleTag = new HashSet<Guid>(bindFrom.Tags.Select(t => t.Id));
				DisplayOrder = bindFrom.DisplayOrder;
				Contents = bindFrom.Contents;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			protected override object OnSubmit(Article saveTo) {
				if (saveTo.Author == null) {
					var sessionManager = Application.Ioc.Resolve<SessionManager>();
					var session = sessionManager.GetSession();
					var userManager = Application.Ioc.Resolve<UserManager>();
					saveTo.Author = userManager.Get(session.ReleatedId.Value);
				}
				saveTo.Title = Title;
				saveTo.Summary = Summary;
				var classManager = Application.Ioc.Resolve<GenericClassManager>();
				var tagManager = Application.Ioc.Resolve<GenericTagManager>();
				saveTo.Classes = new HashSet<GenericClass>(
					classManager.GetMany(c => ArticleClass.Contains(c.Id)));
				saveTo.Tags = new HashSet<GenericTag>(
					tagManager.GetMany(t => ArticleTag.Contains(t.Id)));
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Contents = Contents;
				saveTo.Remark = Remark;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
