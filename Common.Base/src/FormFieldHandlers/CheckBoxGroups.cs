using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 勾选框分组列表
	/// 显示结构
	/// ▢ 全选
	/// 　▢ 分组A
	/// 　　▢ 多选框AA ▢ 多选框AB ▢ 多选框AC
	/// 　▢ 分组B
	/// 　　▢ 多选框BA ▢ 多选框BB ▢ 多选框BC
	/// </summary>
	[ExportMany(ContractKey = typeof(CheckBoxGroupsFieldAttribute)), SingletonReuse]
	public class CheckBoxGroups : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (CheckBoxGroupsFieldAttribute)field.Attribute;
			var listItemGroupsProvider = (IListItemGroupsProvider)Activator.CreateInstance(attribute.Source);
			var listItemGroups = listItemGroupsProvider.GetGroups().ToList();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fieldHtml = new HtmlTextWriter(new StringWriter());
			fieldHtml.AddAttribute("name", field.Attribute.Name);
			fieldHtml.AddAttribute("value", JsonConvert.SerializeObject(field.Value ?? new string[0]));
			fieldHtml.AddAttribute("type", "hidden");
			fieldHtml.AddAttributes(provider.FormControlAttributes);
			fieldHtml.AddAttributes(htmlAttributes);
			fieldHtml.RenderBeginTag("input");
			fieldHtml.RenderEndTag();
			var html = templateManager.RenderTemplate("common.base/tmpl.checkbox_groups.html", new {
				groups = listItemGroups.Select(g => new { key = g.Key, items = g }),
				fieldName = field.Attribute.Name,
				fieldHtml = new HtmlString(fieldHtml.InnerWriter.ToString())
			});
			return provider.FormGroupHtml(field, htmlAttributes, html);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return JsonConvert.DeserializeObject<IList<string>>(value);
		}
	}
}
