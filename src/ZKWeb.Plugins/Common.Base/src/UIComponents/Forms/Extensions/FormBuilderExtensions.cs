using DotLiquid;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Base.src.Components.CsrfTokenStore.Interfaces;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Templating;
using ZKWebStandard.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions {
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
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			builder.BindValues(templateManager.CreateHash(obj));
		}

		/// <summary>
		/// 解析提交上来的值
		/// 同时检查CSRF校验和值是否符合验证器的规则
		/// </summary>
		/// <param name="builder">表单构建器</param>
		/// <param name="submitValues">提交上来的值</param>
		/// <returns></returns>
		public static IDictionary<string, object> ParseValues(
			this FormBuilder builder, IDictionary<string, IList<string>> submitValues) {
			// 检查CSRF校验值
			var csrfTokenStore = Application.Ioc.Resolve<ICsrfTokenStore>();
			if (builder.Attribute.EnableCsrfToken) {
				var exceptedToken = csrfTokenStore.GetCsrfToken();
				var actualTokens = submitValues.GetOrDefault(FormBuilder.CsrfTokenFieldName);
				var actualToken = actualTokens == null ? null : actualTokens[0];
				if (string.IsNullOrEmpty(exceptedToken) || exceptedToken != actualToken) {
					throw new ForbiddenException(
						new T("Check CSRF token failed, please refresh this page and try again"));
				}
			}
			// 枚举字段，逐个进行检查和设置
			var result = new Dictionary<string, object>();
			foreach (var field in builder.Fields) {
				// 解析值
				var values = submitValues.GetOrDefault(field.Attribute.Name);
				object parsed = null;
				if (values != null || field.Attribute is IFormFieldParseFromEnv) {
					var handler = Application.Ioc.Resolve<IFormFieldHandler>(
						serviceKey: field.Attribute.GetType());
					parsed = handler.Parse(field, values);
				}
				// 校验值
				foreach (var attribute in field.ValidationAttributes) {
					var validator = Application.Ioc.Resolve<IFormFieldValidator>(
						serviceKey: attribute.GetType());
					validator.Validate(field, attribute, parsed);
				}
				// 设置到结果中
				result[field.Attribute.Name] = parsed;
			}
			return result;
		}
	}
}
