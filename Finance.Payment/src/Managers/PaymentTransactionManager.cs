using Common.Minimal.Model.Extensions;
using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericRecord.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Extensions;
using ZKWeb.Plugins.Finance.Payment.src.Repositories;
using ZKWeb.Templating;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Finance.Payment.src.Managers {
	/// <summary>
	/// 支付交易管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class PaymentTransactionManager {
		/// <summary>
		/// 添加测试用的交易
		/// </summary>
		public virtual PaymentTransaction CreateTestTransaction(
			long apiId, decimal amount, string currency, long? payerId, long? payeeId, string description) {
			var transaction = UnitOfWork.WriteRepository<PaymentTransactionRepository, PaymentTransaction>(r => {
				return r.CreateTransaction(
					"TestTransaction", apiId, amount, currency, payerId, payeeId, payerId, description);
			});
			return transaction;
		}

		/// <summary>
		/// 获取支付Url，创建交易后可以跳转到这个Url进行支付
		/// </summary>
		public virtual string GetPaymentUrl(long transactionId) {
			return string.Format("/payment/transaction/pay?id={0}", transactionId);
		}

		/// <summary>
		/// 获取查看结果的Url，支付成功或失败后可以跳转到这个Url显示结果
		/// </summary>
		public virtual string GetResultUrl(long transactionId) {
			return string.Format("/payment/transaction/pay_result?id={0}", transactionId);
		}

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
		public virtual HtmlString GetPaymentHtml(long transactionId) {
			// 获取交易和支付接口
			PaymentTransaction transaction = null;
			PaymentApi api = null;
			UnitOfWork.ReadData<PaymentTransaction>(r => {
				transaction = r.GetById(transactionId);
				api = transaction == null ? null : transaction.Api;
				var _ = api == null ? null : api.Type; // 在数据库连接关闭前抓取类型
			});
			// 检查交易和接口是否存在
			if (transaction == null) {
				return BuildErrorHtml(new T("Payment transaction not found"));
			} else if (api == null) {
				return BuildErrorHtml(new T("Payment api not exist"));
			}
			// 检查当前登录用户是否可以支付
			var result = transaction.Check(c => c.IsPayerLoggedIn);
			if (!result.Item1) {
				return BuildErrorHtml(result.Item2);
			}
			result = transaction.Check(c => c.IsPayable);
			if (!result.Item1) {
				return BuildErrorHtml(result.Item2);
			}
			// 调用接口处理器生成支付html
			var html = new HtmlString("No Result");
			var handlers = Application.Ioc.ResolvePaymentApiHandlers(api.Type);
			handlers.ForEach(h => h.GetPaymentHtml(transaction, ref html));
			return html;
		}

		/// <summary>
		/// 获取支付结果的html
		/// 交易不存在等错误发生时返回错误信息
		/// </summary>
		public virtual HtmlString GetResultHtml(long transactionId) {
			// 获取交易
			var transaction = UnitOfWork.ReadData<PaymentTransaction, PaymentTransaction>(
				r => r.GetById(transactionId));
			// 检查当前登录用户是否可以查看
			var result = transaction.Check(c => c.IsPayerLoggedIn);
			if (!result.Item1) {
				return BuildErrorHtml(result.Item2);
			}
			// 调用接口处理器生成结果html
			var html = new HtmlString("No Result");
			var handlers = Application.Ioc.ResolveTransactionHandlers(transaction.Type);
			handlers.ForEach(h => h.GetResultHtml(transaction, ref html));
			return html;
		}

		/// <summary>
		/// 获取交易详细记录的html
		/// </summary>
		public virtual HtmlString GetDetailRecordsHtml(long transactionId) {
			var table = new DataTable();
			table.Columns.Add("CreateTime").ExtendedProperties["width"] = "150";
			table.Columns.Add("Creator").ExtendedProperties["width"] = "150";
			table.Columns.Add("Contents");
			UnitOfWork.ReadRepository<PaymentTransactionRepository>(r => {
				var records = r.GetDetailRecords(transactionId);
				foreach (var record in records) {
					table.Rows.Add(
						record.CreateTime.ToClientTime(),
						record.Creator == null ? null : record.Creator.Username,
						record.Content);
				}
			});
			return table.ToHtml();
		}
	}
}
