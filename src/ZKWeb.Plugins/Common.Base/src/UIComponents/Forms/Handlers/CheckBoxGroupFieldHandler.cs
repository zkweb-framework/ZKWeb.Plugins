using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems;
using ZKWeb.Plugins.Common.Base.src.UIComponents.ListItems.Interfaces;
using ZKWeb.Templating;
using ZKWebStandard.Collection;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Handlers {
	/// <summary>
	/// 勾选框分组
	/// 显示结构
	/// ▢ 全选
	/// 　▢ 多选框A ▢ 多选框B ▢ 多选框C
	/// </summary>
	[ExportMany(ContractKey = typeof(CheckBoxGroupFieldAttribute)), SingletonReuse]
	public class CheckBoxGroupFieldHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (CheckBoxGroupFieldAttribute)field.Attribute;
			var valueAndProvider = ListItemUtils.GetValueAndProvider<IListItemProvider>(
				attribute.Source, field.Value);
			var value = valueAndProvider.First;
			var listItems = valueAndProvider.Second.GetItems().ToList();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fieldHtml = templateManager.RenderTemplate("common.base/tmpl.form.hidden.html", new {
				name = field.Attribute.Name,
				value = JsonConvert.SerializeObject(value ?? new string[0]),
				attributes = htmlAttributes
			});
			var checkboxGroup = templateManager.RenderTemplate("common.base/tmpl.checkbox_group.html", new {
				items = listItems,
				fieldName = field.Attribute.Name,
				fieldHtml = new HtmlString(fieldHtml)
			});
			return field.WrapFieldHtml(htmlAttributes, checkboxGroup);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			var attribute = (CheckBoxGroupFieldAttribute)field.Attribute;
			var parsed = JsonConvert.DeserializeObject<IList<string>>(values[0]);
			return ListItemUtils.WrapValueAndProvider(attribute.Source, parsed);
		}
	}
}
