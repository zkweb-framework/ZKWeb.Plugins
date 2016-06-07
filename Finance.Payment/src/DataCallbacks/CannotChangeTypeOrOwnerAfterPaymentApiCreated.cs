using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Database;
using ZKWeb.Database.Interfaces;
using ZKWeb.Localize;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Finance.Payment.src.DataCallbacks {
	/// <summary>
	/// 支付接口创建后不能修改类型和所属人
	/// </summary>
	[ExportMany]
	public class CannotChangeTypeOrOwnerAfterPaymentApiCreated : IDataSaveCallback<PaymentApi> {
		private long OldId { get; set; }
		private string OldType { get; set; }
		private string OldOwner { get; set; }

		public void BeforeSave(DatabaseContext context, PaymentApi data) {
			OldId = data.Id;
			OldType = data.Type;
			OldOwner = data.Owner == null ? null : data.Owner.Username;
		}

		public void AfterSave(DatabaseContext context, PaymentApi data) {
			if (OldId > 0) {
				var newType = data.Type;
				var newOwner = data.Owner == null ? null : data.Owner.Username;
				if (newType != OldType) {
					throw new InvalidOperationException(new T("Cannot change type after payment api created"));
				} else if (newOwner != OldOwner) {
					throw new InvalidOperationException(new T("Cannot change owner after payment api created"));
				}
			}
		}
	}
}
