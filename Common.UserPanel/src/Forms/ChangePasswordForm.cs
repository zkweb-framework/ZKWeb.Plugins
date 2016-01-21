using DryIoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.UserPanel.src.Forms {
	/// <summary>
	/// 修改密码的表单
	/// </summary>
	public class ChangePasswordForm : ModelFormBuilder {
		/// <summary>
		/// 原密码
		/// </summary>
		[Required]
		[StringLength(100, MinimumLength = 5)]
		[PasswordField("OldPassword", "Please enter old password")]
		public string OldPassword { get; set; }
		/// <summary>
		/// 密码
		/// </summary>
		[Required]
		[StringLength(100, MinimumLength = 5)]
		[PasswordField("Password", "Please enter password")]
		public string Password { get; set; }
		/// <summary>
		/// 确认密码
		/// </summary>
		[Required]
		[StringLength(100, MinimumLength = 5)]
		[PasswordField("ConfirmPassword", "Please repeat the password exactly")]
		public string ConfirmPassword { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			if (Password != ConfirmPassword) {
				throw new HttpException(400, new T("Please repeat the password exactly"));
			}
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var userManager = Application.Ioc.Resolve<UserManager>();
			userManager.ChangePassword(session.ReleatedId, OldPassword, Password);
			return new { message = new T("Saved Successfully") };
		}
	}
}
