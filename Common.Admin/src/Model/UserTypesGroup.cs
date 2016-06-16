using System;
using System.Linq;

namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 用户类型分组
	/// </summary>
	public class UserTypesGroup {
		/// <summary>
		/// 管理员
		/// </summary>
		public readonly static UserTypes[] Admin =
			new UserTypes[] { UserTypes.Admin, UserTypes.SuperAdmin };
		/// <summary>
		/// 合作伙伴
		/// </summary>
		public readonly static UserTypes[] Parter =
			new UserTypes[] { UserTypes.CooperationPartner };
		/// <summary>
		/// 管理员或合作伙伴
		/// </summary>
		public readonly static UserTypes[] AdminOrParter =
			Admin.Concat(Parter).ToArray();
		/// <summary>
		/// 所有类型，仅检查是否已登录时使用
		/// </summary>
		public readonly static UserTypes[] All =
			Enum.GetValues(typeof(UserTypes)).OfType<UserTypes>().ToArray();
	}
}
