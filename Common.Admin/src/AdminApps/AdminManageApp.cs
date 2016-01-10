using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Core;
using ZKWeb.Utils.Extensions;
using ZKWeb.Plugins.Common.Base.src.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.AdminApps {
	/// <summary>
	/// 管理员管理
	/// 要求超级管理员
	/// </summary>
	[ExportMany]
	public class AdminManageApp : AdminAppBuilder<User, AdminManageApp> {
		public override string Name { get { return "Admin Manage"; } }
		public override string Url { get { return "/admin/admins"; } }
		public override string TileClass { get { return "tile bg-blue-hoki"; } }
		public override string IconClass { get { return "fa fa-user-secret"; } }
		public override UserTypes[] AllowedUserTypes { get { return new[] { UserTypes.SuperAdmin }; } }
		protected override IAjaxTableSearchHandler<User> GetSearchHandler() { return new SearchHandler(); }
		protected override FormBuilder GetAddForm() { return new FormBuilder(); }
		protected override FormBuilder GetEditForm() { return new FormBuilder(); }

		/// <summary>
		/// 搜索处理器
		/// </summary>
		public class SearchHandler : IAjaxTableSearchHandler<User> {
			/// <summary>
			/// 构建搜索栏时的处理
			/// </summary>
			public void OnBuildSearchBar(AjaxTableSearchBarBuilder searchBar) {
				searchBar.KeywordPlaceHolder = new T("Username");
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<User> query) {
				query = query.Where(u => AdminManager.AdminTypes.Contains(u.Type));
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(u => u.Username.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<User> query) {
				query = query.OrderByDescending(u => u.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<User, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Username"] = pair.Key.Username;
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
				}
			}

			/// <summary>
			/// 添加列和批量操作
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
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
	}
}
