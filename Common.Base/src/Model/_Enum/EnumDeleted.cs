using System.ComponentModel;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 是否已删除的枚举
	/// </summary>
	public enum EnumDeleted {
		/// <summary>
		/// 未删除
		/// </summary>
		[Description("")]
		None = 0,
		/// <summary>
		/// 已删除
		/// </summary>
		[LabelCssClass("label label-danger")]
		Deleted = 1
	}
}
