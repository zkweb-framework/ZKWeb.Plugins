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
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
		protected override IModelFormBuilder GetAddForm() { return new Form(); }
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<User> {
			/// <summary>
			/// 构建表格时的处理
			/// </summary>
			public void OnBuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddAddActionForAdminApp<AdminManageApp>();
				searchBar.KeywordPlaceHolder = new T("Username");
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddAddActionForAdminApp<AdminManageApp>();
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
				actionColumn.AddEditActionForAdminApp<AdminManageApp>(titleTemplate: new T("Edit Admin"));

				idColumn.AddItemForClickEvent("Test", "fa fa-check", "alert('asdasdas')");

				// idColumn.AddBatchRemoveAction();
				// idColumn.AddBatchRemoveForeverAction();
				// idColumn.AddBatchRecoverAction();
			}
		}

		/// <summary>
		/// 添加和编辑表单
		/// </summary>
		public class Form : DataEditFormBuilder<User, Form> {
			/// <summary>
			/// 用户名
			/// </summary>
			[Required]
			[StringLength(100, MinimumLength = 3)]
			[TextBoxField("Username", "Please enter username")]
			public string Username { get; set; }
			/// <summary>
			/// 密码
			/// </summary>
			[StringLength(100, MinimumLength = 5)]
			[PasswordField("Password", "Keep empty if you don't want to change")]
			public string Password { get; set; }
			/// <summary>
			/// 确认密码
			/// </summary>
			[StringLength(100, MinimumLength = 5)]
			[PasswordField("ConfirmPassword", "Keep empty if you don't want to change")]
			public string ConfirmPassword { get; set; }
			/// <summary>
			/// 是否超级管理员，TODO:需要添加检查
			/// </summary>
			[CheckBoxField("SuperAdmin")]
			public bool IsSuperAdmin { get; set; }
			/// <summary>
			/// 角色，TODO:未实现
			/// </summary>
			public string Role { get; set; }

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(User bindFrom) {
				Username = bindFrom.Username;
				Password = null;
				IsSuperAdmin = bindFrom.Type == UserTypes.SuperAdmin;
				Role = bindFrom.Role == null ? null : bindFrom.Role.Id.ToString();
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(User saveTo) {
				saveTo.Username = Username;
				if (!string.IsNullOrEmpty(Password)) {
					if (Password != ConfirmPassword) {
						throw new HttpException(400, new T("Please repeat the password exactly"));
					}
					saveTo.SetPassword(Password);
				}
				saveTo.Type = IsSuperAdmin ? UserTypes.SuperAdmin : UserTypes.Admin;
				saveTo.Role = null;
				return new { message = new T("Successfully Saved") };
			}
		}
	}
}
