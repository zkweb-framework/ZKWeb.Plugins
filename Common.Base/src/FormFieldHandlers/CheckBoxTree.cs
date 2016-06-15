using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 勾选框树
	/// 显示结构
	/// ▢ 节点A
	/// 　▢ 节点AA
	/// 　　▢ 节点AAA
	/// 　▢ 节点AB
	/// ▢ 节点B
	/// </summary>
	[ExportMany(ContractKey = typeof(CheckBoxTreeFieldAttribute)), SingletonReuse]
	public class CheckBoxTree : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (CheckBoxTreeFieldAttribute)field.Attribute;
			var listItemTreeProvider = (IListItemTreeProvider)Activator.CreateInstance(attribute.Source);
			var listItemTree = listItemTreeProvider.GetTree();
			var items = listItemTree.EnumerateAllNodes().Where(n => n.Value != null).Select(n => new {
				name = n.Value.Name,
				value = n.Value.Value,
				level = n.GetParents().Count() - 1
			});
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fieldHtml = new HtmlTextWriter(new StringWriter());
			fieldHtml.AddAttribute("name", field.Attribute.Name);
			fieldHtml.AddAttribute("value", JsonConvert.SerializeObject(field.Value ?? new string[0]));
			fieldHtml.AddAttribute("type", "hidden");
			fieldHtml.AddAttributes(provider.FormControlAttributes);
			fieldHtml.AddAttributes(htmlAttributes);
			fieldHtml.RenderBeginTag("input");
			fieldHtml.RenderEndTag();
			var html = templateManager.RenderTemplate("common.base/tmpl.checkbox_tree.html", new {
				items,
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
