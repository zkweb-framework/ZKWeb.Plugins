using DotLiquid;
using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZKWeb;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Model.ActionResults;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Forms;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.Database;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.Controllers {
	/// <summary>
	/// 后台的控制器
	/// </summary>
	[ExportMany]
	public class AdminController : IController {
		/// <summary>
		/// 后台首页
		/// 显示应用列表，会根据当前用户权限进行过滤
		/// </summary>
		/// <returns></returns>
		[Action("admin")]
		public IActionResult Admin() {
			PrivilegesChecker.CheckAdminOrPartner();
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser();
			var apps = Application.Ioc.ResolveMany<AdminApp>();
			apps = apps.Where(app =>
				app.AllowedUserTypes.Contains(user.Type) &&
				PrivilegesChecker.HasPrivileges(user, app.RequiredPrivileges));
			return new TemplateResult("common.admin/admin_index.html", new { apps });
		}

		/// <summary>
		/// 后台登陆页
		/// </summary>
		/// <returns></returns>
		[Action("admin/login")]
		[Action("admin/login", HttpMethods.POST)]
		public IActionResult Login() {
			var form = new AdminLoginForm();
			if (HttpContext.Current.Request.HttpMethod == HttpMethods.POST) {
				return new JsonResult(form.Submit());
			} else {
				form.Bind();
				var adminManager = Application.Ioc.Resolve<AdminManager>();
				var warning = adminManager.GetLoginWarning();
				return new TemplateResult("common.admin/admin_login.html", new { form, warning });
			}
		}

		/// <summary>
		/// 退出后台登陆
		/// </summary>
		/// <returns></returns>
		[Action("admin/logout", HttpMethods.POST)]
		public IActionResult Logout() {
			var userManager = Application.Ioc.Resolve<UserManager>();
			userManager.Logout();
			return new RedirectResult("/admin/login");
		}

		public class TestSearchHandler : IAjaxTableSearchHandler<User> {
			public IEnumerable<FormField> GetConditions() {
				yield return new FormField(new TextBoxFieldAttribute("TestCondition"));
			}

			public void OnQuery(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<User> query) {
				var keyword = request.Keyword;
				if (!string.IsNullOrEmpty(keyword)) {
					query = query.Where(u => u.Username.Contains(keyword));
				}
				query = query.Where(u => AdminManager.AdminTypes.Contains(u.Type));
			}

			public void OnSort(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<User> query) {
				query = query.OrderByDescending(u => u.Id);
			}

			public void OnSelect(AjaxTableSearchRequest request, List<KeyValuePair<User, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Username"] = pair.Key.Username;
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
				}
			}

			public void OnResponse(AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Id");
				response.Columns.AddMemberColumn("Username");
				response.Columns.AddMemberColumn("CreateTime");
				// response.Columns.AddActionColumn().AddEditAction();
				// idColumn.AddBatchRemoveAction();
				// idColumn.AddBatchRemoveForeverAction();
				// idColumn.AddBatchRecoverAction();
			}
		}

		/// <summary>
		/// 测试列表页
		/// </summary>
		/// <returns></returns>
		[Action("admin/list")]
		[Action("admin/list/search", HttpMethods.POST)]
		public IActionResult TestSearch() {
			var handler = new TestSearchHandler();
			var request = HttpContext.Current.Request;
			if (request.HttpMethod == HttpMethods.POST) {
				var json = request.GetParam<string>("json");
				var searchRequest = AjaxTableSearchRequest.FromJson(json);
				var searchResponse = AjaxTableSearchResponse.FromRequest(searchRequest, new[] { handler });
				return new JsonResult(searchResponse);
			}
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = "AdminList";
			table.Target = "/admin/list/search";
			var searchBar = Application.Ioc.Resolve<AjaxTableSearchBarBuilder>();
			searchBar.TableId = table.Id;
			searchBar.Conditions.AddRange(handler.GetConditions());
			return new TemplateResult("common.admin/test_list.html", new { table, searchBar });
		}
	}
}
