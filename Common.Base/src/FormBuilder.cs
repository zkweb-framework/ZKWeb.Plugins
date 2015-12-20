using DotLiquid;
using DryIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using ZKWeb.Core;
using ZKWeb.Model;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 表单构建器
	/// 创建的例子
	///		var form = new FormBuilder();
	///		form.Attribute = new FormAttribute() { Name = "TestForm" };
	///		form.Fields.Add(new FormField(new TextBoxFieldAttribute("Username")));
	///		form.Fields.Add(new FormField(new PasswordFieldAttribute("Password")));
	/// 绑定的例子
	///		form.BindValuesFromAnonymousObject(new { Username = "TestUser", Password = "TestPassword" });
	///		return new TemplateResult("test_form.html", new { form = form });
	///	提交的例子
	///		var values = form.ParseValues(HttpContext.Current.Request.GetParams());
	///		var username = values.GetOrDefault[string]("Username");
	///		var password = values.GetOrDefault[string]("Password");
	/// </summary>
	public class FormBuilder : ILiquidizable {
		/// <summary>
		/// 表单属性，可省略
		/// </summary>
		public FormAttribute Attribute { get; set; }
		/// <summary>
		/// 表单字段列表
		/// </summary>
		public List<FormField> Fields { get; private set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public FormBuilder() {
			Fields = new List<FormField>();
		}

		/// <summary>
		/// 描画表单的开始标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderFormBeginTag(HtmlTextWriter html) {
			var attribute = Attribute ?? new FormAttribute(null);
			html.AddAttribute("name", attribute.Name ?? "");
			html.AddAttribute("action", attribute.Action ?? "");
			html.AddAttribute("method", attribute.Method ?? HttpMethods.POST);
			html.AddAttribute("role", "form");
			html.AddAttribute("ajax", attribute.EnableAjaxSubmit ? "true" : "false");
			html.RenderBeginTag("form");
		}

		/// <summary>
		/// 描画表单的结束标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderFormEndTag(HtmlTextWriter html) {
			html.RenderEndTag();
		}

		/// <summary>
		/// 描画表单字段
		/// </summary>
		/// <param name="html">html构建器</param>
		/// <param name="field">表单字段</param>
		protected virtual void RenderFormField(HtmlTextWriter html, FormField field) {
			// 根据验证器添加html属性
			var htmlAttributes = new Dictionary<string, string>();
			foreach (var validatorAttribute in field.ValidateAttributes) {
				var validator = Application.Ioc.Resolve<IFormFieldValidator>(serviceKey: validatorAttribute.GetType());
				validator.AddHtmlAttributes(validatorAttribute, htmlAttributes);
			}
			// 构建表单字段的html并写入到html构建器
			var fieldHandler = Application.Ioc.Resolve<IFormFieldHandler>(serviceKey: field.Attribute.GetType());
			html.WriteLine(fieldHandler.Build(field, htmlAttributes));
		}

		/// <summary>
		/// 描画提交按钮
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderSubmitButton(HtmlTextWriter html) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			foreach (var pair in provider.SubmitButtonAttributes) {
				html.AddAttribute(pair.Key, pair.Value);
			}
			html.RenderBeginTag("button");
			html.WriteEncodedText(new T(Attribute.SubmitButtonText));
			html.RenderEndTag();
		}

		/// <summary>
		/// 允许直接描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new HtmlString(this.ToString());
		}

		/// <summary>
		/// 获取表单html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			var html = new HtmlTextWriter(new StringWriter());
			RenderFormBeginTag(html);
			foreach (var field in Fields) {
				RenderFormField(html, field);
			}
			RenderSubmitButton(html);
			RenderFormEndTag(html);
			return html.InnerWriter.ToString();
		}
	}

	/// <summary>
	/// 表单字段
	/// </summary>
	public class FormField {
		/// <summary>
		/// 表单字段的属性
		/// </summary>
		public FormFieldAttribute Attribute { get; set; }
		/// <summary>
		/// 验证表单的属性列表
		/// </summary>
		public List<Attribute> ValidateAttributes { get; set; }
		/// <summary>
		/// 字段的值
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="attribute">表单字段的属性</param>
		public FormField(FormFieldAttribute attribute) {
			Attribute = attribute;
			ValidateAttributes = new List<Attribute>();
			Value = null;
		}
	}

	/// <summary>
	/// 表单构建器的扩展函数
	/// </summary>
	public static class FormBuilderExtensions {
		/// <summary>
		/// 绑定表单字段的值
		/// </summary>
		/// <param name="builder">表单构建器</param>
		/// <param name="values">来源的值</param>
		public static void BindValues(this FormBuilder builder, IDictionary<string, object> values) {
			foreach (var field in builder.Fields) {
				object value;
				if (values.TryGetValue(field.Attribute.Name, out value)) {
					field.Value = value;
				}
			}
		}

		/// <summary>
		/// 绑定表单字段的值
		/// 根据匿名对象，例如new { Username = "", Password = "" }
		/// </summary>
		/// <param name="builder">表单构建器</param>
		/// <param name="obj">对象</param>
		public static void BindValuesFromAnonymousObject(this FormBuilder builder, object obj) {
			builder.BindValues(Hash.FromAnonymousObject(obj));
		}

		/// <summary>
		/// 解析提交上来的值
		/// </summary>
		/// <param name="builder">表单构建器</param>
		/// <param name="submitValues">提交上来的值</param>
		/// <returns></returns>
		public static IDictionary<string, object> ParseValues(
			this FormBuilder builder, IDictionary<string, string> submitValues) {
			var result = new Dictionary<string, object>();
			foreach (var field in builder.Fields) {
				var value = submitValues.GetOrDefault(field.Attribute.Name);
				if (value != null) {
					var fieldHandler = Application.Ioc.Resolve<IFormFieldHandler>(serviceKey: field.Attribute.GetType());
					result[field.Attribute.Name] = fieldHandler.Parse(field, value);
				}
			}
			return result;
		}
	}
}
