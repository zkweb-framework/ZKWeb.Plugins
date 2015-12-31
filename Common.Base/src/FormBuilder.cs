using DotLiquid;
using DryIoc;
using DryIocAttributes;
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
using ZKWeb.Utils.Functions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 表单构建器
	/// 这个类可以通过Ioc替换，使用时注意要通过Ioc获取
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
	[ExportMany]
	public class FormBuilder : ILiquidizable {
		/// <summary>
		/// Csrf校验值的cookies键名
		/// </summary>
		public const string CsrfTokenKey = "Common.Base.CsrfToken";
		/// <summary>
		/// Csrf校验值的表单字段名称
		/// </summary>
		public const string CsrfTokenFieldName = "__CsrfToken";
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
			Attribute = new FormAttribute(null);
			Fields = new List<FormField>();
		}

		/// <summary>
		/// 描画表单的开始标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderFormBeginTag(HtmlTextWriter html) {
			html.AddAttribute("name", Attribute.Name ?? "");
			html.AddAttribute("action", Attribute.Action ?? "");
			html.AddAttribute("method", Attribute.Method ?? HttpMethods.POST);
			html.AddAttribute("role", "form");
			html.AddAttribute("ajax", Attribute.EnableAjaxSubmit ? "true" : "false");
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
			foreach (var validatorAttribute in field.ValidationAttributes) {
				var validator = Application.Ioc.Resolve<IFormFieldValidator>(serviceKey: validatorAttribute.GetType());
				validator.AddHtmlAttributes(field, validatorAttribute, htmlAttributes);
			}
			// 构建表单字段的html并写入到html构建器
			var fieldHandler = Application.Ioc.Resolve<IFormFieldHandler>(serviceKey: field.Attribute.GetType());
			html.WriteLine(fieldHandler.Build(field, htmlAttributes));
		}

		/// <summary>
		/// 描画csrf校验值
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderCsrfToken(HtmlTextWriter html) {
			// 不需要描画时直接返回
			if (!Attribute.EnableCsrfToken) {
				return;
			}
			// 获取cookies传入的csrf校验值，不存在时设置
			var token = HttpContextUtils.GetCookie(CsrfTokenKey);
			if (string.IsNullOrEmpty(token)) {
				token = HttpServerUtility.UrlTokenEncode(RandomUtils.SystemRandomBytes(20));
				HttpContextUtils.PutCookie(CsrfTokenKey, token);
			}
			// 添加到表单中
			var field = new FormField(new HiddenFieldAttribute(CsrfTokenFieldName));
			field.Value = token;
			RenderFormField(html, field);
		}

		/// <summary>
		/// 描画提交按钮
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderSubmitButton(HtmlTextWriter html) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			html.AddAttribute("class", "form-actions");
			html.RenderBeginTag("div");
			foreach (var pair in provider.SubmitButtonAttributes) {
				html.AddAttribute(pair.Key, pair.Value);
			}
			html.RenderBeginTag("button");
			html.WriteEncodedText(new T(Attribute.SubmitButtonText));
			html.RenderEndTag();
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
			RenderCsrfToken(html);
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
		public List<Attribute> ValidationAttributes { get; set; }
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
			ValidationAttributes = new List<Attribute>();
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
		/// 同时检查csrf校验和值是否符合验证器的规则
		/// </summary>
		/// <param name="builder">表单构建器</param>
		/// <param name="submitValues">提交上来的值</param>
		/// <returns></returns>
		public static IDictionary<string, object> ParseValues(
			this FormBuilder builder, IDictionary<string, string> submitValues) {
			// 检查csrf校验值
			if (builder.Attribute.EnableCsrfToken) {
				var exceptedToken = HttpContextUtils.GetCookie(FormBuilder.CsrfTokenKey);
				var actualToken = submitValues.GetOrDefault(FormBuilder.CsrfTokenFieldName);
				if (string.IsNullOrEmpty(exceptedToken) || exceptedToken != actualToken) {
					throw new FormatException(new T("Check Csrf Token Failed."));
				}
			}
			// 枚举字段，逐个进行检查和设置
			var result = new Dictionary<string, object>();
			foreach (var field in builder.Fields) {
				// 解析值
				string value = submitValues.GetOrDefault(field.Attribute.Name);
				object parsed = null;
				if (value != null) {
					var fieldHandler = Application.Ioc.Resolve<IFormFieldHandler>(serviceKey: field.Attribute.GetType());
					parsed = fieldHandler.Parse(field, value);
				}
				// 校验值
				foreach (var attribute in field.ValidationAttributes) {
					var validator = Application.Ioc.Resolve<IFormFieldValidator>(serviceKey: attribute.GetType());
					validator.Validate(field, attribute, parsed);
				}
				// 设置到结果中
				result[field.Attribute.Name] = parsed;
			}
			return result;
		}
	}
}
