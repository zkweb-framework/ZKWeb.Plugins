using System;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Domain.Uow;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable;
using ZKWeb.Plugins.Common.Base.src.UIComponents.StaticTable.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.src.UIComponents.HtmlProviders {
	/// <summary>
	/// 支付交易的Html提供器
	/// </summary>
	public class PaymentTransactionHtmlProvider {
		/// <summary>
		/// 构建显示错误信息的Html
		/// </summary>
		protected virtual HtmlString BuildErrorHtml(string error) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			return new HtmlString(templateManager.RenderTemplate("finance.payment/error_text.html", new { error }));
		}

		/// <summary>
		/// 获取支付时使用的Html
		/// 交易不存在等错误发生时返回错误信息
		/// </summary>
		public virtual HtmlString GetPaymentHtml(Guid transactionId) {
			var unitOfWork = Application.Ioc.Resolve<UnitOfWork>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			using (unitOfWork.Scope()) {
				// 获取交易和支付接口
				var transaction = transactionManager.Get(transactionId);
				var api = transaction == null ? null : transaction.Api;
				// 检查交易和接口是否存在
				if (transaction == null) {
					return BuildErrorHtml(new T("Payment transaction not found"));
				} else if (api == null) {
					return BuildErrorHtml(new T("Payment api not exist"));
				}
				// 检查当前登录用户是否可以支付
				var result = transaction.Check(c => c.IsPayerLoggedIn);
				if (!result.First) {
					return BuildErrorHtml(result.Second);
				}
				result = transaction.Check(c => c.IsPayable);
				if (!result.First) {
					return BuildErrorHtml(result.Second);
				}
				// 调用接口处理器生成支付html
				var html = new HtmlString("No Result");
				var handlers = api.GetHandlers();
				handlers.ForEach(h => h.GetPaymentHtml(transaction, ref html));
				return html;
			}
		}

		/// <summary>
		/// 获取支付结果的html
		/// 交易不存在等错误发生时返回错误信息
		/// </summary>
		public virtual HtmlString GetResultHtml(Guid transactionId) {
			var unitOfWork = Application.Ioc.Resolve<UnitOfWork>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			using (unitOfWork.Scope()) {
				// 获取交易
				var transaction = transactionManager.Get(transactionId);
				if (transaction == null) {
					return BuildErrorHtml(new T("Payment transaction not found"));
				}
				// 检查当前登录用户是否可以查看
				var result = transaction.Check(c => c.IsPayerLoggedIn);
				if (!result.First) {
					return BuildErrorHtml(result.Second);
				}
				// 调用接口处理器生成结果html
				var html = new List<HtmlString>();
				var handlers = transaction.GetHandlers();
				handlers.ForEach(h => h.GetResultHtml(transaction, html));
				return new HtmlString(string.Join("", html));
			}
		}

		/// <summary>
		/// 获取交易详细记录的html
		/// </summary>
		public virtual HtmlString GetDetailRecordsHtml(Guid transactionId) {
			var unitOfWork = Application.Ioc.Resolve<UnitOfWork>();
			var transactionManager = Application.Ioc.Resolve<PaymentTransactionManager>();
			using (unitOfWork.Scope()) {
				var table = new StaticTableBuilder();
				table.Columns.Add("CreateTime", "150");
				table.Columns.Add("Creator", "150");
				table.Columns.Add("Contents");
				var records = transactionManager.GetDetailRecords(transactionId);
				foreach (var record in records) {
					table.Rows.Add(new {
						CreateTime = record.CreateTime.ToClientTime(),
						Creator = record.Creator == null ? null : record.Creator.Username,
						Contents = record.Content
					});
				}
				return (HtmlString)table.ToLiquid();
			}
		}
	}
}
