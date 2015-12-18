using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src {
	/// <summary>
	/// 表单管理器
	/// </summary>
	[ExportMany]
	public class FormManager {
		private Dictionary<string, IFormFieldHandler> FieldHandlers =
			new Dictionary<string, IFormFieldHandler>();

		private Dictionary<Type, IFormFieldValidator> FieldValidators { get; } =
			new Dictionary<Type, IFormFieldValidator>();

		public void RegisterHandler(string fieldType, IFormFieldHandler handler) {
			FieldHandlers[fieldType] = handler;
		}

		public void RegisterValidator<TAttribute>(IFormFieldValidator validator) {
			FieldValidators[typeof(TAttribute)] = validator;
		}

		public IFormFieldHandler GetHandler(string fieldType) {
			return FieldHandlers.GetOrDefault(fieldType);
		}

		public IFormFieldValidator GetValidator(Type validatorType) {
			return FieldValidators.GetOrDefault(validatorType);
		}
	}

	public class FormBuilder {
		public override string ToString() {
			return base.ToString();
		}
	}

	public class ModelFormBuilder {
		public override string ToString() {
			return base.ToString();
		}
	}

	public interface IFormFieldValidator {
		Dictionary<string, string> HtmlAttributes(object attribute);
		void Validate(object attribute, string value);
	}

	public interface IFormFieldHandler {
		string Build(object value, Dictionary<string, string> ValidatorAttributes);
		object Parse(string value);
	}

	/// <summary>
	/// 表单字段的属性
	/// </summary>
	public class FormFieldAttribute : Attribute {
		/// <summary>
		/// 字段类型
		/// </summary>
		public string FieldType { get; set; }
		/// <summary>
		/// 字段名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="fieldType">字段类型</param>
		/// <param name="name">字段名称</param>
		public FormFieldAttribute(string fieldType, string name) {
			FieldType = fieldType;
			Name = name;
		}
	}

	/// <summary>
	/// 表单属性
	/// </summary>
	public class FormAttribute : Attribute {
		/// <summary>
		/// 表单名称
		/// </summary>
		public string FormName { get; set; }

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="formName">表单名称</param>
		public FormAttribute(string formName) {
			FormName = formName;
		}
	}
}
