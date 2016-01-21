using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Model;
using ZKWeb.Model.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.UserPanel.src.Forms;

namespace ZKWeb.Plugins.Common.UserPanel.src.Controllers {
	/// <summary>
	/// 用户中心的控制器
	/// </summary>
	[ExportMany]
	public class UserPanelController : IController {
		/// <summary>
		/// 修改密码
		/// </summary>
		/// <returns></returns>
		[Action("home/change_password")]
		[Action("home/change_password", HttpMethods.POST)]
		public IActionResult ChangePassword() {
			PrivilegesChecker.Check(UserTypesGroup.All);
			var form = new ChangePasswordForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult("common.user_panel/generic_form.html",
					new { title = "Change Password", iconClass = "fa fa-lock", form });
			}
		}

		/// <summary>
		/// 修改头像
		/// </summary>
		/// <returns></returns>
		[Action("home/change_avatar")]
		[Action("home/change_avatar", HttpMethods.POST)]
		public IActionResult ChangeAvatar() {
			PrivilegesChecker.Check(UserTypesGroup.All);
			var form = new ChangeAvatarForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult("common.user_panel/generic_form.html",
					new { title = "Change Avatar", iconClass = "fa fa-smile-o", form });
			}
		}
	}
}
