using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Handlers {
	/// <summary>
	/// 下拉列表
	/// </summary>
	[ExportMany(ContractKey = typeof(DropdownListFieldAttribute)), SingletonReuse]
	public class DropdownListFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 构建Select元素的Html
		/// </summary>
		public static HtmlString BuildSelectHtml(
			DropdownListFieldAttribute attribute,
			IDictionary<string, string> htmlAttributes, object value) {
			var listItemProvider = (IListItemProvider)Activator.CreateInstance(attribute.Source);
			var listItems = listItemProvider.GetItems().ToList();
			var valueString = (value is Enum) ?
				((int)value).ToString() : value.ConvertOrDefault<string>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var select = templateManager.RenderTemplate("common.base/tmpl.form.select.html", new {
				name = attribute.Name,
				attributes = htmlAttributes,
				options = listItems.Select(item => new {
					name = item.Name,
					value = item.Value,
					selected = item.Value == valueString
				})
			});
			return new HtmlString(select);
		}

		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (DropdownListFieldAttribute)field.Attribute;
			var selectHtml = BuildSelectHtml(attribute, htmlAttributes, field.Value);
			return field.WrapFieldHtml(htmlAttributes, selectHtml.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return values[0];
		}
	}
}
