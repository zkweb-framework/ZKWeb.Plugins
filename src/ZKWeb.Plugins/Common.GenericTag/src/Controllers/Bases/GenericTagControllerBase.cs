using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.HtmlItems.Extensions;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.AdminSettings.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
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
using ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems.Extensions;
using ZKWeb.Plugins.Common.GenericTag.src.Domain.Services;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.GenericTag.src.Controllers.Bases {
	using GenericTag = Domain.Entities.GenericTag;

	/// <summary>
	/// 通用标签构建器
	/// </summary>
	public abstract class GenericTagControllerBase :
		CrudAdminSettingsControllerBase<GenericTag, Guid> {
		public virtual string Type { get { return Name.Replace(" ", ""); } }
		public override string Group { get { return "TagManage"; } }
		public override string GroupIconClass { get { return "fa fa-tags"; } }
		public override string IconClass { get { return "fa fa-tags"; } }
		public override string Url { get { return "/admin/settings/generic_tag/" + Type.ToLower(); } }
		public override string[] ViewPrivileges { get { return new[] { "TagManage:" + Type }; } }
		public override string[] EditPrivileges { get { return ViewPrivileges; } }
		public override string[] DeletePrivileges { get { return ViewPrivileges; } }
		public override string[] DeleteForeverPrivileges { get { return ViewPrivileges; } }
		protected override IAjaxTableHandler<GenericTag, Guid> GetTableHandler() { return new TableHandler(this); }
		protected override IModelFormBuilder GetAddForm() { return new Form(Type); }
		protected override IModelFormBuilder GetEditForm() { return new Form(Type); }

		/// <summary>
		/// 获取批量操作的数据Id列表
		/// </summary>
		/// <returns></returns>
		protected override IList<Guid> GetBatchActionIds() {
			// 检查是否所有Id都属于指定的类型，防止越权操作
			var ids = base.GetBatchActionIds();
			var genericTagManager = Application.Ioc.Resolve<GenericTagManager>();
			if (!genericTagManager.IsAllTagsTypeEqualTo(ids, Type)) {
				throw new ForbiddenException(new T("Try to access tag that type not matched"));
			}
			return ids;
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<GenericTag, Guid> {
			/// <summary>
			/// 标签控制器
			/// </summary>
			public GenericTagControllerBase Controller { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			/// <param name="controller">标签控制器</param>
			public TableHandler(GenericTagControllerBase controller) {
				Controller = controller;
			}

			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditAction(
					Controller.Type, Controller.EditUrl, dialogParameters: new { size = "size-wide" });
				table.MenuItems.AddAddAction(
					Controller.Type, Controller.AddUrl, dialogParameters: new { size = "size-wide" });
				searchBar.KeywordPlaceHolder = "Name/Remark";
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddAction(
					Controller.Type, Controller.AddUrl, dialogParameters: new { size = "size-wide" });
				searchBar.BeforeItems.AddAddAction(
					Controller.Type, Controller.AddUrl, dialogParameters: new { size = "size-wide" });
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<GenericTag> query) {
				// 在第一页显示所有分类
				request.PageNo = 0;
				request.PageSize = 0x7ffffffe;
				// 提供类型给其他回调
				request.Conditions["Type"] = Controller.Type;
				// 按类型
				query = query.Where(q => q.Type == Controller.Type);
				// 按关键词
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q => q.Name.Contains(request.Keyword) || q.Remark.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public override void OnSort(
				AjaxTableSearchRequest request, ref IQueryable<GenericTag> query) {
				// 默认按显示顺序排列
				query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择字段
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<GenericTag>> pairs) {
				foreach (var pair in pairs) {
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["DisplayOrder"] = pair.Entity.DisplayOrder;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Name", "45%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddEditAction(
					Controller.Type, Controller.EditUrl, dialogParameters: new { size = "size-wide" });
				idColumn.AddDivider();
				idColumn.AddDeleteActions(
					request, typeof(GenericTag), Controller.Type, Controller.BatchUrl);
			}
		}

		/// <summary>
		/// 添加和编辑使用的表单
		/// </summary>
		public class Form : EntityFormBuilder<GenericTag, Guid, Form> {
			/// <summary>
			/// 标签类型
			/// </summary>
			public string Type { get; set; }
			/// <summary>
			/// 名称
			/// </summary>
			[Required]
			[StringLength(100)]
			[TextBoxField("Name", "Name")]
			public string Name { get; set; }
			/// <summary>
			/// 显示顺序
			/// </summary>
			[Required]
			[TextBoxField("DisplayOrder", "Order from small to large")]
			public long DisplayOrder { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[TextAreaField("Remark", 5, "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			/// <param name="type">标签类型</param>
			public Form(string type) {
				Type = type;
			}

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(GenericTag bindFrom) {
				if (bindFrom.Type != null && bindFrom.Type != Type) {
					// 检查类型，防止越权操作
					throw new ForbiddenException(new T("Try to access tag that type not matched"));
				}
				Name = bindFrom.Name;
				DisplayOrder = bindFrom.DisplayOrder;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(GenericTag saveTo) {
				if (saveTo.Type == null) {
					// 添加时
					saveTo.Type = Type;
					saveTo.CreateTime = DateTime.UtcNow;
				} else if (saveTo.Type != null && saveTo.Type != Type) {
					// 编辑时检查类型，防止越权操作
					throw new ForbiddenException(new T("Try to access tag that type not matched"));
				}
				saveTo.Name = Name;
				saveTo.DisplayOrder = DisplayOrder;
				saveTo.Remark = Remark;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
