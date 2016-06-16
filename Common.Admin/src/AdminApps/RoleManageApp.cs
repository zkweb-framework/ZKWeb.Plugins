using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Extensions;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Admin.src.ListItemProviders;
using Newtonsoft.Json;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Localize;
using ZKWeb.Database;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.AdminApps {
	/// <summary>
	/// 角色管理
	/// 要求超级管理员
	/// </summary>
	[ExportMany]
	public class RoleManageApp : AdminAppBuilder<UserRole> {
		public override string Name { get { return "Role Manage"; } }
		public override string Url { get { return "/admin/roles"; } }
		public override string TileClass { get { return "tile bg-blue"; } }
		public override string IconClass { get { return "fa fa-legal"; } }
		public override UserTypes[] AllowedUserTypes { get { return new[] { UserTypes.SuperAdmin }; } }
		protected override IAjaxTableCallback<UserRole> GetTableCallback() { return new SearchCallback(); }
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class SearchCallback : IAjaxTableCallback<UserRole> {
			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public void OnBuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupForCrudPage<RoleManageApp>();
				searchBar.StandardSetupForCrudPage<RoleManageApp>("Name/Remark");
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<UserRole> query) {
				query = query.FilterByRecycleBin(request);
				var keyword = request.Keyword;
				if (!string.IsNullOrEmpty(keyword)) {
					query = query.Where(r => r.Name.Contains(keyword) || r.Remark.Contains(keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<UserRole> query) {
				query = query.OrderByDescending(r => r.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<EntityToTableRow<UserRole>> pairs) {
				foreach (var pair in pairs) {
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Name"] = pair.Entity.Name;
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["LastUpdated"] = pair.Entity.LastUpdated.ToClientTimeString();
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和批量操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupForCrudPage<RoleManageApp>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Name", "45%");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				response.Columns.AddActionColumn().StandardSetupForCrudPage<RoleManageApp>(request);
			}
		}

		/// <summary>
		/// 添加和编辑表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<UserRole, Form> {
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
			protected override void OnBind(DatabaseContext context, UserRole bindFrom) {
				Name = bindFrom.Name;
				Privileges = bindFrom.Privileges;
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, UserRole saveTo) {
				saveTo.Name = Name;
				saveTo.Privileges = Privileges;
				saveTo.Remark = Remark;
				if (saveTo.Id <= 0) {
					saveTo.CreateTime = DateTime.UtcNow;
				}
				saveTo.LastUpdated = DateTime.UtcNow;
				return new {
					message = new T("Saved Successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}
		}
	}
}
