using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Repositories;

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
	}
}
