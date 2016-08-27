namespace ZKWeb.Plugins.Common.Base.src.Domain.Entities.Interfaces {
	/// <summary>
	/// 包含标记删除的接口
	/// </summary>
	public interface IHaveDeleted {
		/// <summary>
		/// 标记已删除
		/// </summary>
		bool Deleted { get; set; }
	}
}
