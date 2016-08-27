using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Admin.src.Domain.Entities.UserTypes {
	/// <summary>
	/// 用户类型: 合作伙伴
	/// </summary>
	[ExportMany]
	public class CooperationPartner : IUserType, IAmCooperationPartner {
		public const string ConstType = "CooperationPartner";
		public string Type { get { return ConstType; } }
	}
}
