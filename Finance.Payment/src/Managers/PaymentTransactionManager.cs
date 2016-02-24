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
			PaymentTransaction transaction = null;
			UnitOfWork.WriteData<PaymentTransactionRepository, PaymentTransaction>(repository => {
				transaction = repository.CreateTransaction(
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
		/// 获取支付时使用的Html
		/// 交易不存在等错误发生时返回错误信息
		/// </summary>
		public virtual HtmlString GetPaymentHtml(long transactionId) {
			// 获取交易和支付接口
			PaymentTransaction transaction = null;
			PaymentApi api = null;
			UnitOfWork.ReadData<PaymentTransaction>(repository => {
				transaction = repository.GetById(transactionId);
				api = transaction == null ? null : transaction.Api;
				var _ = api == null ? null : api.Type; // 在数据库连接关闭前抓取类型
			});
			// 检查交易和接口是否存在
			var buildErrorHtml = new Func<string, HtmlString>(error => {
				var templateManager = Application.Ioc.Resolve<TemplateManager>();
				return new HtmlString(templateManager.RenderTemplate("finance.payment/error_text.html", new { error }));
			});
			if (transaction == null) {
				return buildErrorHtml(new T("Payment transaction not found"));
			} else if (api == null) {
				return buildErrorHtml(new T("Payment api not exist"));
			}
			// 检查当前登录用户是否可以支付
			var result = transaction.Check(c => c.IsPayableByLoggedinUser);
			if (!result.Item1) {
				return buildErrorHtml(result.Item2);
			}
			result = transaction.Check(c => c.IsPayable);
			if (!result.Item1) {
				return buildErrorHtml(result.Item2);
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取交易详细记录的html
		/// </summary>
		public virtual HtmlString GetDetailRecordsHtml(long transactionId) {
			var table = new DataTable();
			table.Columns.Add("CreateTime").ExtendedProperties["width"] = "150";
			table.Columns.Add("Creator").ExtendedProperties["width"] = "150";
			table.Columns.Add("Contents");
			UnitOfWork.ReadData<PaymentTransactionRepository, PaymentTransaction>(repository => {
				var records = repository.GetDetailRecords(transactionId);
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
