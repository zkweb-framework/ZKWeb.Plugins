using DotLiquid;
using System.Collections.Generic;
using System.Text;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.CsrfTokenStore.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Templating;
using ZKWeb.Web;
using ZKWebStandard.Collection;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;
using ZKWebStandard.Utils;
using ZKWebStandard.Web;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms {
	/// <summary>
	/// 表单构建器
	/// 这个类可以通过Ioc替换，使用时注意要通过Ioc获取
	/// </summary>
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
	[ExportMany]
	public class FormBuilder : ILiquidizable {
		/// <summary>
		/// Csrf校验值的表单字段名称
		/// </summary>
		public const string CsrfTokenFieldName = "__CSRFTOKEN";
		/// <summary>
		/// 表单属性，可省略
		/// </summary>
		public FormAttribute Attribute { get; set; }
		/// <summary>
		/// 表单字段列表
		/// </summary>
		public List<FormField> Fields { get; protected set; }
		/// <summary>
		/// 当前客户端使用的csrf校验值
		/// </summary>
		public string CsrfToken { get; protected set; }

		/// <summary>
		/// 初始化
		/// </summary>
		public FormBuilder() {
			Attribute = new FormAttribute(null);
			Fields = new List<FormField>();
			if (Attribute.EnableCsrfToken) {
				// 获取客户端传入的csrf校验值，不存在时设置
				var csrfTokenStore = Application.Ioc.Resolve<ICsrfTokenStore>();
				CsrfToken = csrfTokenStore.GetCsrfToken();
				if (string.IsNullOrEmpty(CsrfToken)) {
					CsrfToken = RandomUtils.SystemRandomBytes(20).ToHex();
					csrfTokenStore.SetCsrfToken(CsrfToken);
				}
			}
		}

		/// <summary>
		/// 描画表单的开始标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderFormBeginTag(StringBuilder html) {
			var request = HttpManager.CurrentContext.Request;
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			html.AppendLine(
				templateManager.RenderTemplate("common.base/tmpl.form.begin_tag.html", new {
					name = Attribute.Name ?? "",
					action = Attribute.Action ?? (request.Path + request.QueryString),
					method = Attribute.Method ?? HttpMethods.POST,
					ajax = Attribute.EnableAjaxSubmit ? "true" : "false",
					cssClass = Attribute.CssClass
				}));
		}

		/// <summary>
		/// 描画表单的结束标签
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderFormEndTag(StringBuilder html) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			html.AppendLine(templateManager.RenderTemplate("common.base/tmpl.form.end_tag.html", null));
		}

		/// <summary>
		/// 描画表单字段
		/// </summary>
		/// <param name="html">html构建器</param>
		/// <param name="field">表单字段</param>
		protected virtual void RenderFormField(StringBuilder html, FormField field) {
			html.AppendLine(field.ToString());
		}

		/// <summary>
		/// 描画csrf校验值
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderCsrfToken(StringBuilder html) {
			if (Attribute.EnableCsrfToken) {
				// 添加csrf校验值到表单
				var field = new FormField(new HiddenFieldAttribute(CsrfTokenFieldName));
				field.Value = CsrfToken;
				RenderFormField(html, field);
			}
		}

		/// <summary>
		/// 描画提交按钮
		/// </summary>
		/// <param name="html">html构建器</param>
		protected virtual void RenderSubmitButton(StringBuilder html) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			html.AppendLine(
				templateManager.RenderTemplate("common.base/tmpl.form.submit_button.html", new {
					buttonText = new T(Attribute.SubmitButtonText)
				}));
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
			var html = new StringBuilder();
			RenderFormBeginTag(html);
			foreach (var field in Fields) {
				RenderFormField(html, field);
			}
			RenderCsrfToken(html);
			RenderSubmitButton(html);
			RenderFormEndTag(html);
			return html.ToString();
		}
	}
}
