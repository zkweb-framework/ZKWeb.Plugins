using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.UserPanel.src.Model;
using ZKWeb.Plugins.Common.UserPanel.src.Scaffolding;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.UserPanel.src.GenericFormsForUserPanel {
	/// <summary>
	/// 修改自身密码的表单
	/// </summary>
	[ExportMany]
	public class ChangePasswordForm : GenericFormForUserPanel {
		public override string Group { get { return "Account Manage"; } }
		public override string GroupIcon { get { return "fa fa-user"; } }
		public override string Name { get { return "Change Password"; } }
		public override string IconClass { get { return "fa fa-lock"; } }
		public override string Url { get { return "/home/change_password"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
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
}
