using System;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes {
	/// <summary>
	/// Json字段
	/// 这个字段储存的值会在绑定时序列化
	/// </summary>
	public class JsonFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 字段类型，序列化和反序列化时使用
		/// </summary>
		public Type FieldType { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="fieldType">字段类型，序列化和反序列化时使用</param>
		public JsonFieldAttribute(string name, Type fieldType) {
			Name = name;
			FieldType = fieldType;
		}
	}
}
