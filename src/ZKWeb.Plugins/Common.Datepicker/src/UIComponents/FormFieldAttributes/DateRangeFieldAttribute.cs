using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;

namespace ZKWeb.Plugins.Common.Datepicker.src.UIComponents.FormFieldAttributes {
	/// <summary>
	/// 选择日期范围的字段
	/// 字段类型应该是DateRange
	/// </summary>
	public class DateRangeFieldAttribute : FormFieldAttribute {
		/// <summary>
		/// 开始时间的预置文本
		/// </summary>
		public string BeginPlaceHolder { get; set; }
		/// <summary>
		/// 结束时间的预置文本
		/// </summary>
		public string FinishPlaceHolder { get; set; }
		/// <summary>
		/// 时间格式，默认是"yyyy/MM/dd"
		/// </summary>
		public string DateFormat { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="name">字段名称</param>
		/// <param name="placeHolder">预期文本</param>
		/// <param name="dateFormat">时间格式，默认是"yyyy/MM/dd"</param>
		public DateRangeFieldAttribute(string name,
			string beginPlaceHolder = null, string finishPlaceHolder = null, string dateFormat = null) {
			Name = name;
			BeginPlaceHolder = beginPlaceHolder;
			FinishPlaceHolder = finishPlaceHolder;
			DateFormat = dateFormat ?? "yyyy/MM/dd";
		}
	}
}
