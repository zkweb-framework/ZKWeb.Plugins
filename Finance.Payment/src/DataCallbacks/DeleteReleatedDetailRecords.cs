using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Database;
using ZKWeb.Database.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Repositories;
using ZKWeb.Plugins.Common.GenericRecord.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Database;
using ZKWeb.Plugins.Finance.Payment.src.Repositories;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.DataCallbacks {
	/// <summary>
	/// 删除交易时同时删除关联的详细记录
	/// </summary>
	[ExportMany]
	public class DeleteReleatedDetailRecords : IDataDeleteCallback<PaymentTransaction> {
		public void AfterDelete(DatabaseContext context, PaymentTransaction data) { }

		public void BeforeDelete(DatabaseContext context, PaymentTransaction data) {
			var recordRepository = RepositoryResolver.Resolve<GenericRecord>(context);
			recordRepository.DeleteWhere(
				r => r.Type == PaymentTransactionRepository.RecordType && r.ReleatedId == data.Id);
		}
	}
}
