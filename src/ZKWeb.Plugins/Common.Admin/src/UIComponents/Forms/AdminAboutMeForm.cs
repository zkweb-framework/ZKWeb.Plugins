using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.ListItemProviders;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Admin.src.UIComponents.Forms {
	/// <summary>
	/// 后台"关于我"的表单
	/// </summary>
	public class AdminAboutMeForm : TabModelFormBuilder {
		/// <summary>
		/// 用户名
		/// </summary>
		[LabelField("Username")]
		public string Username { get; set; }
		/// <summary>
		/// 角色
		/// </summary>
		[LabelField("Roles")]
		public string Roles { get; set; }
		/// <summary>
		/// 超级管理员
		/// </summary>
		[LabelField("SuperAdmin")]
		public string SuperAdmin { get; set; }
		/// <summary>
		/// 权限
		/// </summary>
		[CheckBoxGroupsField("Privileges", typeof(PrivilegesListItemGroupsProvider))]
		public HashSet<string> Privileges { get; set; }
		/// <summary>
		/// 原密码
		/// </summary>
		[StringLength(100, MinimumLength = 5)]
		[PasswordField("OldPassword", "Keep empty if you don't want to change", Group = "Change Password")]
		public string OldPassword { get; set; }
		/// <summary>
		/// 密码
		/// </summary>
		[StringLength(100, MinimumLength = 5)]
		[PasswordField("Password", "Keep empty if you don't want to change", Group = "Change Password")]
		public string Password { get; set; }
		/// <summary>
		/// 确认密码
		/// </summary>
		[StringLength(100, MinimumLength = 5)]
		[PasswordField("ConfirmPassword", "Keep empty if you don't want to change", Group = "Change Password")]
		public string ConfirmPassword { get; set; }
		/// <summary>
		/// 头像
		/// </summary>
		[FileUploaderField("Avatar", Group = "Change Avatar")]
		public IHttpPostedFile Avatar { get; set; }
		/// <summary>
		/// 删除头像
		/// </summary>
		[CheckBoxField("DeleteAvatar", Group = "Change Avatar")]
		public bool DeleteAvatar { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var isSuperAdmin = user.GetUserType() is IAmSuperAdmin;
			Username = user.Username;
			Roles = string.Join(", ", user.Roles.Select(r => r.Name));
			SuperAdmin = new T(isSuperAdmin ? "Yes" : "No");
			Privileges = new HashSet<string>(user.Roles.SelectMany(r => r.Privileges));
			if (isSuperAdmin) {
				// 超级管理员时勾选所有权限
				var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
				Privileges = new HashSet<string>(privilegeManager.GetPrivileges());
			}
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var userId = session.ReleatedId.Value;
			var userManager = Application.Ioc.Resolve<UserManager>();
			// 修改密码
			if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(Password)) {
				if (Password != ConfirmPassword) {
					throw new BadRequestException(new T("Please repeat the password exactly"));
				}
				userManager.ChangePassword(userId, OldPassword, Password);
			}
			// 修改头像
			if (Avatar != null) {
				userManager.SaveAvatar(userId, Avatar.OpenReadStream());
			} else if (DeleteAvatar) {
				userManager.DeleteAvatar(userId);
			}
			return this.SaveSuccessAndRefreshPage(1500);
		}
	}
}
