using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Domain.Repositories {
	/// <summary>
	/// 支付交易的仓储
	/// </summary>
	[ExportMany]
	public class PaymentTransactionRepository : RepositoryBase<PaymentTransaction, Guid> { }
}
