using System;
using ZKWeb.Database;
using ZKWeb.Plugins.Common.Base.src.Domain.Repositories.Interfaces;
using ZKWeb.Plugins.Common.GenericRecord.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Entities;
using ZKWeb.Plugins.Finance.Payment.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Components.EntityOperationHandlers {
	/// <summary>
	/// 删除交易时同时删除关联的详细记录
	/// </summary>
	[ExportMany]
	public class DeleteReleatedDetailRecords : IEntityOperationHandler<PaymentTransaction> {
		public void BeforeSave(IDatabaseContext context, PaymentTransaction entity) { }

		public void AfterSave(IDatabaseContext context, PaymentTransaction entity) { }

		public void AfterDelete(IDatabaseContext context, PaymentTransaction data) { }

		public void BeforeDelete(IDatabaseContext context, PaymentTransaction data) {
			var repository = Application.Ioc.Resolve<IRepository<GenericRecord, Guid>>();
			repository.BatchDelete(r =>
				r.Type == PaymentTransactionManager.RecordType &&
				r.ReleatedId == data.Id);
		}
	}
}
