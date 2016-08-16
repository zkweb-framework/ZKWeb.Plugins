using System;
using ZKWeb.Database;
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
	public class DeleteReleatedDetailRecords : IEntityOperationHandler<PaymentTransaction> {
		public void BeforeSave(IDatabaseContext context, PaymentTransaction entity) { }

		public void AfterSave(IDatabaseContext context, PaymentTransaction entity) { }

		public void AfterDelete(IDatabaseContext context, PaymentTransaction data) { }

		public void BeforeDelete(IDatabaseContext context, PaymentTransaction data) {
			var recordRepository = RepositoryResolver.Resolve<GenericRecord>(context);
			recordRepository.BatchDelete(
				r => r.Type == PaymentTransactionRepository.RecordType && r.ReleatedId == data.Id);
		}
	}
}
