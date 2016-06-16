using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;
using ZKWeb.Plugins.Common.Base.src.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 可搜索的下拉列表
	/// </summary>
	[ExportMany(ContractKey = typeof(SearchableDropdownListFieldAttribute)), SingletonReuse]
	public class SearchableDropdownList : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (SearchableDropdownListFieldAttribute)field.Attribute;
			var selectHtml = DropdownList.BuildSelectHtml(attribute, htmlAttributes, field.Value);
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var searchableDropdown = templateManager.RenderTemplate(
				"common.base/tmpl.searchable_dropdown_list.html", new { selectHtml });
			return field.WrapFieldHtml(htmlAttributes, searchableDropdown);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			return values;
		}
	}
}
