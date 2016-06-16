using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Captcha.src.FormFieldAttributes;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Admin.src.Forms {
	/// <summary>
	/// 用户注册
	/// </summary>
	[Form("UserRegForm", SubmitButtonText = "Register")]
	public class UserRegForm : ModelFormBuilder {
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
		/// 验证码
		/// </summary>
		[Required]
		[CaptchaField("Captcha", "Common.Admin.UserReg", "Please enter captcha")]
		public string Captcha { get; set; }

		/// <summary>
		/// 绑定
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 注册用户，成功后自动登陆
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			if (Password != ConfirmPassword) {
				throw new HttpException(400, new T("Please repeat the password exactly"));
			}
			var userManager = Application.Ioc.Resolve<UserManager>();
			userManager.Reg(Username, Password);
			userManager.Login(Username, Password, false);
			return new { message = new T("You have registered successfully, thanks for you registration") };
		}
	}
}
