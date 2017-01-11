using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Datepicker.src.Components.Datepicker;
using ZKWeb.Plugins.Common.Datepicker.src.UIComponents.FormFieldAttributes;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Datepicker.src.UIComponents.FormFieldHandlers {
	/// <summary>
	/// 选择日期范围的字段处理器
	/// </summary>
	[ExportMany(ContractKey = typeof(DateRangeFieldAttribute)), SingletonReuse]
	public class DateRangeFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (DateRangeFieldAttribute)field.Attribute;
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var dateRange = (field.Value as DateRange) ?? new DateRange();
			var html = templateManager.RenderTemplate("common.datepicker/tmpl.form.daterange.html", new {
				name = field.Attribute.Name,
				value = JsonConvert.SerializeObject(new {
					Begin = dateRange.Begin?.ToClientTime().ToString(attribute.DateFormat),
					Finish = dateRange.Finish?.ToClientTime().ToString(attribute.DateFormat)
				}),
				beginPlaceholder = new T(attribute.BeginPlaceHolder),
				finishPlaceHolder = new T(attribute.FinishPlaceHolder),
				dateFormat = attribute.DateFormat,
				attributes = htmlAttributes
			});
			return field.WrapFieldHtml(htmlAttributes, html);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			var json = values[0];
			var dateRangeString = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
			var beginString = dateRangeString?.GetOrDefault("Begin");
			var finishString = dateRangeString?.GetOrDefault("Finish");
			var result = new DateRange();
			DateTime parsed;
			var attribute = (DateRangeFieldAttribute)field.Attribute;
			if (DateTime.TryParseExact(beginString, attribute.DateFormat, null, DateTimeStyles.None, out parsed) ||
				DateTime.TryParse(beginString, out parsed)) {
				result.Begin = parsed.FromClientTime();
			}
			if (DateTime.TryParseExact(finishString, attribute.DateFormat, null, DateTimeStyles.None, out parsed) ||
				DateTime.TryParse(finishString, out parsed)) {
				result.Finish = parsed.FromClientTime();
			}
			return result;
		}
	}
}
