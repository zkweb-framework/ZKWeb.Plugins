using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Datepicker.src.UIComponents.FormFieldAttributes;
using ZKWeb.Plugins.Common.DynamicForm.src.UIComponents.DynamicForm.Interfaces;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.DynamicForm.DatePicker.src.Components.UIComponents.DynamicFormFieldFactories {
	/// <summary>
	/// 日期范围选择器生成器
	/// </summary>
	[ExportMany(ContractKey = "Date")]
	public class DateFieldFactory : IDynamicFormFieldFactory {
		/// <summary>
		/// 创建表单字段属性
		/// </summary>
		public FormFieldAttribute Create(IDictionary<string, object> fieldData) {
			var name = fieldData.GetOrDefault<string>("Name");
			var placeHolder = fieldData.GetOrDefault<string>("PlaceHolder");
			var dateFormat = fieldData.GetOrDefault<string>("DateFormat");
			var group = fieldData.GetOrDefault<string>("Group");
			return new DateFieldAttribute(
				name, placeHolder, dateFormat) { Group = group };
		}
	}
}
