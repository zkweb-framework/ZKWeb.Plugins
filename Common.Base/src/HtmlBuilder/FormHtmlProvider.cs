using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.FormFieldValidators;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Common.Base.src.HtmlBuilder {
	/// <summary>
	/// 表单html提供器
	///	这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// </summary>
	[ExportMany, SingletonReuse]
	public class FormHtmlProvider {
		/// <summary>
		/// 表单控件的默认属性
		/// </summary>
		public virtual IDictionary<string, string> FormControlAttributes { get { return _FormControlAttributes; } }
		private Dictionary<string, string> _FormControlAttributes = new Dictionary<string, string>() {
			{ "class", "form-control" },
			{ "data-val", "true" }
		};

		/// <summary>
		/// 提交按钮的默认属性
		/// </summary>
		public virtual IDictionary<string, string> SubmitButtonAttributes { get { return _SubmitButtonAttributes; } }
		private Dictionary<string, string> _SubmitButtonAttributes = new Dictionary<string, string>() {
			{ "class", "btn btn-submit" },
			{ "type", "submit" }
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
