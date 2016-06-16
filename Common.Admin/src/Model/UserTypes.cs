namespace ZKWeb.Plugins.Common.Admin.src.Model {
	/// <summary>
	/// 用户类型
	/// </summary>
	public enum UserTypes {
		/// <summary>
		/// 一般用户
		/// 不可登陆后台
		/// </summary>
		User = 0,
		/// <summary>
		/// 管理员
		/// 可登陆后台
		/// </summary>
		Admin = 1,
		/// <summary>
		/// 超级管理员
		/// 可登陆后台
		/// </summary>
		SuperAdmin = 2,
		/// <summary>
		/// 合作伙伴
		/// 可登陆后台，但不能进行管理员操作
		/// </summary>
		CooperationPartner = 3
	}
}
