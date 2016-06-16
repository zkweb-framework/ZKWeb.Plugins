using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZKWeb.Plugins.Common.Base.src.FormFieldValidators;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Base.src.Scaffolding {
	/// <summary>
	/// 表单html提供器
	///	这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// </summary>
	[ExportMany, SingletonReuse]
	public class FormHtmlProvider_Obslete {
		/// <summary>
		/// 表单控件的默认属性
		/// </summary>
		public virtual IDictionary<string, string> FormControlAttributes { get { return _FormControlAttributes; } }
		private Dictionary<string, string> _FormControlAttributes = new Dictionary<string, string>() {
			{ "class", "form-control" },
			{ "data-val", "true" }
		};
		

		/// <summary>
		/// 构建表单组的html
		/// 包含
		///		名称，控件，验证文本
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <param name="htmlAttributes">添加到元素的html属性</param>
		/// <param name="control">控件</param>
		/// <returns></returns>
		public virtual string FormGroupHtml(
			FormField field, Dictionary<string, string> htmlAttributes, string control) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.form_group.html", new {
				name = field.Attribute.Name,
				required = htmlAttributes.ContainsKey(Required.Key),
				control = new HtmlString(control)
			});
			return html;
		}
	}
}
