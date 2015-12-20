using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.FormFieldValidators;

namespace ZKWeb.Plugins.Common.Base.src {
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
			{ "class", "form-control" }
		};

		/// <summary>
		/// 提交按钮的默认属性
		/// </summary>
		public virtual IDictionary<string, string> SubmitButtonAttributes { get { return _SubmitButtonAttributes; } }
		private Dictionary<string, string> _SubmitButtonAttributes = new Dictionary<string, string>() {
			{ "class", "btn green" },
			{ "type", "submit" }
		};

		/// <summary>
		/// 构建表单组的html
		/// 包含
		///		名称，控件，验证文本
		/// 结构
		/// div.form-group
		///		label.control-label
		///		div.form-controls
		///			控件
		///			span.field-validation-valid
		/// </summary>
		/// <param name="field">表单字段</param>
		/// <param name="htmlAttributes">添加到元素的html属性</param>
		/// <param name="control">控件</param>
		/// <returns></returns>
		public virtual string FormGroupHtml(
			FormField field, Dictionary<string, string> htmlAttributes, string control) {
			var html = new StringBuilder();
			html.AppendLine("<div class='form-group'>");
			html.AppendLine("	<label class='control-label'>");
			html.AppendLine("		" + HttpUtility.HtmlEncode(new T(field.Attribute.Name)));
			if (htmlAttributes.ContainsKey(Required.Key)) {
				html.AppendLine("		<em>*</em>");
			}
			html.AppendLine("	</label>");
			html.AppendLine("	<div class='form-controls'>");
			html.AppendLine("		" + control);
			html.AppendLine("		<span class='field-validation-valid' data-valmsg-for='" +
									HttpUtility.HtmlAttributeEncode(field.Attribute.Name) +
									"' data-valmsg-replace='true'></span>");
			html.AppendLine("	</div>");
			html.AppendLine("</div>");
			return html.ToString();
		}
	}
}
