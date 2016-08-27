using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Entities.UserTypes {
	/// <summary>
	/// 用户类型: 普通用户
	/// </summary>
	[ExportMany]
	public class NormalUserType : IUserType {
		public const string ConstType = "User";
		public string Type { get { return ConstType; } }
	}
}
