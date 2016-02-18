using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Forms;
using ZKWeb.Utils.Extensions;
using System.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Finance.Payment.src.ListItemProviders;
using System.ComponentModel.DataAnnotations;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Localize;
using ZKWeb.Database;

namespace ZKWeb.Plugins.Finance.Payment.src.AdminApps {
	/// <summary>
	/// 支付接口管理
	/// </summary>
	[ExportMany]
	public class PaymentApiManageApp : AdminAppBuilder<PaymentApi, PaymentApiManageApp> {
		public override string Name { get { return "PaymentApiManage"; } }
		public override string Url { get { return "/admin/payment_apis"; } }
		public override string TileClass { get { return "tile bg-yellow-gold"; } }
		public override string IconClass { get { return "fa fa fa-arrow-circle-o-down"; } }
		protected override IAjaxTableCallback<PaymentApi> GetTableCallback() { return new TableCallback(); }
		protected override IModelFormBuilder GetAddForm() {
			var request = HttpContext.Current.Request;
			var type = request.GetParam<string>("type"); //支付接口类型，没有传入时需要先选择
			if (string.IsNullOrEmpty(type)) {
				return new SelectTypeForAddForm();
			}
			return new PaymentApiEditForm();
		}
		protected override IModelFormBuilder GetEditForm() { return new PaymentApiEditForm(); }

		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<string> GetPrivileges() {
			var extraPrivileges = new[] {
				Name + ":Test" // 测试支付接口的权限
			};
			return base.GetPrivileges().Concat(extraPrivileges);
		}

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<PaymentApi> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditActionForAdminApp<PaymentApiManageApp>();
				table.MenuItems.AddAddActionForAdminApp<PaymentApiManageApp>();
				searchBar.KeywordPlaceHolder = new T("Name/Owner/Remark");
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
				searchBar.MenuItems.AddAddActionForAdminApp<PaymentApiManageApp>();
			}

			/// <summary>
			/// 查询数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<PaymentApi> query) {
				// 按回收站
				query = query.FilterByRecycleBin(request);
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Name.Contains(request.Keyword) ||
						q.Owner.Username.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<PaymentApi> query) {
				query = query.OrderBy(q => q.DisplayOrder).ThenByDescending(q => q.Id);
			}

			/// <summary>
			/// 选择数据
			/// </summary>
			public void OnSelect(AjaxTableSearchRequest request, List<KeyValuePair<PaymentApi, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					var owner = pair.Key.Owner;
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Name"] = new T(pair.Key.Name);
					pair.Value["Type"] = new T(pair.Key.Type);
					pair.Value["Owner"] = owner == null ? null : owner.Username;
					pair.Value["OwnerId"] = owner == null ? null : (long?)owner.Id;
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
					pair.Value["LastUpdated"] = pair.Key.LastUpdated.ToClientTimeString();
					pair.Value["DisplayOrder"] = pair.Key.DisplayOrder;
					pair.Value["Deleted"] = pair.Key.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列和操作
			/// </summary>
			public void OnResponse(AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Name", "35%");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddMemberColumn("Owner");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddMemberColumn("DisplayOrder");
				response.Columns.AddEnumLabelColumn("Deleted", typeof(EnumDeleted));
				var actionColumn = response.Columns.AddActionColumn("150");
				actionColumn.AddButtonForOpenLink(
					new T("TestPayment"), "btn btn-xs default", "fa fa-edit",
					"/admin/payment_apis/test_payment?id=<%-row.Id%>");
				actionColumn.AddEditActionForAdminApp<PaymentApiManageApp>();
				idColumn.AddDivider();
				idColumn.AddDeleteActionsForAdminApp<PaymentApiManageApp>(request);
			}
		}

		/// <summary>
		/// 用于选择添加时使用的支付接口类型的表单
		/// </summary>
		[Form("SelectTypeForAddForm", SubmitButtonText = "NextStep")]
		public class SelectTypeForAddForm : ModelFormBuilder {
			/// <summary>
			/// 类型
			/// </summary>
			[Required]
			[RadioButtonsField("PaymentApiType", typeof(PaymentApiTypeListItemProvider))]
			public string Type { get; set; }

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() {
				var provider = new PaymentApiTypeListItemProvider();
				var firstItem = provider.GetItems().FirstOrDefault();
				Type = firstItem == null ? null : firstItem.Value;
			}

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				var app = new PaymentApiManageApp();
				var url = string.Format("{0}?type={1}", app.AddUrl, Type);
				return new { script = ScriptStrings.RedirectModal(url) };
			}
		}
	}
}
