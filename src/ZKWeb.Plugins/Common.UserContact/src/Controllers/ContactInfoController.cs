using System.ComponentModel.DataAnnotations;
using ZKWeb.Localize;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.UserPanel.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.UserContact.src.Domain.Services;

namespace ZKWeb.Plugins.Common.UserContact.src.Controllers {
	/// <summary>
	/// 编辑自身联系信息的表单
	/// </summary>
	[ExportMany]
	public class ContactInfoController : FormUserPanelControllerBase {
		public override string Group { get { return "Account Manage"; } }
		public override string GroupIconClass { get { return "fa fa-user"; } }
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
			[TextBoxField("Tel", "Tel")]
			[RegularExpression(RegexUtils.Expressions.Tel)]
			public string Tel { get; set; }
			/// <summary>
			/// 手机
			/// </summary>
			[TextBoxField("Mobile", "Mobile")]
			[RegularExpression(RegexUtils.Expressions.Tel)]
			public string Mobile { get; set; }
			/// <summary>
			/// QQ
			/// </summary>
			[TextBoxField("QQ", "QQ")]
			[RegularExpression(RegexUtils.Expressions.Digits)]
			public string QQ { get; set; }
			/// <summary>
			/// 邮箱
			/// </summary>
			[TextBoxField("Email", "Email")]
			[RegularExpression(RegexUtils.Expressions.Email)]
			public string Email { get; set; }
			/// <summary>
			/// 地址
			/// </summary>
			[TextBoxField("Address", "Address")]
			public string Address { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var sessionManager = Application.Ioc.Resolve<SessionManager>();
				var userContactManager = Application.Ioc.Resolve<UserContactManager>();
				var session = sessionManager.GetSession();
				var contact = userContactManager.GetContact(session.ReleatedId.Value);
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
				var userContactManager = Application.Ioc.Resolve<UserContactManager>();
				var session = sessionManager.GetSession();
				userContactManager.SetContact(session.ReleatedId.Value, c => {
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
