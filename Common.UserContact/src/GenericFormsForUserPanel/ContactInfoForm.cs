using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.UserPanel.src;
using ZKWeb.Plugins.Common.UserPanel.src.Scaffolding;
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.UserContact.src.GenericFormsForUserPanel {
	/// <summary>
	/// 编辑自身联系信息的表单
	/// </summary>
	[ExportMany]
	public class ContactInfoForm : GenericFormForUserPanel {
		public override string Group { get { return "Account Manage"; } }
		public override string GroupIcon { get { return "fa fa-user"; } }
		public override string Name { get { return "Contact Information"; } }
		public override string IconClass { get { return "fa fa-phone"; } }
		public override string Url { get { return "/home/contact_info"; } }
		protected override IModelFormBuilder GetForm() { return new Form(); }

		/// <summary>
		/// 表单
		/// </summary>
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 电话
			/// </summary>
			[TextBoxField("Tel")]
			[RegularExpression(RegexUtils.Expressions.Tel)]
			public string Tel { get; set; }
			/// <summary>
			/// 手机
			/// </summary>
			[TextBoxField("Mobile")]
			[RegularExpression(RegexUtils.Expressions.Tel)]
			public string Mobile { get; set; }
			/// <summary>
			/// QQ
			/// </summary>
			[TextBoxField("QQ")]
			[RegularExpression(RegexUtils.Expressions.Digits)]
			public string QQ { get; set; }
			/// <summary>
			/// 邮箱
			/// </summary>
			[TextBoxField("Email")]
			[RegularExpression(RegexUtils.Expressions.Email)]
			public string Email { get; set; }
			/// <summary>
			/// 地址
			/// </summary>
			[TextBoxField("Address")]
			public string Address { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var sessionManager = Application.Ioc.Resolve<SessionManager>();
				var session = sessionManager.GetSession();
				var contactManager = Application.Ioc.Resolve<UserContactManager>();
				var contact = contactManager.GetContact(session.ReleatedId);
				Tel = contact.Tel;
				Mobile = contact.Mobile;
				QQ = contact.QQ;
				Email = contact.Email;
				Address = contact.Address;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var sessionManager = Application.Ioc.Resolve<SessionManager>();
				var session = sessionManager.GetSession();
				var contactManager = Application.Ioc.Resolve<UserContactManager>();
				contactManager.SetContact(session.ReleatedId, c => {
					c.Tel = Tel;
					c.Mobile = Mobile;
					c.QQ = QQ;
					c.Email = Email;
					c.Address = Address;
				});
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
