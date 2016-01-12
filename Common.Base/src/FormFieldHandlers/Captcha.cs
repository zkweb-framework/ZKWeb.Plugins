using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 验证码
	/// </summary>
	[ExportMany(ContractKey = typeof(CaptchaFieldAttribute)), SingletonReuse]
	public class Captcha : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (CaptchaFieldAttribute)field.Attribute;
			var html = new HtmlTextWriter(new StringWriter());
			// 控件组
			html.AddAttribute("class", "input-group");
			html.RenderBeginTag("div");
			// 输入框
			html.AddAttribute("name", field.Attribute.Name);
			html.AddAttribute("value", (field.Value ?? "").ToString());
			html.AddAttribute("type", "text");
			html.AddAttribute("placeholder", new T(attribute.PlaceHolder));
			html.AddAttributes(provider.FormControlAttributes);
			html.AddAttributes(htmlAttributes);
			html.RenderBeginTag("input");
			html.RenderEndTag();
			// 验证码图片
			html.AddAttribute("class", "input-group-addon");
			html.RenderBeginTag("span");
			html.AddAttribute("alt", new T("Captcha"));
			html.AddAttribute("class", "captcha");
			html.AddAttribute("src", "/captcha?key=" + attribute.Key);
			html.AddAttribute("title", new T("Click to change captcha image"));
			html.RenderBeginTag("img");
			html.RenderEndTag(); // img
			html.RenderEndTag(); // span
			html.RenderEndTag(); // div
			return provider.FormGroupHtml(field, htmlAttributes, html.InnerWriter.ToString());
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			// 检查验证码
			var attribute = (CaptchaFieldAttribute)field.Attribute;
			var captchaManager = Application.Ioc.Resolve<CaptchaManager>();
			if (!captchaManager.Check(attribute.Key, value)) {
				throw new HttpException(400, new T("Incorrect captcha"));
			}
			return value;
		}
	}
}
