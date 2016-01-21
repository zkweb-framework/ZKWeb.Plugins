using DryIoc;
using System;
using System.Collections.Generic;
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
	/// 修改自身头像的表单
	/// </summary>
	public class ChangeAvatarForm : ModelFormBuilder {
		/// <summary>
		/// 头像
		/// </summary>
		[FileUploaderField("Avatar")]
		public HttpPostedFileBase Avatar { get; set; }
		/// <summary>
		/// 删除头像
		/// </summary>
		[CheckBoxField("DeleteAvatar")]
		public bool DeleteAvatar { get; set; }

		/// <summary>
		/// 绑定表单
		/// </summary>
		protected override void OnBind() { }

		/// <summary>
		/// 提交表单
		/// </summary>
		/// <returns></returns>
		protected override object OnSubmit() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			var userManager = Application.Ioc.Resolve<UserManager>();
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
