using System;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Bases;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Domain.Repositories {
	/// <summary>
	/// 支付接口的仓储
	/// </summary>
	[ExportMany]
	public class PaymentApiRepository : RepositoryBase<PaymentApi, Guid> { }
}
