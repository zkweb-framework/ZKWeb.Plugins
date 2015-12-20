using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Model.ActionResults;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Controllers {
	[ExportMany]
	public class TestController : IController {
		[Action("/")]
		public string Index() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var session = sessionManager.GetSession();
			session.Items["last updated"] = DateTime.UtcNow;
			sessionManager.SaveSession();
			return "test index, session = " + session.Id + " translate = " + new T("abc");
		}

		[Action("redirect")]
		public IActionResult Redirect() {
			return new RedirectResult("/");
		}

		[Action("json")]
		public IActionResult Json() {
			return new JsonResult(new { a = 1, b = 2, c = 3 });
		}

		[Action("file")]
		public IActionResult File() {
			return new FileResult(PathConfig.WebsiteConfigPath);
		}

		[Action("template")]
		public IActionResult Template() {
			return new TemplateResult("test.html",
				new {
					a = "<p>test encode</p>",
					b = "<b>test html</b>",
					c = typeof(TestController).Assembly.Location
				});
		}

		[Action("form")]
		[Action("form", HttpMethods.POST)]
		public IActionResult TestForm() {
			var form = new FormBuilder();
			form.Attribute = new FormAttribute("TestForm");
			form.Fields.Add(new FormField(new TextBoxFieldAttribute("Username")));
			form.Fields.Add(new FormField(new PasswordFieldAttribute("Password")));
			var request = HttpContext.Current.Request;
			if (request.HttpMethod == HttpMethods.POST) {
				var values = form.ParseValues(request.GetParams());
				var username = values.GetOrDefault<string>("Username");
				var password = values.GetOrDefault<string>("Password");
				return new JsonResult(new { Username = username, Passwod = password });
			} else {
				form.BindValuesFromAnonymousObject(new { Username = "TestUser", Password = "TestPassword" });
				return new TemplateResult("test_form.html", new { form = form });
			}
		}

		[Form("TestForm", "/test_form")]
		public class TestFormModel : ModelFormBuilder {
			[TextBoxField("Username")]
			public string Username { get; set; }
			[PasswordField("Password")]
			public string Password { get; set; }
		}
	}
}
