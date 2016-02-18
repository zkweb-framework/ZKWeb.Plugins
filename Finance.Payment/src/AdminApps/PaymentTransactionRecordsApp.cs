using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Utils.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;

namespace ZKWeb.Plugins.Finance.Payment.src.AdminApps {
	/// <summary>
	/// 支付交易记录
	/// </summary>
	[ExportMany]
	public class PaymentTransactionRecordsApp : AdminAppBuilder<PaymentTransaction, PaymentTransactionRecordsApp> {
		public override string Name { get { return "PaymentTransactionRecords"; } }
		public override string Url { get { return "/admin/payment_transactions"; } }
		public override string TileClass { get { return "tile bg-yellow-gold"; } }
		public override string IconClass { get { return "fa fa fa-download"; } }
		protected override IAjaxTableCallback<PaymentTransaction> GetTableCallback() { return new TableCallback(); }
		protected override IModelFormBuilder GetAddForm() {
			throw new HttpException(400, new T("Add transaction from admin panel is not supported"));
		}
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableCallback : IAjaxTableCallback<PaymentTransaction> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public void OnBuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.MenuItems.AddDivider();
				table.MenuItems.AddEditActionForAdminApp<PaymentTransactionRecordsApp>();
				searchBar.KeywordPlaceHolder = new T("Serial/Payer/Payee/Description/Remark");
				searchBar.MenuItems.AddDivider();
				searchBar.MenuItems.AddRecycleBin();
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public void OnQuery(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<PaymentTransaction> query) {
				// 按回收站
				query = query.FilterByRecycleBin(request);
				// 按关键字
				if (!string.IsNullOrEmpty(request.Keyword)) {
					query = query.Where(q =>
						q.Serial.Contains(request.Keyword) ||
						q.ExternalSerial.Contains(request.Keyword) ||
						q.Payer.Username.Contains(request.Keyword) ||
						q.Payee.Username.Contains(request.Keyword) ||
						q.Description.Contains(request.Keyword) ||
						q.Remark.Contains(request.Keyword));
				}
			}

			/// <summary>
			/// 排序数据
			/// </summary>
			public void OnSort(
				AjaxTableSearchRequest request, DatabaseContext context, ref IQueryable<PaymentTransaction> query) {
				query = query.OrderByDescending(t => t.Id);
			}

			/// <summary>
			/// 选择字段
			/// </summary>
			public void OnSelect(
				AjaxTableSearchRequest request, List<KeyValuePair<PaymentTransaction, Dictionary<string, object>>> pairs) {
				foreach (var pair in pairs) {
					var payer = pair.Key.Payer;
					var payee = pair.Key.Payee;
					var api = pair.Key.Api; // not nullable
					pair.Value["Id"] = pair.Key.Id;
					pair.Value["Serial"] = pair.Key.Serial;
					pair.Value["Type"] = new T(pair.Key.Type);
					pair.Value["ApiName"] = new T(api.Name);
					pair.Value["ApiId"] = api.Id;
					pair.Value["ExternalSerial"] = pair.Key.ExternalSerial;
					pair.Value["Amount"] = pair.Key.Amount;
					pair.Value["Payer"] = payer == null ? null : payer.Username;
					pair.Value["Payee"] = payee == null ? null : payee.Username;
					pair.Value["PayerId"] = payer == null ? null : (long?)payer.Id;
					pair.Value["PayeeId"] = payee == null ? null : (long?)payee.Id;
					pair.Value["CreateTime"] = pair.Key.CreateTime.ToClientTimeString();
					pair.Value["LastUpdated"] = pair.Key.LastUpdated.ToClientTimeString();
					pair.Value["State"] = pair.Key.State;
					pair.Value["Deleted"] = pair.Key.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				var idColumn = response.Columns.AddIdColumn("Id");
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Serial");
				response.Columns.AddMemberColumn("ExternalSerial");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddMemberColumn("ApiName");
				response.Columns.AddMemberColumn("Amount");
				response.Columns.AddMemberColumn("Payer");
				response.Columns.AddMemberColumn("Payee");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddEnumLabelColumn("State", typeof(PaymentTransactionState));
				var actionColumn = response.Columns.AddActionColumn();
				actionColumn.AddEditActionForAdminApp<PaymentTransactionRecordsApp>();
				idColumn.AddDivider();
				idColumn.AddDeleteActionsForAdminApp<PaymentTransactionRecordsApp>(request);
			}
		}

		/// <summary>
		/// 查看支付交易的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<PaymentTransaction, Form> {
			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, PaymentTransaction bindFrom) {
				throw new NotImplementedException();
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, PaymentTransaction saveTo) {
				throw new NotImplementedException();
			}
		}
	}
}
