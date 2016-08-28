using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Attributes;

namespace ZKWeb.Plugins.Common.Admin.src.UIComponents.Forms {
	/// <summary>
	/// 用户登录
	/// </summary>
	[Form("UserLoginForm", SubmitButtonText = "Login")]
	public class UserLoginForm : ModelFormBuilder {
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
		/// 验证码
		/// </summary>
		[Required]
		[CaptchaField("Captcha", "Common.Admin.UserLogin", "Please enter captcha")]
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
			var userManager = Application.Ioc.Resolve<UserManager>();
			RedirectAfterLogin = userManager.GetUrlRedirectAfterLogin();
		}

		/// <summary>
		/// 登陆用户
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var userManager = Application.Ioc.Resolve<UserManager>();
			userManager.Login(Username, Password, RememberLogin);
			return new { message = new T("Login successful") };
		}
	}
}
