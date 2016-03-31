using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 可搜索的下拉列表
	/// </summary>
	[ExportMany(ContractKey = typeof(SearchableDropdownListFieldAttribute)), SingletonReuse]
	public class SearchableDropdownList : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (DropdownListFieldAttribute)field.Attribute;
			var selectHtml = DropdownList.BuildSelectHtml(attribute,
				provider.FormControlAttributes.Concat(htmlAttributes), field.Value);
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.searchable_dropdown_list.html", new { selectHtml });
			return provider.FormGroupHtml(field, htmlAttributes, html);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return value;
		}
	}
}
