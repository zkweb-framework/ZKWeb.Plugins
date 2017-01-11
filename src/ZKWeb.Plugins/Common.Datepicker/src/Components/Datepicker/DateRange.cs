using System;

namespace ZKWeb.Plugins.Common.Datepicker.src.Components.Datepicker {
	/// <summary>
	/// 时间范围
	/// </summary>
	public class DateRange {
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime? Begin { get; set; }
		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime? Finish { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public DateRange() { }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="begin">开始时间</param>
		/// <param name="finish">结束时间</param>
		public DateRange(DateTime? begin, DateTime? finish) {
			Begin = begin;
			Finish = finish;
		}
	}
}
