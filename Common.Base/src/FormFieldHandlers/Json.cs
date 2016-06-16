using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.Scaffolding;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 经过Json序列化的隐藏字段
	/// </summary>
	[ExportMany(ContractKey = typeof(JsonFieldAttribute)), SingletonReuse]
	public class Json : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			/*var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var html = new HtmlTextWriter(new StringWriter());
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", JsonConvert.SerializeObject(field.Value));
			html.AddAttribute("type", "hidden");
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("input");
			html.RenderEndTag();
			return html.InnerWriter.ToString();*/
			throw new NotImplementedException(); // TODO: FIXME
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			var attribute = (JsonFieldAttribute)field.Attribute;
			return JsonConvert.DeserializeObject(value, attribute.FieldType);
		}
	}
}
