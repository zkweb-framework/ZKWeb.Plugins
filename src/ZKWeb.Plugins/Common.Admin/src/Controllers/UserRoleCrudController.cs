using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Extensions;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// 角色管理
	/// 要求超级管理员
	/// </summary>
	[ExportMany]
	public class UserRoleCrudController : CrudAdminAppControllerBase<UserRole, Guid> {
		public override string Group { get { return "System Manage"; } }
		public override string GroupIconClass { get { return "fa fa-gear"; } }
		public override string Name { get { return "Role Manage"; } }
		public override string Url { get { return "/admin/roles"; } }
		public override string TileClass { get { return "tile bg-blue"; } }
		public override string IconClass { get { return "fa fa-legal"; } }
		public override Type RequiredUserType { get { return typeof(IAmSuperAdmin); } }
		protected override IAjaxTableHandler<UserRole, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<UserRole, Guid> {
			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<UserRoleCrudController>();
				searchBar.StandardSetupFor<UserRoleCrudController>("Name/Remark");
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<UserRole> query) {
				var keyword = request.Keyword;
				if (!string.IsNullOrEmpty(keyword)) {
					query = query.Where(r => r.Name.Contains(keyword) || r.Remark.Contains(keyword));
				}
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<UserRole>> pairs) {
				foreach (var pair in pairs) {
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["UpdateTime"] = pair.Entity.UpdateTime.ToClientTimeString();
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和批量操作
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<UserRoleCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Name", "45%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("UpdateTime");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupFor<UserRoleCrudController>(request);
			}
		}

		/// <summary>
		/// 添加和编辑表单
		/// </summary>
		public class Form : TabEntityFormBuilder<UserRole, Guid, Form> {
			/// <summary>
			/// 名称
			/// </summary>
			[Required]
			[StringLength(100, MinimumLength = 1)]
			[TextBoxField("Name", "Please enter name")]
			public string Name { get; set; }
			/// <summary>
			/// 权限
			/// </summary>
			[Required]
			[CheckBoxGroupsField("Privileges", typeof(PrivilegesListItemGroupsProvider))]
			public HashSet<string> Privileges { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[StringLength(10000, MinimumLength = 0)]
			[TextAreaField("Remark", 5, "Please enter remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(UserRole bindFrom) {
				Name = bindFrom.Name;
				Privileges = bindFrom.Privileges;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(UserRole saveTo) {
				saveTo.Name = Name;
				saveTo.Privileges = Privileges;
				saveTo.Remark = Remark;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
