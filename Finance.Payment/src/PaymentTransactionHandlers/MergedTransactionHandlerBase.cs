using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Model;
using ZKWebStandard.Collection;

namespace ZKWeb.Plugins.Finance.Payment.src.PaymentTransactionHandlers {
	/// <summary>
	/// 合并交易处理器的基类
	/// 因为合并交易需要安全检查和区分类别
	/// 这个处理器不注册到容器中，需要继承使用
	/// </summary>
	public abstract class MergedTransactionHandlerBase {
		/// <summary>
		/// 交易创建后
		/// </summary>
		public virtual void OnCreated(DatabaseContext context, PaymentTransaction transaction) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 等待付款时
		/// </summary>
		public virtual void OnWaitingPaying(
			DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 担保交易付款后
		/// </summary>
		public virtual void OnSecuredPaid(
			DatabaseContext context, PaymentTransaction transaction,
			PaymentTransactionState previousState, ref AutoSendGoodsParameters parameters) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 交易成功时
		/// </summary>
		public virtual void OnSuccess(
			DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 交易终止时
		/// </summary>
		public virtual void OnAbort(
			DatabaseContext context, PaymentTransaction transaction, PaymentTransactionState previousState) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取显示交易结果的Html
		/// </summary>
		public virtual void GetResultHtml(PaymentTransaction transaction, ref HtmlString html) {
			throw new NotImplementedException();
		}
	}
}
