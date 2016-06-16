using System.ComponentModel.DataAnnotations;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.AdminApps;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.UserContact.src.DataEditFormCallbacks {
	/// <summary>
	/// 添加联系信息到后台的用户编辑页中
	/// </summary>
	[ExportMany]
	public class AddContactEditorToUserManager : IDataEditFormExtension<User, UserManageApp.BaseForm> {
		/// <summary>
		/// 电话
		/// </summary>
		[TextBoxField("Tel", "Tel", Group = "Contact Information")]
		[RegularExpression(RegexUtils.Expressions.Tel)]
		public string Tel { get; set; }
		/// <summary>
		/// 手机
		/// </summary>
		[TextBoxField("Mobile", "Mobile", Group = "Contact Information")]
		[RegularExpression(RegexUtils.Expressions.Tel)]
		public string Mobile { get; set; }
		/// <summary>
		/// QQ
		/// </summary>
		[TextBoxField("QQ", "QQ", Group = "Contact Information")]
		[RegularExpression(RegexUtils.Expressions.Digits)]
		public string QQ { get; set; }
		/// <summary>
		/// 邮箱
		/// </summary>
		[TextBoxField("Email", "Email", Group = "Contact Information")]
		[RegularExpression(RegexUtils.Expressions.Email)]
		public string Email { get; set; }
		/// <summary>
		/// 地址
		/// </summary>
		[TextBoxField("Address", "Address", Group = "Contact Information")]
		public string Address { get; set; }

		/// <summary>
		/// 创建表单时的处理
		/// </summary>
		public void OnCreated(UserManageApp.BaseForm form) {
			form.AddFieldsFrom(this);
		}

		/// <summary>
		/// 绑定数据到表单
		/// </summary>
		public void OnBind(UserManageApp.BaseForm form, DatabaseContext context, User bindFrom) {
			var contact = context.Get<Database.UserContact>(c => c.User.Id == bindFrom.Id) ??
				new Database.UserContact();
			Tel = contact.Tel;
			Mobile = contact.Mobile;
			QQ = contact.QQ;
			Email = contact.Email;
			Address = contact.Address;
		}

		/// <summary>
		/// 保存表单到数据
		/// </summary>
		public void OnSubmit(UserManageApp.BaseForm form, DatabaseContext context, User saveTo) { }

		/// <summary>
		/// 保存数据后的处理
		/// </summary>
		public void OnSubmitSaved(UserManageApp.BaseForm form, DatabaseContext context, User saved) {
			var contact = context.Get<Database.UserContact>(c => c.User.Id == saved.Id) ??
				new Database.UserContact() { User = saved };
			contact.Tel = Tel;
			contact.Mobile = Mobile;
			contact.QQ = QQ;
			contact.Email = Email;
			contact.Address = Address;
			context.Save(ref contact);
		}
	}
}
