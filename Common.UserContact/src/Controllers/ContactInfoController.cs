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
using ZKWeb.Plugins.Common.UserContact.src.Forms;

namespace ZKWeb.Plugins.Common.UserContact.src.Controllers {
	/// <summary>
	/// 会员中心的联系信息
	/// </summary>
	[ExportMany]
	public class ContactInfoController : IController {
		/// <summary>
		/// 联系信息
		/// </summary>
		[Action("home/contact_info")]
		[Action("home/contact_info", HttpMethods.POST)]
		public IActionResult ContactInfo() {
			PrivilegesChecker.Check(UserTypesGroup.All);
			var form = new ContactInfoForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				return new TemplateResult("common.user_panel/generic_form.html",
					new { title = "Contact Information", iconClass = "fa fa-phone", form });
			}
		}
	}
}
