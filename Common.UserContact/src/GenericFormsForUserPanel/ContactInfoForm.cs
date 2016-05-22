using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.UserContact.src.Repositories;
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
				var session = sessionManager.GetSession();
				UnitOfWork.ReadRepository<UserContactRepository>(r => {
					var contact = r.GetContact(session.ReleatedId);
					Tel = contact.Tel;
					Mobile = contact.Mobile;
					QQ = contact.QQ;
					Email = contact.Email;
					Address = contact.Address;
				});
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var sessionManager = Application.Ioc.Resolve<SessionManager>();
				var session = sessionManager.GetSession();
				UnitOfWork.WriteRepository<UserContactRepository>(r => {
					r.SetContact(session.ReleatedId, c => {
						c.Tel = Tel;
						c.Mobile = Mobile;
						c.QQ = QQ;
						c.Email = Email;
						c.Address = Address;
					});
				});
				return new { message = new T("Saved Successfully") };
			}
		}
	}
}
