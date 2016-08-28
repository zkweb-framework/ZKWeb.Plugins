using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Captcha.src.Managers;
using ZKWebStandard.Ioc;
using ZKWeb.Templating;
using ZKWeb.Plugins.Common.Captcha.src.UIComponents.Form.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Form.Extensions;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;

namespace ZKWeb.Plugins.Common.Captcha.src.UIComponents.Handlers {
	/// <summary>
	/// 验证码
	/// </summary>
	[ExportMany(ContractKey = typeof(CaptchaFieldAttribute)), SingletonReuse]
	public class CaptchaHandler : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, IDictionary<string, string> htmlAttributes) {
			var attribute = (CaptchaFieldAttribute)field.Attribute;
			var captchaManager = Application.Ioc.Resolve<CaptchaManager>();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var catpcha = templateManager.RenderTemplate("common.captcha/tmpl.form.captcha.html", new {
				name = attribute.Name,
				value = (field.Value ?? "").ToString(),
				placeholder = new T(attribute.PlaceHolder),
				attributes = htmlAttributes,
				key = attribute.Key,
				supportCaptchaAudio = captchaManager.SupportCaptchaAudio
			});
			return field.WrapFieldHtml(htmlAttributes, catpcha);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, IList<string> values) {
			// 检查验证码
			var attribute = (CaptchaFieldAttribute)field.Attribute;
			var value = values[0];
			var captchaManager = Application.Ioc.Resolve<CaptchaManager>();
			if (!captchaManager.Check(attribute.Key, value)) {
				throw new ForbiddenException(new T("Incorrect captcha"));
			}
			return value;
		}
	}
}
