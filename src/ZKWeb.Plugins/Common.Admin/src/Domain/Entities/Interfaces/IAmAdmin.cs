namespace ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces {
	/// <summary>
	/// 标记用户类型是管理员
	/// </summary>
	public interface IAmAdmin : IAmUser, ICanUseAdminPanel { }
}
