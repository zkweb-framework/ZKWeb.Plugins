using System.ComponentModel.DataAnnotations;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.Admin.src.Controllers;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.UserContact.src.Domain.Services;

namespace ZKWeb.Plugins.Common.UserContact.src.DataEditFormCallbacks {
	/// <summary>
	/// 添加联系信息到后台的用户编辑页中
	/// </summary>
	[ExportMany]
	public class AddContactEditorToUserManager :
		IEntityFormExtraHandler<User, Guid, UserCrudController.BaseForm> {
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
		public void OnCreated(UserCrudController.BaseForm form) {
			form.AddFieldsFrom(this);
		}

		/// <summary>
		/// 绑定数据到表单
		/// </summary>
		public void OnBind(UserCrudController.BaseForm form, User bindFrom) {
			var userContactManager = Application.Ioc.Resolve<UserContactManager>();
			var contact = userContactManager.GetContact(bindFrom.Id);
			Tel = contact.Tel;
			Mobile = contact.Mobile;
			QQ = contact.QQ;
			Email = contact.Email;
			Address = contact.Address;
		}

		/// <summary>
		/// 保存表单到数据
		/// </summary>
		public void OnSubmit(UserCrudController.BaseForm form, User saveTo) { }

		/// <summary>
		/// 保存数据后的处理
		/// </summary>
		public void OnSubmitSaved(UserCrudController.BaseForm form, User saved) {
			var userContactManager = Application.Ioc.Resolve<UserContactManager>();
			userContactManager.SetContact(saved.Id, contact => {
				contact.Tel = Tel;
				contact.Mobile = Mobile;
				contact.QQ = QQ;
				contact.Email = Email;
				contact.Address = Address;
			});
		}
	}
}
