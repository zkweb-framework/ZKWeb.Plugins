using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Entities.UserTypes {
	/// <summary>
	/// 用户类型: 管理员
	/// </summary>
	[ExportMany]
	public class AdminUserType : IUserType, IAmAdmin {
		public const string ConstType = "Admin";
		public string Type { get { return ConstType; } }
	}
}
