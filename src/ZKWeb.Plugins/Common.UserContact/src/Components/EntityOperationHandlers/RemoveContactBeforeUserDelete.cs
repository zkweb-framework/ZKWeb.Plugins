using ZKWeb.Database;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.UserContact.src.Components.EntityOperationHandlers {
	/// <summary>
	/// 在用户删除前删除关联的联系信息
	/// </summary>
	[ExportMany]
	public class RemoveContactBeforeUserDelete :
		IEntityOperationHandler<User> {
		public void BeforeSave(IDatabaseContext context, User entity) { }

		public void AfterSave(IDatabaseContext context, User entity) { }

		public void AfterDelete(IDatabaseContext context, User data) { }

		public void BeforeDelete(IDatabaseContext context, User data) {
			context.BatchDelete<Domain.Entities.UserContact>(c => c.User.Id == data.Id);
		}
	}
}
