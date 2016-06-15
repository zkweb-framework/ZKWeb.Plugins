using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 布尔值的枚举
	/// </summary>
	public enum EnumBool {
		/// <summary>
		/// 是
		/// </summary>
		[LabelCssClass("label label-success")]
		True = 1,
		/// <summary>
		/// 否
		/// </summary>
		[LabelCssClass("label label-default")]
		False = 0,
		/// <summary>
		/// 空
		/// </summary>
		Empty = -1,
	}
}
