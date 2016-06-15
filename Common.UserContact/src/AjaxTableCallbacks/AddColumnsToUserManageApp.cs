using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.AdminApps;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.UserContact.src.Repositories;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.UserContact.src.AjaxTableCallbacks {
	/// <summary>
	/// 添加以下列到用户管理
	///		电话
	///		手机
	/// </summary>
	[ExportMany]
	public class AddColumnsToUserManageApp : IAjaxTableCallbackExtension<User, UserManageApp.TableCallback> {
		public void OnBuildTable(AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) { }

		public void OnQuery(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<User> query) { }

		public void OnSort(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<User> query) { }

		public void OnSelect(AjaxTableSearchRequest request, List<EntityToTableRow<User>> pairs) {
			var contacts = UnitOfWork.ReadRepository<
				UserContactRepository, Dictionary<long, Database.UserContact>>(r => {
					return r.GetContacts(pairs.Select(p => p.Entity.Id).ToList());
				});
			foreach (var pair in pairs) {
				var contact = contacts.GetOrDefault(pair.Entity.Id) ?? new Database.UserContact();
				pair.Row["Tel"] = contact.Tel;
				pair.Row["Mobile"] = contact.Mobile;
			}
		}

		public void OnResponse(AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
			var usernameColumn = response.Columns.FirstOrDefault(c => c.Key == "Username");
			if (usernameColumn != null) {
				usernameColumn.Width = "35%";
			}
			response.Columns.MoveAfter(response.Columns.AddMemberColumn("Tel"), "Username");
			response.Columns.MoveAfter(response.Columns.AddMemberColumn("Mobile"), "Tel");
		}
	}
}
