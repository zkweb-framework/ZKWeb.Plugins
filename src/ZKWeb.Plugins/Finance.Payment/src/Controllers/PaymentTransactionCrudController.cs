using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Extensions;
using ZKWeb.Localize;
using ZKWebStandard.Ioc;
using ZKWebStandard.Collection;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using System;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Enums;
using ZKWeb.Plugins.Common.Admin.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Bases;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.Common.Admin.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable;
using ZKWeb.Plugins.Common.Currency.src.Domain.Service;
using ZKWeb.Plugins.Common.Currency.src.Components.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Enums;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Controllers;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;

namespace ZKWeb.Plugins.Finance.Payment.src.Controllers {
	/// <summary>
	/// 支付交易记录
	/// </summary>
	[ExportMany]
	public class PaymentTransactionCrudController : CrudAdminAppControllerBase<PaymentTransaction, Guid> {
		public override string Group { get { return "Payment"; } }
		public override string GroupIconClass { get { return "fa fa-money"; } }
		public override string Name { get { return "PaymentTransactionRecords"; } }
		public override string Url { get { return "/admin/payment_transactions"; } }
		public override string TileClass { get { return "tile bg-yellow"; } }
		public override string IconClass { get { return "fa fa-download"; } }
		public override string AddUrl { get { return null; } }
		protected override bool UseTransaction { get { return true; } }
		protected override IAjaxTableHandler<PaymentTransaction, Guid> GetTableHandler() { return new TableHandler(); }
		protected override IModelFormBuilder GetAddForm() {
			throw new BadRequestException(new T("Add transaction from admin panel is not supported"));
		}
		protected override IModelFormBuilder GetEditForm() { return new Form(); }

		/// <summary>
		/// 表格回调
		/// </summary>
		public class TableHandler : AjaxTableHandlerBase<PaymentTransaction, Guid> {
			/// <summary>
			/// 构建表格
			/// </summary>
			public override void BuildTable(
				AjaxTableBuilder table, AjaxTableSearchBarBuilder searchBar) {
				table.StandardSetupFor<PaymentTransactionCrudController>();
				searchBar.StandardSetupFor<PaymentTransactionCrudController>(
					"Serial/Payer/Payee/Description/Remark");
			}

			/// <summary>
			/// 过滤数据
			/// </summary>
			public override void OnQuery(
				AjaxTableSearchRequest request, ref IQueryable<PaymentTransaction> query) {
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
			/// 选择字段
			/// </summary>
			public override void OnSelect(
				AjaxTableSearchRequest request, IList<EntityToTableRow<PaymentTransaction>> pairs) {
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
					pair.Row["PaymentFee"] = currency.Format(pair.Entity.PaymentFee);
					pair.Row["Payer"] = payer == null ? null : payer.Username;
					pair.Row["Payee"] = payee == null ? null : payee.Username;
					pair.Row["PayerId"] = payer == null ? null : (Guid?)payer.Id;
					pair.Row["PayeeId"] = payee == null ? null : (Guid?)payee.Id;
					pair.Row["CreateTime"] = pair.Entity.CreateTime.ToClientTimeString();
					pair.Row["UpdateTime"] = pair.Entity.UpdateTime.ToClientTimeString();
					pair.Row["State"] = pair.Entity.State;
					pair.Row["Deleted"] = pair.Entity.Deleted ? EnumDeleted.Deleted : EnumDeleted.None;
				}
			}

			/// <summary>
			/// 添加列
			/// </summary>
			public override void OnResponse(
				AjaxTableSearchRequest request, AjaxTableSearchResponse response) {
				response.Columns.AddIdColumn("Id").StandardSetupFor<PaymentTransactionCrudController>(request);
				response.Columns.AddNoColumn();
				response.Columns.AddMemberColumn("Serial");
				response.Columns.AddMemberColumn("ExternalSerial");
				response.Columns.AddMemberColumn("Type");
				response.Columns.AddEditColumnFor<PaymentApiCrudController>("ApiName", "ApiId");
				response.Columns.AddMemberColumn("Amount");
				response.Columns.AddMemberColumn("PaymentFee");
				response.Columns.AddEditColumnFor<UserCrudController>("Payer", "PayerId");
				response.Columns.AddEditColumnFor<UserCrudController>("Payee", "PayeeId");
				response.Columns.AddMemberColumn("CreateTime");
				response.Columns.AddMemberColumn("UpdateTime");
				response.Columns.AddEnumLabelColumn("State", typeof(PaymentTransactionState));
				response.Columns.AddActionColumn().StandardSetupFor<PaymentTransactionCrudController>(request);
			}
		}

		/// <summary>
		/// 查看支付交易的表单
		/// </summary>
		public class Form : TabEntityFormBuilder<PaymentTransaction, Guid, Form> {
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
			/// 支付手续费
			/// </summary>
			[LabelField("PaymentFee")]
			public string PaymentFee { get; set; }
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
			protected override void OnBind(PaymentTransaction bindFrom) {
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
				PaymentFee = currency.Format(bindFrom.PaymentFee);
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
			protected override object OnSubmit(PaymentTransaction saveTo) {
				saveTo.Remark = Remark;
				return this.SaveSuccessAndCloseModal();
			}
		}
	}
}
