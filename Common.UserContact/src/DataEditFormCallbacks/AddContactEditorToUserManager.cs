using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src.AdminApps;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.UserContact.src.DataEditFormCallbacks {
	/// <summary>
	/// 添加联系信息到后台的用户编辑页中
	/// </summary>
	[ExportMany]
	public class AddContactEditorToUserManager : IDataEditFormCallback<User, UserManageApp.BaseForm> {
		public void OnCreated(UserManageApp.BaseForm form) {
			form.Form.Fields.AddRange(new[] {
				new FormField(new TextBoxFieldAttribute("Tel") { Group = "Contact Infomation" }),
				new FormField(new TextBoxFieldAttribute("Mobile") { Group = "Contact Infomation" }),
				new FormField(new TextBoxFieldAttribute("QQ") { Group = "Contact Infomation" }),
				new FormField(new TextBoxFieldAttribute("Email") { Group = "Contact Infomation" }),
				new FormField(new TextBoxFieldAttribute("Address") { Group = "Contact Infomation" })
			});
		}

		public void OnBind(UserManageApp.BaseForm form, DatabaseContext context, User bindFrom) {
		}

		public void OnSubmit(UserManageApp.BaseForm form, DatabaseContext context, User saveTo) {
		}

		public void OnSubmitSaved(UserManageApp.BaseForm form, DatabaseContext context, User saved) {
		}
	}
}
