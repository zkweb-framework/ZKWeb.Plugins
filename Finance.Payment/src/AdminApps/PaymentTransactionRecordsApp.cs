using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Admin.src;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Utils.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Admin.src.Scaffolding;
using ZKWeb.Localize;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Currency.src.Managers;
using ZKWeb.Plugins.Common.Currency.src.Model;
using ZKWeb.Plugins.Finance.Payment.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.AdminApps;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Finance.Payment.src.AdminApps {
	/// <summary>
	/// 支付交易记录
	/// </summary>
	[ExportMany]
	public class PaymentTransactionRecordsApp : AdminAppBuilder<PaymentTransaction> {
		public override string Name { get { return "PaymentTransactionRecords"; } }
		public override string Url { get { return "/admin/payment_transactions"; } }
		public override string TileClass { get { return "tile bg-yellow"; } }
		public override string IconClass { get { return "fa fa-download"; } }
		public override string AddUrl { get { return null; } }
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
				table.StandardSetupForAdminApp<PaymentTransactionRecordsApp>();
				searchBar.StandardSetupForAdminApp<PaymentTransactionRecordsApp>(
					"Serial/Payer/Payee/Description/Remark");
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
				AjaxTableSearchRequest request, List<EntityToTableRow<PaymentTransaction>> pairs) {
				var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
				foreach (var pair in pairs) {
					var payer = pair.Entity.Payer;
					var payee = pair.Entity.Payee;
					var api = pair.Entity.Api; // not nullable
					var currency = currencyManager.GetCurrency(pair.Entity.CurrencyType);
					pair.Row["Id"] = pair.Entity.Id;
					pair.Row["Serial"] = pair.Entity.Serial;
					pair.Row["Type"] = new T(pair.Entity.Type);
					pair.Row["ApiName"] = new T(api.Name);
					pair.Row["ApiId"] = api.Id;
					pair.Row["ExternalSerial"] = pair.Entity.ExternalSerial;
					pair.Row["Amount"] = currency.Format(pair.Entity.Amount);
					pair.Row["Payer"] = payer == null ? null : payer.Username;
					pair.Row["Payee"] = payee == null ? null : payee.Username;
					pair.Row["PayerId"] = payer == null ? null : (long?)payer.Id;
					pair.Row["PayeeId"] = payee == null ? null : (long?)payee.Id;
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["LastUpdated"] = pair.Entity.LastUpdated.ToClientTimeString();
					pair.Row["State"] = pair.Entity.State;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列
			/// </summary>
			public void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupForAdminApp<PaymentTransactionRecordsApp>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Serial");
				response.Columns.AddMemberColumn("ExternalSerial");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddEditColumnForAdminApp<PaymentApiManageApp>("ApiName", "ApiId");
				response.Columns.AddMemberColumn("Amount");
				response.Columns.AddEditColumnForAdminApp<UserManageApp>("Payer", "PayerId");
				response.Columns.AddEditColumnForAdminApp<UserManageApp>("Payee", "PayeeId");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("LastUpdated");
				response.Columns.AddEnumLabelColumn("State", typeof(PaymentTransactionState));
				response.Columns.AddActionColumn().StandardSetupForAdminApp<PaymentTransactionRecordsApp>(request);
			}
		}

		/// <summary>
		/// 查看支付交易的表单
		/// </summary>
		public class Form : TabDataEditFormBuilder<PaymentTransaction, Form> {
			/// <summary>
			/// 交易类型
			/// </summary>
			[LabelField("Type")]
			public string Type { get; set; }
			/// <summary>
			/// 支付接口
			/// </summary>
			[LabelField("PaymentApi")]
			public string PaymentApi { get; set; }
			/// <summary>
			/// 交易状态
			/// </summary>
			[LabelField("State")]
			public string State { get; set; }
			/// <summary>
			/// 流水号
			/// </summary>
			[LabelField("Serial")]
			public string Serial { get; set; }
			/// <summary>
			/// 外部流水号
			/// </summary>
			[LabelField("ExternalSerial")]
			public string ExternalSerial { get; set; }
			/// <summary>
			/// 货币
			/// </summary>
			[LabelField("Currency")]
			public string Currency { get; set; }
			/// <summary>
			/// 金额
			/// </summary>
			[LabelField("Amount")]
			public string Amount { get; set; }
			/// <summary>
			/// 付款人
			/// </summary>
			[LabelField("Payer")]
			public string Payer { get; set; }
			/// <summary>
			/// 收款人
			/// </summary>
			[LabelField("Payee")]
			public string Payee { get; set; }
			/// <summary>
			/// 描述
			/// </summary>
			[LabelField("Description")]
			public string Description { get; set; }
			/// <summary>
			/// 最后发生的错误
			/// </summary>
			[LabelField("LastError")]
			public string LastError { get; set; }
			/// <summary>
			/// 详细记录
			/// </summary>
			[HtmlField("DetailRecords", Group = "DetailRecords")]
			public HtmlString DetailRecords { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[TextAreaField("Remark", 5, "Remark", Group = "Remark")]
			public string Remark { get; set; }

			/// <summary>
			/// 绑定数据到表单
			/// </summary>
			protected override void OnBind(DatabaseContext context, PaymentTransaction bindFrom) {
				var currencyManager = Application.Ioc.Resolve<CurrencyManager>();
				var currency = currencyManager.GetCurrency(bindFrom.CurrencyType);
				var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
				Type = new T(bindFrom.Type);
				PaymentApi = bindFrom.Api.ToString();
				State = new T(bindFrom.State.GetDescription());
				Serial = bindFrom.Serial;
				ExternalSerial = bindFrom.ExternalSerial;
				Currency = new T(bindFrom.CurrencyType);
				Amount = currency.Format(bindFrom.Amount);
				Payer = bindFrom.Payer == null ? null : bindFrom.Payer.Username;
				Payee = bindFrom.Payee == null ? null : bindFrom.Payee.Username;
				Description = bindFrom.Description;
				LastError = bindFrom.LastError;
				DetailRecords = transactionManager.GetDetailRecordsHtml(bindFrom.Id);
				Remark = bindFrom.Remark;
			}

			/// <summary>
			/// 保存表单到数据
			/// </summary>
			protected override object OnSubmit(DatabaseContext context, PaymentTransaction saveTo) {
				saveTo.Remark = Remark;
				return new {
					message = new T("Saved Successfully"),
					script = ScriptStrings.AjaxtableUpdatedAndCloseModal
				};
			}
		}
	}
}
