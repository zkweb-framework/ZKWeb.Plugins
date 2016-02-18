using DryIoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.ListItemProviders;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.Forms {
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
		[LabelField("Role")]
		public string Role { get; set; }
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
		public HttpPostedFileBase Avatar { get; set; }
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
			Username = user.Username;
			Role = user.Role == null ? new T("No Role") : user.Role.Name;
			SuperAdmin = new T(user.Type == UserTypes.SuperAdmin ? "Yes" : "No");
			Privileges = user.Role == null ? new HashSet<string>() : user.Role.Privileges;
			if (user.Type == UserTypes.SuperAdmin) {
				// 超级管理员时勾选所有权限
				var providers = Application.Ioc.ResolveMany<IPrivilegesProvider>();
				Privileges = new HashSet<string>(providers.SelectMany(p => p.GetPrivileges()));
			}
		}

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var userManager = Application.Ioc.Resolve<UserManager>();
			// 修改密码
			if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(Password)) {
				if (Password != ConfirmPassword) {
					throw new HttpException(400, new T("Please repeat the password exactly"));
				}
				userManager.ChangePassword(session.ReleatedId, OldPassword, Password);
			}
			// 修改头像
			if (Avatar != null) {
				userManager.SaveAvatar(session.ReleatedId, Avatar.InputStream);
			} else if (DeleteAvatar) {
				userManager.DeleteAvatar(session.ReleatedId);
			}
			return new {
				message = new T("Saved Successfully"),
				script = ScriptStrings.RefreshAfter(1500)
			};
		}
	}
}
