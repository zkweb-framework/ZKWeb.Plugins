using System;
using System.Collections.Generic;
using System.Globalization;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Datepicker.src.UIComponents.FormFieldAttributes;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Datepicker.src.UIComponents.FormFieldHandlers {
	/// <summary>
	/// 选择日期的字段处理器
	/// </summary>
	[ExportMany(ContractKey = typeof(DateFieldAttribute)), SingletonReuse]
	public class DateFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (DateFieldAttribute)field.Attribute;
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var clientTime = (field.Value is DateTime) ? ((DateTime)field.Value).ToClientTime() : (DateTime?)null;
			var html = templateManager.RenderTemplate("common.datepicker/tmpl.form.date.html", new {
				name = field.Attribute.Name,
				value = clientTime?.ToString(attribute.DateFormat),
				placeholder = new T(attribute.PlaceHolder),
				dateFormat = attribute.DateFormat,
				attributes = htmlAttributes
			});
			return field.WrapFieldHtml(htmlAttributes, html);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			var value = values[0];
			DateTime result;
			var attribute = (DateFieldAttribute)field.Attribute;
			if (DateTime.TryParseExact(value, attribute.DateFormat, null, DateTimeStyles.None, out result) ||
				DateTime.TryParse(value, out result)) {
				return result.FromClientTime();
			}
			return null;
		}
	}
}
