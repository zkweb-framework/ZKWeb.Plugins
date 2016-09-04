using System;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Enums.Attributes {
	/// <summary>
	/// 显示为标签文本时使用的css类
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class LabelCssClassAttribute : Attribute {
		/// <summary>
		/// css类
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="cssClass">css类，例如"label label-success"</param>
		public LabelCssClassAttribute(string cssClass) {
			CssClass = cssClass;
		}
	}
}
