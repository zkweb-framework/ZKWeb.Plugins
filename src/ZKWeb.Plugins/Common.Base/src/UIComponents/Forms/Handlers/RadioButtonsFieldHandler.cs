using System;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Handlers {
	/// <summary>
	/// 单选按钮组
	/// </summary>
	[ExportMany(ContractKey = typeof(RadioButtonsFieldAttribute)), SingletonReuse]
	public class RadioButtonsFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 构建单选按钮组的Html
		/// </summary>
		public static HtmlString BuildRadioButtonsHtml(
			RadioButtonsFieldAttribute attribute,
			IDictionary<string, string> htmlAttributes, object value) {
			var valueAndProvider = ListItemUtils.GetValueAndProvider<IListItemProvider>(
				attribute.Source, value);
			var listItems = valueAndProvider.Second.GetItems().ToList();
			var valueString = (valueAndProvider.First is Enum) ?
				((int)valueAndProvider.First).ToString() :
				valueAndProvider.First.ConvertOrDefault<string>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var radioList = templateManager.RenderTemplate("common.base/tmpl.form.radio_list.html", new {
				name = attribute.Name,
				attributes = htmlAttributes,
				options = listItems.Select(item => new {
					name = item.Name,
					value = item.Value,
					selected = item.Value == valueString
				})
			});
			return new HtmlString(radioList);
		}

		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (RadioButtonsFieldAttribute)field.Attribute;
			var radioButtonsHtml = BuildRadioButtonsHtml(attribute, htmlAttributes, field.Value);
			return field.WrapFieldHtml(htmlAttributes, radioButtonsHtml.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			var attribute = (RadioButtonsFieldAttribute)field.Attribute;
			var parsed = values[0];
			return ListItemUtils.WrapValueAndProvider(attribute.Source, parsed);
		}
	}
}
