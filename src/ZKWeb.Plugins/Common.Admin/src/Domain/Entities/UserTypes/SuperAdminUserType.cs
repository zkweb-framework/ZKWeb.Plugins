using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Entities.UserTypes {
	/// <summary>
	/// 用户类型: 超级管理员
	/// </summary>
	[ExportMany]
	public class SuperAdminUserType : IUserType, IAmSuperAdmin {
		public const string ConstType = "SuperAdmin";
		public string Type { get { return ConstType; } }
	}
}
