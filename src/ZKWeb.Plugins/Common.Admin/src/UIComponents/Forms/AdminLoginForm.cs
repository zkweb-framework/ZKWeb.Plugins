using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes;

namespace ZKWeb.Plugins.Common.Admin.src.UIComponents.Forms {
	/// <summary>
	/// 管理员登陆
	/// </summary>
	[Form("AdminLoginForm", SubmitButtonText = "Login")]
	public class AdminLoginForm : ModelFormBuilder {
		/// <summary>
		/// 用户名
		/// </summary>
		[Required]
		[TextBoxField("Username", "Please enter username")]
		public string Username { get; set; }
		/// <summary>
		/// 密码
		/// </summary>
		[Required]
		[PasswordField("Password", "Please enter password")]
		public string Password { get; set; }
		/// <summary>
		/// 验证码
		/// </summary>
		[Required]
		[CaptchaField("Captcha", "Common.Admin.AdminLogin", "Please enter captcha")]
		public string Captcha { get; set; }
		/// <summary>
		/// 记住登陆
		/// </summary>
		[CheckBoxField("RememberLogin")]
		public bool RememberLogin { get; set; }
		/// <summary>
		/// 登陆后跳转到的地址
		/// </summary>
		[HiddenField("RedirectAfterLogin")]
		public string RedirectAfterLogin { get; set; }

		/// <summary>
		/// 绑定
		/// </summary>
		protected override void OnBind() {
			var adminManager = Application.Ioc.Resolve<AdminManager>();
			RedirectAfterLogin = adminManager.GetUrlRedirectAfterLogin();
		}

		/// <summary>
		/// 提交
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var adminManager = Application.Ioc.Resolve<AdminManager>();
			adminManager.Login(Username, Password, RememberLogin);
			return new { message = new T("Login successful") };
		}
	}
}
