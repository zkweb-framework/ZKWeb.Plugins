using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Model;

namespace ZKWeb.Plugins.Finance.Payment.src.PaymentTransactionHandlers {
	/// <summary>
	/// 测试交易的处理器
	/// </summary>
	[ExportMany]
	public class TestTransactionHandler : IPaymentTransactionHandler {
		/// <summary>
		/// 交易类型
		/// </summary>
		public string Type { get { return "TestTransaction"; } }
		
		/// <summary>
		/// 交易创建后
		/// </summary>
		public void OnCreated(PaymentTransaction transaction) {
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// 等待付款时
		/// </summary>
		public void OnWaitingPaying(DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 担保交易付款后
		/// </summary>
		public void OnSecuredPaid(DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState, ref AutoSendGoodsParameters autoSendGoodsParameters) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 交易成功时
		/// </summary>
		public void OnSuccess(DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 交易终止时
		/// </summary>
		public void OnAbort(DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取显示交易结果的Html
		/// </summary>
		public void GetResultHtml(PaymentTransaction transaction, ref HtmlString html) {
			throw new NotImplementedException();
		}
	}
}
