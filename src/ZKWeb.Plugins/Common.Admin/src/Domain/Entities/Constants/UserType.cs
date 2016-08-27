namespace ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Constants {
	/// <summary>
	/// 用户类型
	/// </summary>
	public static class UserTypes {
		/// <summary>
		/// 超级管理员
		/// 可登陆后台
		/// </summary>
		public const string SuperAdmin = "SuperAdmin";
		/// <summary>
		/// 管理员
		/// 可登陆后台
		/// </summary>
		public const string Admin = "Admin";
		/// <summary>
		/// 合作伙伴
		/// 可登录后台，但不能进行部分操作
		/// </summary>
		public const string CooperationPartner = "CooperationPartner";
		/// <summary>
		/// 普通用户
		/// 不可登陆后台
		/// </summary>
		public const string User = "User";
	}
}
