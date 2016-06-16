using DotLiquid;
using System.Collections.Generic;
using System.IO;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.Scaffolding {
	/// <summary>
	/// 表单构建器
	/// 这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// <example>
	/// // 创建
	/// var form = Application.Ioc.Resolve[FormBuilder]();
	/// form.Attribute = new FormAttribute() { Name = "TestForm" };
	/// form.Fields.Add(new FormField(new TextBoxFieldAttribute("Username")));
	/// form.Fields.Add(new FormField(new PasswordFieldAttribute("Password")));
	/// // 绑定
	/// form.BindValuesFromAnonymousObject(new { Username = "TestUser", Password = "TestPassword" });
	/// return new TemplateResult("test_form.html", new { form });
	/// // 提交
	/// var values = form.ParseValues(HttpManager.CurrentContext.Request.GetParams());
	/// var username = values.GetOrDefault[string]("Username");
	/// var password = values.GetOrDefault[string]("Password");
	/// </example>
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
		public List<FormField> Fields { get; protected set; }

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
			var request = HttpManager.CurrentContext.Request;
			html.AddAttribute("name", Attribute.Name ?? "");
			html.AddAttribute("action", Attribute.Action ?? (request.Path + request.QueryString));
			html.AddAttribute("method", Attribute.Method ?? HttpMethods.POST);
			html.AddAttribute("role", "form");
			html.AddAttribute("ajax", Attribute.EnableAjaxSubmit ? "true" : "false");
			html.AddAttribute("class", Attribute.CssClass);
			html.RenderBeginTag("form");
		}

		/// <summary>
		/// 描画表单的结束标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderFormEndTag(HtmlTextWriter html) {
			html.RenderEndTag(); // form
		}

		/// <summary>
		/// 描画表单字段
		/// </summary>
		/// <param name="html">html构建器</param>
		/// <param name="field">表单字段</param>
		protected virtual void RenderFormField(HtmlTextWriter html, FormField field) {
			html.WriteLine(field);
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
			var context = HttpManager.CurrentContext;
			var token = context.GetCookie(CsrfTokenKey);
			if (string.IsNullOrEmpty(token)) {
				token = RandomUtils.SystemRandomBytes(20).ToHex();
				context.PutCookie(CsrfTokenKey, token);
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
			html.RenderEndTag(); // button
			html.RenderEndTag(); // div
		}

		/// <summary>
		/// 允许直接描画到模板
		/// </summary>
		/// <returns></returns>
		object ILiquidizable.ToLiquid() {
			return new HtmlString(ToString());
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
			this FormBuilder builder, IDictionary<string, IList<string>> submitValues) {
			// 检查csrf校验值
			var context = HttpManager.CurrentContext;
			if (builder.Attribute.EnableCsrfToken) {
				var exceptedToken = context.GetCookie(FormBuilder.CsrfTokenKey);
				var actualTokens = submitValues.GetOrDefault(FormBuilder.CsrfTokenFieldName);
				var actualToken = actualTokens == null ? null : actualTokens[0];
				if (string.IsNullOrEmpty(exceptedToken) || exceptedToken != actualToken) {
					throw new HttpException(403, new T("Check Csrf Token Failed."));
				}
			}
			// 枚举字段，逐个进行检查和设置
			var result = new Dictionary<string, object>();
			foreach (var field in builder.Fields) {
				// 解析值
				var values = submitValues.GetOrDefault(field.Attribute.Name);
				object parsed = null;
				if (values != null || field.Attribute is IFormFieldParseFromEnv) {
					var fieldHandler = Application.Ioc.Resolve<IFormFieldHandler>(serviceKey: field.Attribute.GetType());
					parsed = fieldHandler.Parse(field, values);
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
