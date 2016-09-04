using System;
using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Components.EntityOperationHandlers {
	/// <summary>
	/// 支付接口创建后不能修改类型和所属人
	/// </summary>
	[ExportMany]
	public class CannotChangeTypeOrOwnerAfterPaymentApiCreated : IEntityOperationHandler<PaymentApi> {
		private Guid OldId { get; set; }
		private string OldType { get; set; }
		private string OldOwner { get; set; }

		public void BeforeSave(IDatabaseContext context, PaymentApi data) {
			OldId = data.Id;
			OldType = data.Type;
			OldOwner = data.Owner == null ? null : data.Owner.Username;
		}

		public void AfterSave(IDatabaseContext context, PaymentApi data) {
			if (OldId != Guid.Empty) {
				var newType = data.Type;
				var newOwner = data.Owner == null ? null : data.Owner.Username;
				if (newType != OldType) {
					throw new InvalidOperationException(new T("Cannot change type after payment api created"));
				} else if (newOwner != OldOwner) {
					throw new InvalidOperationException(new T("Cannot change owner after payment api created"));
				}
			}
		}

		public void BeforeDelete(IDatabaseContext context, PaymentApi entity) { }

		public void AfterDelete(IDatabaseContext context, PaymentApi entity) { }
	}
}
