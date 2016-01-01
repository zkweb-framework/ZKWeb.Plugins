using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Admin.src.Forms {
	/// <summary>
	/// 用户注册
	/// </summary>
	[Form("UserRegForm", SubmitButtonText = "Register")]
	public class UserRegForm : ModelFormBuilder {
		[Required]
		[StringLength(100, MinimumLength = 3)]
		[TextBoxField("Username", "Please enter username")]
		public string Username { get; set; }
		[Required]
		[StringLength(100, MinimumLength = 5)]
		[PasswordField("Password", "Please enter password")]
		public string Password { get; set; }
		[Required]
		[StringLength(100, MinimumLength = 5)]
		[PasswordField("ConfirmPassword", "Please repeat the password exactly")]
		public string ConfirmPassword { get; set; }
		[Required]
		[CaptchaField("Captcha", "Common.Admin.UserReg", "Please enter captcha")]
		public string Captcha { get; set; }

		/// <summary>
		/// 绑定
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			throw new NotImplementedException();
		}
	}
}
