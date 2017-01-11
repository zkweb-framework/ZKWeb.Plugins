using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Common.Datepicker.src.UIComponents.FormFieldAttributes {
	/// <summary>
	/// 选择日期的字段
	/// 字段类型应该是DateTime或DateTime?
	/// </summary>
	public class DateFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 预置文本
		/// </summary>
		public string PlaceHolder { get; set; }
		/// <summary>
		/// 时间格式，默认是"yyyy/MM/dd"
		/// </summary>
		public string DateFormat { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="placeHolder">预置文本</param>
		/// <param name="dateFormat">时间格式，默认是"yyyy/MM/dd"</param>
		public DateFieldAttribute(string name, string placeHolder = null, string dateFormat = null) {
			Name = name;
			PlaceHolder = placeHolder;
			DateFormat = dateFormat ?? "yyyy/MM/dd";
		}
	}
}
