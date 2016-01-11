using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Core;
using ZKWeb.Utils.Extensions;
using ZKWeb.Plugins.Common.Base.src.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.AdminApps {
	/// <summary>
	/// 管理员管理
	/// 要求超级管理员
	/// </summary>
	[ExportMany]
	public class AdminManageApp : AdminAppBuilder<User, AdminManageApp> {
		public override string Name { get { return "Admin Manage"; } }
		public override string Url { get { return "/admin/admins"; } }
		public override string TileClass { get { return "tile bg-blue-hoki"; } }
		public override string IconClass { get { return "fa fa-user-secret"; } }
		public override UserTypes[] AllowedUserTypes { get { return new[] { UserTypes.SuperAdmin }; } }
		protected override IAjaxTableCallback<User> GetTableCallback() { return new TableCallback(); }
		protected override FormBuilder GetAddForm() { return new FormBuilder(); }
		protected override FormBuilder GetEditForm() { return new FormBuilder(); }
		
		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<User> {
			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public void OnBuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				searchBar.KeywordPlaceHolder = new T("Username");
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<User> query) {
				query = query.Where(u => AdminManager.AdminTypes.Contains(u.Type));
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(u => u.Username.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<User> query) {
				query = query.OrderByDescending(u => u.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<User, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					var role = pair.Key.Role;
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Username"] = pair.Key.Username;
					pair.Value["Role"] = role == null ? null : role.Name;
					pair.Value["RoleId"] = role == null ? null : (long?)role.Id;
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
					pair.Value["SuperAdmin"] = (
						pair.Key.Type == UserTypes.SuperAdmin ? EnumBool.True : EnumBool.False);
					pair.Value["Deleted"] = pair.Key.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和批量操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Username", "45%");
				response.Columns.AddMemberColumn("Role");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddEnumLabelColumn("SuperAdmin", typeof(EnumBool));
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn();

				actionColumn.AddRemoteModalForBelongedRow(
					"View", "btn btn-xs default", "fa fa-edit",
					"Edit Admin", "/admin_admins/edit/<%-row.Id%>");

				idColumn.AddItemForClickEvent("Test", "fa fa-check", "alert('asdasdas')");
				
				// idColumn.AddBatchRemoveAction();
				// idColumn.AddBatchRemoveForeverAction();
				// idColumn.AddBatchRecoverAction();
			}
		}
	}
}
