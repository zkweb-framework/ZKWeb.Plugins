using System;

namespace ZKWeb.Plugins.Common.Base.src.Model {
	/// <summary>
	/// 表单字段的属性
	/// 用于标记模型中的成员是表单字段
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class FormFieldAttribute : Attribute {
		/// <summary>
		/// 字段名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 字段所属的分组
		/// </summary>
		public string Group { get; set; }
	}
}
